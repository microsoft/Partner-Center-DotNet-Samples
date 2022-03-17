using System.Threading;

namespace NCEBulkMigrationTool;

internal class NewCommerceMigrationProvider : INewCommerceMigrationProvider
{
    private readonly ITokenProvider tokenProvider;

    public NewCommerceMigrationProvider(ITokenProvider tokenProvider)
    {
        this.tokenProvider = tokenProvider;
    }

    public async Task<bool> UploadNewCommerceMigrationsAsync()
    {
        var csvProvider = new CsvProvider();

        var inputFileNames = Directory.EnumerateFiles($"{Constants.InputFolderPath}/subscriptions");
        var authenticationResult = await this.tokenProvider.GetTokenAsync();

        foreach (var fileName in inputFileNames)
        {
            Console.WriteLine($"Processing file {fileName}");

            using TextReader fileReader = File.OpenText(fileName);
            using var csvReader = new CsvReader(fileReader, CultureInfo.InvariantCulture, leaveOpen: true);
            var inputMigrationRequests = csvReader.GetRecords<MigrationRequest>().ToList();

            if (inputMigrationRequests.Count > 100)
            {
                Console.WriteLine($"There are too many migration requests in the file: {fileName}. The maximum limit for migration uploads per file is 100. Please fix the input file to continue...");
                continue;
            }

            var migrations = new ConcurrentBag<IEnumerable<MigrationResult>>();

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticationResult.AccessToken);
            httpClient.DefaultRequestHeaders.Add(Constants.PartnerCenterClientHeader, Constants.ClientName);

            var options = new ParallelOptions()
            {
                MaxDegreeOfParallelism = 5
            };

            long subscriptionsCntr = 0; //The counter to track
            var batchId = Guid.NewGuid().ToString();

            var inputBaseMigrationRequests = inputMigrationRequests.Where(m => !m.AddOn && m.MigrationEligible);
            var inputAddOnMigrationRequests = inputMigrationRequests.Where(m => m.AddOn && m.MigrationEligible);

            await Parallel.ForEachAsync(inputBaseMigrationRequests, options, async (migrationRequest, cancellationToken) =>
            {
                try
                {
                    var newCommerceMigration = await this.PostNewCommerceMigrationAsync(httpClient, migrationRequest, inputAddOnMigrationRequests, batchId, cancellationToken);
                    migrations.Add(newCommerceMigration);
                }
                catch (Exception)
                {
                    Console.WriteLine($"Migration for subscription: {migrationRequest.LegacySubscriptionId} failed.");
                }
                finally
                {
                    Interlocked.Increment(ref subscriptionsCntr);
                    Console.WriteLine($"Processed {subscriptionsCntr} subscription migration requests.", subscriptionsCntr);
                }
            });

            csvReader.Dispose();
            fileReader.Close();

            var index = fileName.LastIndexOf('\\');
            var processedFileName = fileName[++index..];

            Console.WriteLine("Exporting migrations");
            await csvProvider.ExportCsv(migrations.SelectMany(m => m), $"{Constants.OutputFolderPath}/migrations/{processedFileName}_{batchId}.csv");
            
            File.Move(fileName, $"{Constants.InputFolderPath}/subscriptions/processed/{processedFileName}", true);

            await Task.Delay(1000 * 60);

            Console.WriteLine($"Exported migrations at {Environment.CurrentDirectory}/{Constants.OutputFolderPath}/migrations/{processedFileName}_{batchId}.csv");
        }

        return true;
    }

    public async Task<bool> ExportNewCommerceMigrationStatusAsync()
    {
        var csvProvider = new CsvProvider();

        var inputFileNames = Directory.EnumerateFiles($"{Constants.InputFolderPath}/migrations");
        var authenticationResult = await this.tokenProvider.GetTokenAsync();

        foreach (var fileName in inputFileNames)
        {
            Console.WriteLine($"Processing file {fileName}");
            using TextReader fileReader = File.OpenText(fileName);
            using var csvReader = new CsvReader(fileReader, CultureInfo.InvariantCulture, leaveOpen: true);
            var inputMigrations = csvReader.GetRecords<MigrationResult>().ToList();

            var migrations = new ConcurrentBag<MigrationResult>();

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticationResult.AccessToken);

            var options = new ParallelOptions()
            {
                MaxDegreeOfParallelism = 5
            };

            long migrationsCntr = 0; //The counter to track
            var batchId = Guid.NewGuid().ToString();

            var inputBaseMigrationRequests = inputMigrations.Where(m => !m.AddOn);
            var inputAddOnMigrationRequests = inputMigrations.Where(m => m.AddOn);

            await Parallel.ForEachAsync(inputBaseMigrationRequests, options, async (migration, cancellationToken) =>
            {
                try
                {
                    var newCommerceMigration = await this.GetNewCommerceMigrationByMigrationIdAsync(httpClient, migration, cancellationToken);
                    migrations.Add(newCommerceMigration.BaseMigrationResult);
                    if (newCommerceMigration.AddOnMigrationsResult?.Any() == true)
                    {
                        foreach (var addOnMigrationResult in newCommerceMigration.AddOnMigrationsResult)
                        {
                            var addOnMigration = inputAddOnMigrationRequests.Single(a => a.LegacySubscriptionId.Equals(addOnMigrationResult.CurrentSubscriptionId, StringComparison.OrdinalIgnoreCase));
                            addOnMigration = addOnMigration with
                            {
                                NCESubscriptionId = addOnMigrationResult.NewCommerceSubscriptionId,
                                MigrationId = newCommerceMigration.BaseMigrationResult.MigrationId,
                                MigrationStatus = newCommerceMigration.BaseMigrationResult.MigrationStatus,
                            };

                            migrations.Add(addOnMigration);
                        }
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine($"Couldn't retrieve status for migrationId: {migration.MigrationId}.");
                }
                finally
                {
                    Interlocked.Increment(ref migrationsCntr);
                    Console.WriteLine($"Processed {migrationsCntr} migration status lookups.", migrationsCntr);
                }
            });

            csvReader.Dispose();
            fileReader.Close();

            var index = fileName.LastIndexOf('\\');
            var processedFileName = fileName[++index..];

            Console.WriteLine("Exporting migration status.");
            await csvProvider.ExportCsv(migrations, $"{Constants.OutputFolderPath}/migrationstatus/{processedFileName}.csv");

            File.Move(fileName, $"{Constants.InputFolderPath}/migrations/processed/{processedFileName}", true);

            await Task.Delay(1000 * 60);

            Console.WriteLine($"Exported migration status at {Environment.CurrentDirectory}/{Constants.OutputFolderPath}/migrationstatus/{processedFileName}.csv");
        }

        return true;
    }

    private async Task<(MigrationResult BaseMigrationResult, IEnumerable<NewCommerceMigration> AddOnMigrationsResult)> GetNewCommerceMigrationByMigrationIdAsync(HttpClient httpClient, MigrationResult migrationResult, CancellationToken cancellationToken)
    {
        // Validate that the migration result has a migrationId, if a migration didn't initiate the migrationId will be empty.
        if (string.IsNullOrWhiteSpace(migrationResult.MigrationId))
        {
            // We cannot determine the status, we should return this migration result.
            return (migrationResult, Enumerable.Empty<NewCommerceMigration>());
        }

        var getNewCommerceMigration = new HttpRequestMessage(HttpMethod.Get, string.Format(Routes.GetNewCommerceMigration, migrationResult.CustomerTenantId, migrationResult.MigrationId));

        getNewCommerceMigration.Headers.Add("MS-CorrelationId", Guid.NewGuid().ToString());

        var migrationResponse = await httpClient.SendAsync(getNewCommerceMigration, cancellationToken).ConfigureAwait(false);
        if (migrationResponse.StatusCode == HttpStatusCode.Unauthorized)
        {
            var authenticationResult = await this.tokenProvider.GetTokenAsync();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticationResult.AccessToken);
            httpClient.DefaultRequestHeaders.Add(Constants.PartnerCenterClientHeader, Constants.ClientName);
            getNewCommerceMigration = new HttpRequestMessage(HttpMethod.Get, string.Format(Routes.GetNewCommerceMigration, migrationResult.CustomerTenantId, migrationResult.MigrationId));
            getNewCommerceMigration.Headers.Add("MS-CorrelationId", Guid.NewGuid().ToString());
            migrationResponse = await httpClient.SendAsync(getNewCommerceMigration).ConfigureAwait(false);
        }

        NewCommerceMigrationError? migrationError = null;
        NewCommerceMigration? migration = null;

        if (migrationResponse.IsSuccessStatusCode)
        {
            migration = await migrationResponse.Content.ReadFromJsonAsync<NewCommerceMigration>().ConfigureAwait(false);
        }
        else
        {
            migrationError = await migrationResponse.Content.ReadFromJsonAsync<NewCommerceMigrationError>().ConfigureAwait(false);
        }

        var result = this.PrepareMigrationResult(migrationResult, migrationResult.BatchId, migration, migrationError);
        return (result, migration?.AddOnMigrations);
    }

    private async Task<List<MigrationResult>> PostNewCommerceMigrationAsync(HttpClient httpClient, MigrationRequest migrationRequest, IEnumerable<MigrationRequest> addOnMigrationRequests, string batchId, CancellationToken cancellationToken)
    {
        var newCommerceMigrationRequest = new HttpRequestMessage(HttpMethod.Post, string.Format(Routes.PostNewCommerceMigration, migrationRequest.CustomerTenantId));

        newCommerceMigrationRequest.Headers.Add("MS-CorrelationId", Guid.NewGuid().ToString());

        NewCommerceMigration newCommerceMigration = new NewCommerceMigration
        {
            CurrentSubscriptionId = migrationRequest.LegacySubscriptionId,
            Quantity = migrationRequest.SeatCount,
            BillingCycle = migrationRequest.BillingPlan,
            TermDuration = migrationRequest.Term,
            ExternalReferenceId = batchId,
        };

        // If they want to start a new term, then we should take the input from the file.
        if (migrationRequest.StartNewTermInNce)
        {
            newCommerceMigration.PurchaseFullTerm = true;
        }

        newCommerceMigration.AddOnMigrations = GetAddOnMigrations(migrationRequest.LegacySubscriptionId, addOnMigrationRequests);

        newCommerceMigrationRequest.Content = new StringContent(JsonSerializer.Serialize(newCommerceMigration), Encoding.UTF8, "application/json");

        var migrationResponse = await httpClient.SendAsync(newCommerceMigrationRequest, cancellationToken).ConfigureAwait(false);
        if (migrationResponse.StatusCode == HttpStatusCode.Unauthorized)
        {
            var authenticationResult = await this.tokenProvider.GetTokenAsync();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticationResult.AccessToken);
            httpClient.DefaultRequestHeaders.Add(Constants.PartnerCenterClientHeader, Constants.ClientName);
            newCommerceMigrationRequest = new HttpRequestMessage(HttpMethod.Post, string.Format(Routes.PostNewCommerceMigration, migrationRequest.CustomerTenantId));
            newCommerceMigrationRequest.Headers.Add("MS-CorrelationId", Guid.NewGuid().ToString());
            newCommerceMigrationRequest.Content = new StringContent(JsonSerializer.Serialize(newCommerceMigration), Encoding.UTF8, "application/json");
            migrationResponse = await httpClient.SendAsync(newCommerceMigrationRequest).ConfigureAwait(false);
        }

        NewCommerceMigrationError? migrationError = null;
        NewCommerceMigration? migration = null;

        if (migrationResponse.IsSuccessStatusCode)
        {
            migration = await migrationResponse.Content.ReadFromJsonAsync<NewCommerceMigration>().ConfigureAwait(false);
        }
        else
        {
            migrationError = await migrationResponse.Content.ReadFromJsonAsync<NewCommerceMigrationError>().ConfigureAwait(false);
        }

        return this.PrepareMigrationResult(migrationRequest, addOnMigrationRequests, batchId, migration, migrationError);
    }

    private static IEnumerable<NewCommerceMigration> GetAddOnMigrations(string currentSubscriptionId, IEnumerable<MigrationRequest> addOnMigrationRequests)
    {
        if (!addOnMigrationRequests.Any())
        {
            return Enumerable.Empty<NewCommerceMigration>();
        }

        var allAddOnMigrations = new List<NewCommerceMigration>();

        var childAddOns = addOnMigrationRequests.Where(a => a.BaseSubscriptionId.Equals(currentSubscriptionId, StringComparison.OrdinalIgnoreCase));

        if (childAddOns.Any())
        {
            var addOnNewCommerceMigrations = childAddOns.Select(request => new NewCommerceMigration
            {
                CurrentSubscriptionId = request.LegacySubscriptionId,
                Quantity = request.SeatCount,
                BillingCycle = request.BillingPlan,
                TermDuration = request.Term,
                PurchaseFullTerm = request.StartNewTermInNce,
            });

            allAddOnMigrations.AddRange(addOnNewCommerceMigrations);

            foreach (var item in childAddOns)
            {
                var multiLevelAddons = GetAddOnMigrations(item.LegacySubscriptionId, addOnMigrationRequests);
                allAddOnMigrations.AddRange(multiLevelAddons);
            }
        }

        return allAddOnMigrations;
    }

    /// <summary>
    /// Prepares the MigrationResult for the CSV output. This takes the request and transforms it into a migration result.
    /// </summary>
    /// <param name="migrationRequest">The migration request.</param>
    /// <param name="batchId">The batchId.</param>
    /// <param name="newCommerceMigration">The new commerce migration response.</param>
    /// <param name="newCommerceMigrationError">The new commerce migration error.</param>
    /// <returns>The MigrationResult mapping.</returns>
    private List<MigrationResult> PrepareMigrationResult(MigrationRequest migrationRequest, IEnumerable<MigrationRequest> addOnMigrationRequests, string batchId, NewCommerceMigration? newCommerceMigration = null, NewCommerceMigrationError? newCommerceMigrationError = null)
    {
        var migrationResults = new List<MigrationResult>();
        PrepareMigrationResult(migrationRequest, batchId, newCommerceMigration, newCommerceMigrationError, migrationResults);

        if (newCommerceMigration?.AddOnMigrations.Any() == true)
        {
            PrepareAddOnMigrationResult(addOnMigrationRequests, batchId, newCommerceMigration, newCommerceMigrationError, migrationResults);
        }

        return migrationResults;
    }

    private List<MigrationResult> PrepareAddOnMigrationResult(IEnumerable<MigrationRequest> addOnMigrationRequests, string batchId, NewCommerceMigration? newCommerceMigration, NewCommerceMigrationError? newCommerceMigrationError, List<MigrationResult> migrationResults)
    {
        foreach (var addOnMigrationResponse in newCommerceMigration.AddOnMigrations)
        {
            var addOnMigrationRequest = addOnMigrationRequests.SingleOrDefault(n => n.LegacySubscriptionId.Equals(addOnMigrationResponse.CurrentSubscriptionId, StringComparison.OrdinalIgnoreCase));
            addOnMigrationResponse.Status = newCommerceMigration.Status;
            addOnMigrationResponse.Id = newCommerceMigration.Id;
            PrepareMigrationResult(addOnMigrationRequest, batchId, addOnMigrationResponse, newCommerceMigrationError, migrationResults);
        }

        return migrationResults;
    }

    private static void PrepareMigrationResult(MigrationRequest migrationRequest, string batchId, NewCommerceMigration? newCommerceMigration, NewCommerceMigrationError? newCommerceMigrationError, List<MigrationResult> migrationResults)
    {
        if (newCommerceMigrationError != null)
        {
            var migrationResult = new MigrationResult
            {
                PartnerTenantId = migrationRequest.PartnerTenantId,
                IndirectResellerMpnId = migrationRequest.IndirectResellerMpnId,
                CustomerName = migrationRequest.CustomerName,
                CustomerTenantId = migrationRequest.CustomerTenantId,
                LegacySubscriptionId = migrationRequest.LegacySubscriptionId,
                LegacySubscriptionName = migrationRequest.LegacySubscriptionName,
                LegacyProductName = migrationRequest.LegacyProductName,
                ExpirationDate = migrationRequest.ExpirationDate,
                AddOn = migrationRequest.AddOn,
                StartedNewTermInNce = migrationRequest.StartNewTermInNce,
                NCETermDuration = migrationRequest.Term,
                NCEBillingPlan = migrationRequest.BillingPlan,
                NCESeatCount = migrationRequest.SeatCount,
                ErrorCode = newCommerceMigrationError.Code,
                ErrorReason = newCommerceMigrationError.Description,
            };

            migrationResults.Add(migrationResult);
        }

        if (newCommerceMigration != null)
        {
            var migrationResult = new MigrationResult
            {
                PartnerTenantId = migrationRequest.PartnerTenantId,
                IndirectResellerMpnId = migrationRequest.IndirectResellerMpnId,
                CustomerName = migrationRequest.CustomerName,
                CustomerTenantId = migrationRequest.CustomerTenantId,
                LegacySubscriptionId = migrationRequest.LegacySubscriptionId,
                LegacySubscriptionName = migrationRequest.LegacySubscriptionName,
                LegacyProductName = migrationRequest.LegacyProductName,
                ExpirationDate = migrationRequest.ExpirationDate,
                AddOn = migrationRequest.AddOn,
                MigrationStatus = newCommerceMigration.Status,
                StartedNewTermInNce = migrationRequest.StartNewTermInNce,
                NCETermDuration = newCommerceMigration.TermDuration,
                NCEBillingPlan = newCommerceMigration.BillingCycle,
                NCESeatCount = newCommerceMigration.Quantity,
                NCESubscriptionId = newCommerceMigration.NewCommerceSubscriptionId,
                BatchId = batchId,
                MigrationId = newCommerceMigration.Id,
            };

            migrationResults.Add(migrationResult);
        }
    }

    /// <summary>
    /// Prepares the MigrationResult for the CSV output. This function takes a migration result and generates a new migration result based on the data the API returned.
    /// </summary>
    /// <param name="migrationRequest">The migration request.</param>
    /// <param name="batchId">The batchId.</param>
    /// <param name="newCommerceMigration">The new commerce migration response.</param>
    /// <param name="newCommerceMigrationError">The new commerce migration error.</param>
    /// <returns>The MigrationResult mapping.</returns>
    private MigrationResult PrepareMigrationResult(MigrationResult migrationResult, string batchId, NewCommerceMigration? newCommerceMigration = null, NewCommerceMigrationError? newCommerceMigrationError = null)
    {
        MigrationResult result = new MigrationResult();

        if (newCommerceMigrationError != null)
        {
            result = new MigrationResult
            {
                PartnerTenantId = migrationResult.PartnerTenantId,
                IndirectResellerMpnId = migrationResult.IndirectResellerMpnId,
                CustomerName = migrationResult.CustomerName,
                CustomerTenantId = migrationResult.CustomerTenantId,
                LegacySubscriptionId = migrationResult.LegacySubscriptionId,
                LegacySubscriptionName = migrationResult.LegacySubscriptionName,
                LegacyProductName = migrationResult.LegacyProductName,
                ExpirationDate = migrationResult.ExpirationDate,
                StartedNewTermInNce = migrationResult.StartedNewTermInNce,
                NCETermDuration = migrationResult.NCETermDuration,
                NCEBillingPlan = migrationResult.NCEBillingPlan,
                NCESeatCount = migrationResult.NCESeatCount,
                ErrorCode = newCommerceMigrationError.Code,
                ErrorReason = newCommerceMigrationError.Description,
            };
        }

        if (newCommerceMigration != null)
        {
            result = new MigrationResult
            {
                PartnerTenantId = migrationResult.PartnerTenantId,
                IndirectResellerMpnId = migrationResult.IndirectResellerMpnId,
                CustomerName = migrationResult.CustomerName,
                CustomerTenantId = migrationResult.CustomerTenantId,
                LegacySubscriptionId = migrationResult.LegacySubscriptionId,
                LegacySubscriptionName = migrationResult.LegacySubscriptionName,
                LegacyProductName = migrationResult.LegacyProductName,
                ExpirationDate = migrationResult.ExpirationDate,
                MigrationStatus = newCommerceMigration.Status,
                StartedNewTermInNce = migrationResult.StartedNewTermInNce,
                NCETermDuration = newCommerceMigration.TermDuration,
                NCEBillingPlan = newCommerceMigration.BillingCycle,
                NCESeatCount = newCommerceMigration.Quantity,
                NCESubscriptionId = newCommerceMigration.NewCommerceSubscriptionId,
                BatchId = batchId,
                MigrationId = newCommerceMigration.Id,
            };
        }

        return result;
    }
}