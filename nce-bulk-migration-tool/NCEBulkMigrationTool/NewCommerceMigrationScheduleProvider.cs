// -----------------------------------------------------------------------
// <copyright file="NewCommerceMigrationScheduleProvider.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCEBulkMigrationTool
{
    internal class NewCommerceMigrationScheduleProvider : INewCommerceMigrationScheduleProvider
    {
        private static string partnerTenantId = string.Empty;
        private readonly ITokenProvider tokenProvider;
        private long subscriptionsCntr = 0;

        /// <summary>
        /// The NewCommerceMigrationScheduleProvider constructor.
        /// </summary>
        /// <param name="tokenProvider">The token provider.</param>
        public NewCommerceMigrationScheduleProvider(ITokenProvider tokenProvider)
        {
            this.tokenProvider = tokenProvider;
        }

        /// <inheritdoc/>
        public async Task<bool> ValidateAndGetSubscriptionsToScheduleMigrationAsync()
        {
            subscriptionsCntr = 0;
            var csvProvider = new CsvProvider();

            using TextReader fileReader = File.OpenText($"{Constants.InputFolderPath}/customers.csv");
            using var csvReader = new CsvReader(fileReader, CultureInfo.InvariantCulture, leaveOpen: true);
            var inputCustomers = csvReader.GetRecordsAsync<CompanyProfile>();

            ConcurrentBag<IEnumerable<ScheduleMigrationRequest>> allMigrationRequests = new ConcurrentBag<IEnumerable<ScheduleMigrationRequest>>();
            var failedCustomersBag = new ConcurrentBag<CompanyProfile>();

            var authenticationResult = await this.tokenProvider.GetTokenAsync();
            partnerTenantId = authenticationResult.TenantId;

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(Routes.BaseUrl)
            };
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticationResult.AccessToken);
            httpClient.DefaultRequestHeaders.Add(Constants.PartnerCenterClientHeader, Constants.ClientName);

            var options = new ParallelOptions()
            {
                MaxDegreeOfParallelism = 5
            };

            await Parallel.ForEachAsync(inputCustomers, options, async (customer, cancellationToken) =>
            {
                try
                {
                    var subscriptions = await GetLegacySubscriptionsAsync(httpClient, customer, cancellationToken);

                    Console.WriteLine($"Validating subscriptions eligibility for customer {customer.CompanyName}");
                    var migrationEligibilities = await ValidateMigrationEligibility(customer, httpClient, options, subscriptions);
                    var migrationScheduleDetails = await GetMigrationScheduleAsync(customer, httpClient, options, migrationEligibilities);
                    allMigrationRequests.Add(migrationScheduleDetails.Select(a => a));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to verify migration eligibility for customer {customer.CompanyName} {ex}");
                    failedCustomersBag.Add(customer);
                }
            });

            csvReader.Dispose();
            fileReader.Close();

            Console.WriteLine("Exporting subscriptions");
            await csvProvider.ExportCsv(allMigrationRequests.SelectMany(m => m), $"{Constants.OutputFolderPath}/subscriptionsforschedule.csv");
            Console.WriteLine($"Exported subscriptions at {Environment.CurrentDirectory}/{Constants.OutputFolderPath}/subscriptionsforschedule.csv");

            if (failedCustomersBag.Count > 0)
            {
                Console.WriteLine("Exporting failed customers");
                await csvProvider.ExportCsv(failedCustomersBag, $"{Constants.OutputFolderPath}/failedCustomers_schedulemigration.csv");
                Console.WriteLine($"Exported failed customers at {Environment.CurrentDirectory}/{Constants.OutputFolderPath}/failedCustomers_schedulemigration.csv");
            }

            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> UploadNewCommerceMigrationSchedulesAsync()
        {
            var csvProvider = new CsvProvider();

            var inputFileNames = Directory.EnumerateFiles($"{Constants.InputFolderPath}/subscriptionsforschedule");
            var authenticationResult = await this.tokenProvider.GetTokenAsync();

            foreach (var fileName in inputFileNames)
            {
                Console.WriteLine($"Processing file {fileName}");

                using TextReader fileReader = File.OpenText(fileName);
                using var csvReader = new CsvReader(fileReader, CultureInfo.InvariantCulture, leaveOpen: true);
                var inputMigrationRequests = csvReader.GetRecords<ScheduleMigrationRequest>().ToList();

                if (inputMigrationRequests.Count > 200)
                {
                    Console.WriteLine($"There are too many migration requests in the file: {fileName}. The maximum limit for migration uploads per file is 200. Please fix the input file to continue...");
                    continue;
                }

                var migrations = new ConcurrentBag<IEnumerable<ScheduleMigrationResult>>();

                var httpClient = new HttpClient
                {
                    BaseAddress = new Uri(Routes.BaseUrl)
                };
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
                        List<ScheduleMigrationResult> migrationResult;
                        migrationResult = await this.PostNewCommerceMigrationScheduleAsync(httpClient, migrationRequest, inputAddOnMigrationRequests, batchId, cancellationToken);
                        migrations.Add(migrationResult);
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
                await csvProvider.ExportCsv(migrations.SelectMany(m => m), $"{Constants.OutputFolderPath}/schedulemigrations/{processedFileName}_{batchId}.csv");

                File.Move(fileName, $"{Constants.InputFolderPath}/subscriptionsforschedule/processed/{processedFileName}", true);

                await Task.Delay(1000);

                Console.WriteLine($"Exported migrations at {Environment.CurrentDirectory}/{Constants.OutputFolderPath}/schedulemigrations/{processedFileName}_{batchId}.csv");
            }

            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> ExportNewCommerceMigrationSchedulesAsync()
        {
            var csvProvider = new CsvProvider();

            using TextReader fileReader = File.OpenText($"{Constants.InputFolderPath}/customers.csv");
            using var csvReader = new CsvReader(fileReader, CultureInfo.InvariantCulture, leaveOpen: true);
            var inputCustomers = csvReader.GetRecordsAsync<CompanyProfile>();

            ConcurrentBag<IEnumerable<NewCommerceMigrationSchedule>> allMigrationSchedules = new();
            var failedCustomersBag = new ConcurrentBag<CompanyProfile>();

            var authenticationResult = await this.tokenProvider.GetTokenAsync();
            partnerTenantId = authenticationResult.TenantId;

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(Routes.BaseUrl)
            };
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticationResult.AccessToken);
            httpClient.DefaultRequestHeaders.Add(Constants.PartnerCenterClientHeader, Constants.ClientName);

            var options = new ParallelOptions()
            {
                MaxDegreeOfParallelism = 5
            };

            await Parallel.ForEachAsync(inputCustomers, options, async (customer, cancellationToken) =>
            {
                try
                {
                    var getScheduleMigrationsRequest = new HttpRequestMessage(HttpMethod.Get, $"{Routes.GetNewCommerceMigrationSchedules}/?CustomerTenantId={customer.TenantId}");

                    getScheduleMigrationsRequest.Headers.Add("MS-CorrelationId", Guid.NewGuid().ToString());

                    var scheduleMigrationsResponse = await httpClient.SendAsync(getScheduleMigrationsRequest, cancellationToken).ConfigureAwait(false);
                    if (scheduleMigrationsResponse.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var authenticationResult = await this.tokenProvider.GetTokenAsync();
                        httpClient.DefaultRequestHeaders.Clear();
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticationResult.AccessToken);
                        httpClient.DefaultRequestHeaders.Add(Constants.PartnerCenterClientHeader, Constants.ClientName);
                        getScheduleMigrationsRequest = new HttpRequestMessage(HttpMethod.Get, $"{Routes.GetNewCommerceMigrationSchedules}/?CustomerTenantId={customer.TenantId}");
                        getScheduleMigrationsRequest.Headers.Add("MS-CorrelationId", Guid.NewGuid().ToString());

                        scheduleMigrationsResponse = await httpClient.SendAsync(getScheduleMigrationsRequest).ConfigureAwait(false);
                    }

                    scheduleMigrationsResponse.EnsureSuccessStatusCode();
                    var newCommerceMigrationSchedules = await scheduleMigrationsResponse.Content.ReadFromJsonAsync<IEnumerable<NewCommerceMigrationSchedule>>().ConfigureAwait(false);
                    allMigrationSchedules.Add(newCommerceMigrationSchedules);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to fetch migration schedules for customer {customer.CompanyName} {ex}");
                    failedCustomersBag.Add(customer);
                }
            });

            csvReader.Dispose();
            fileReader.Close();

            Console.WriteLine("Exporting migration schedules");
            await csvProvider.ExportCsv(allMigrationSchedules.SelectMany(m => m), $"{Constants.OutputFolderPath}/schedulemigrations.csv");
            Console.WriteLine($"Exported schedule migrations at {Environment.CurrentDirectory}/{Constants.OutputFolderPath}/schedulemigrations.csv");

            if (failedCustomersBag.Count > 0)
            {
                Console.WriteLine("Exporting failed customers");
                await csvProvider.ExportCsv(failedCustomersBag, "failedCustomers.csv");
                Console.WriteLine($"Exported failed customers at {Environment.CurrentDirectory}/failedCustomers.csv");
            }

            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> CancelNewCommerceMigrationSchedulesAsync()
        {
            var csvProvider = new CsvProvider();

            var inputFileNames = Directory.EnumerateFiles($"{Constants.InputFolderPath}/cancelschedulemigrations");
            var authenticationResult = await this.tokenProvider.GetTokenAsync();

            foreach (var fileName in inputFileNames)
            {
                Console.WriteLine($"Processing file {fileName}");

                using TextReader fileReader = File.OpenText(fileName);
                using var csvReader = new CsvReader(fileReader, CultureInfo.InvariantCulture, leaveOpen: true);
                var inputMigrationSchedules = csvReader.GetRecords<NewCommerceMigrationSchedule>().ToList();

                if (inputMigrationSchedules.Count > 200)
                {
                    Console.WriteLine($"There are too many migration schedule requests in the file: {fileName}. The maximum limit for migration uploads per file is 200. Please fix the input file to continue...");
                    continue;
                }

                var migrationSchedules = new ConcurrentBag<NewCommerceMigrationSchedule>();

                var httpClient = new HttpClient
                {
                    BaseAddress = new Uri(Routes.BaseUrl)
                };
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticationResult.AccessToken);
                httpClient.DefaultRequestHeaders.Add(Constants.PartnerCenterClientHeader, Constants.ClientName);

                var options = new ParallelOptions()
                {
                    MaxDegreeOfParallelism = 5
                };

                long scheduleMigrationsCounter = 0;
                var batchId = Guid.NewGuid().ToString();

                var scheduledMigrationsToCancel = inputMigrationSchedules.Where(s => string.Equals(s.Status, "Cancel", StringComparison.OrdinalIgnoreCase));

                await Parallel.ForEachAsync(scheduledMigrationsToCancel, options, async (scheduleMigration, cancellationToken) =>
                {
                    try
                    {
                        NewCommerceMigrationSchedule migrationScheduleResult;
                        migrationScheduleResult = await this.CancelNewCommerceMigrationScheduleAsync(httpClient, scheduleMigration, cancellationToken);
                        migrationSchedules.Add(migrationScheduleResult);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine($"Cancel Migration Schedule for subscription: {scheduleMigration.CurrentSubscriptionId} failed.");
                    }
                    finally
                    {
                        Interlocked.Increment(ref scheduleMigrationsCounter);
                        Console.WriteLine($"Processed {scheduleMigrationsCounter} cancel schedule migration requests.");
                    }
                });

                csvReader.Dispose();
                fileReader.Close();

                var index = fileName.LastIndexOf('\\');
                var processedFileName = fileName[++index..];

                Console.WriteLine("Exporting cancelled scheduled migrations");
                await csvProvider.ExportCsv(migrationSchedules.Select(s => s), $"{Constants.OutputFolderPath}/cancelschedulemigrations/{processedFileName}_{batchId}.csv");

                File.Move(fileName, $"{Constants.InputFolderPath}/cancelschedulemigrations/processed/{processedFileName}", true);

                await Task.Delay(1000);

                Console.WriteLine($"Exported cancelled schedule migrations at {Environment.CurrentDirectory}/{Constants.OutputFolderPath}/cancelschedulemigrations/{processedFileName}_{batchId}.csv");
            }

            return true;
        }

        private async Task<IEnumerable<Subscription>> GetLegacySubscriptionsAsync(HttpClient httpClient, CompanyProfile customer, CancellationToken cancellationToken)
        {
            var allSubscriptions = await this.GetSubscriptionsAsync(httpClient, customer, cancellationToken).ConfigureAwait(false);
            var subscriptions = allSubscriptions.Where(s => Guid.TryParse(s.OfferId, out _));

            return subscriptions;
        }

        private async Task<IEnumerable<Subscription>> GetSubscriptionsAsync(HttpClient httpClient, CompanyProfile customer, CancellationToken cancellationToken)
        {
            var subscriptionRequest = new HttpRequestMessage(HttpMethod.Get, string.Format(Routes.GetSubscriptions, customer.TenantId));

            subscriptionRequest.Headers.Add("MS-CorrelationId", Guid.NewGuid().ToString());

            var subscriptionResponse = await httpClient.SendAsync(subscriptionRequest, cancellationToken).ConfigureAwait(false);
            if (subscriptionResponse.StatusCode == HttpStatusCode.Unauthorized)
            {
                var authenticationResult = await this.tokenProvider.GetTokenAsync();
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticationResult.AccessToken);
                httpClient.DefaultRequestHeaders.Add(Constants.PartnerCenterClientHeader, Constants.ClientName);
                subscriptionRequest = new HttpRequestMessage(HttpMethod.Get, string.Format(Routes.GetSubscriptions, customer.TenantId));
                subscriptionRequest.Headers.Add("MS-CorrelationId", Guid.NewGuid().ToString());

                subscriptionResponse = await httpClient.SendAsync(subscriptionRequest).ConfigureAwait(false);
            }

            subscriptionResponse.EnsureSuccessStatusCode();
            var subscriptionCollection = await subscriptionResponse.Content.ReadFromJsonAsync<ResourceCollection<Subscription>>().ConfigureAwait(false);

            return subscriptionCollection!.Items;
        }

        private async Task<ConcurrentBag<ScheduleMigrationRequest>> ValidateMigrationEligibility(CompanyProfile customer, HttpClient httpClient, ParallelOptions options, IEnumerable<Subscription> subscriptions)
        {
            var baseSubscriptions = subscriptions.Where(s => string.IsNullOrWhiteSpace(s.ParentSubscriptionId));
            var addOns = subscriptions.Where(s => !string.IsNullOrWhiteSpace(s.ParentSubscriptionId));
            var migrationRequests = new ConcurrentBag<ScheduleMigrationRequest>();
            var addOnEligibilityList = new ConcurrentBag<IEnumerable<NewCommerceEligibility>>();
            await Parallel.ForEachAsync(baseSubscriptions, options, async (subscription, cancellationToken) =>
            {
                var payload = new NewCommerceMigration
                {
                    CurrentSubscriptionId = subscription.Id,
                };

                var migrationRequest = new HttpRequestMessage(HttpMethod.Post, string.Format(Routes.ValidateMigrationEligibility, customer.TenantId))
                {
                    Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json")
                };

                migrationRequest.Headers.Add("MS-CorrelationId", Guid.NewGuid().ToString());

                var migrationResponse = await httpClient.SendAsync(migrationRequest, cancellationToken).ConfigureAwait(false);
                if (migrationResponse.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var authenticationResult = await this.tokenProvider.GetTokenAsync();
                    httpClient.DefaultRequestHeaders.Clear();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticationResult.AccessToken);
                    httpClient.DefaultRequestHeaders.Add(Constants.PartnerCenterClientHeader, Constants.ClientName);
                    migrationRequest = new HttpRequestMessage(HttpMethod.Post, string.Format(Routes.ValidateMigrationEligibility, customer.TenantId))
                    {
                        Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json")
                    };
                    migrationRequest.Headers.Add("MS-CorrelationId", Guid.NewGuid().ToString());

                    migrationResponse = await httpClient.SendAsync(migrationRequest).ConfigureAwait(false);
                }

                migrationResponse.EnsureSuccessStatusCode();
                var newCommerceEligibility = await migrationResponse.Content.ReadFromJsonAsync<NewCommerceEligibility>().ConfigureAwait(false);
                if (newCommerceEligibility.AddOnMigrations.Any())
                {
                    addOnEligibilityList.Add(newCommerceEligibility.AddOnMigrations);
                }

                migrationRequests.Add(PrepareMigrationRequest(customer, subscription, newCommerceEligibility!));

                Interlocked.Increment(ref subscriptionsCntr);
                Console.WriteLine($"Validated migration eligibility for {subscriptionsCntr} subscriptions.");
            });

            foreach (var addOn in addOns)
            {
                var addOnEligibility = addOnEligibilityList.SelectMany(a => a).SingleOrDefault(m => m.CurrentSubscriptionId.Equals(addOn.Id, StringComparison.OrdinalIgnoreCase));
                if (addOnEligibility != null)
                {
                    migrationRequests.Add(PrepareMigrationRequest(customer, addOn, addOnEligibility));
                }
            }

            return migrationRequests;
        }

        private async Task<ConcurrentBag<ScheduleMigrationRequest>> GetMigrationScheduleAsync(CompanyProfile customer, HttpClient httpClient, ParallelOptions options, IEnumerable<ScheduleMigrationRequest> scheduleMigrationRequests)
        {
            var baseSubscriptions = scheduleMigrationRequests.Where(s => string.IsNullOrWhiteSpace(s.BaseSubscriptionId));
            var addOns = scheduleMigrationRequests.Where(s => !string.IsNullOrWhiteSpace(s.BaseSubscriptionId));
            var migrationRequests = new ConcurrentBag<ScheduleMigrationRequest>();
            var addOnEligibilityList = new ConcurrentBag<IEnumerable<NewCommerceMigrationSchedule>>();
            await Parallel.ForEachAsync(baseSubscriptions, options, async (subscription, cancellationToken) =>
            {
                var getScheduleMigrationRequest = new HttpRequestMessage(HttpMethod.Get, $"{Routes.GetNewCommerceMigrationSchedules}?CustomerTenantId={customer.TenantId}&CurrentSubscriptionId={subscription.LegacySubscriptionId}");

                getScheduleMigrationRequest.Headers.Add("MS-CorrelationId", Guid.NewGuid().ToString());

                var migrationResponse = await httpClient.SendAsync(getScheduleMigrationRequest, cancellationToken).ConfigureAwait(false);
                if (migrationResponse.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var authenticationResult = await this.tokenProvider.GetTokenAsync();
                    httpClient.DefaultRequestHeaders.Clear();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticationResult.AccessToken);
                    httpClient.DefaultRequestHeaders.Add(Constants.PartnerCenterClientHeader, Constants.ClientName);
                    getScheduleMigrationRequest = new HttpRequestMessage(HttpMethod.Get, $"{Routes.GetNewCommerceMigrationSchedules}?CustomerTenantId={customer.TenantId}&CurrentSubscriptionId={subscription.LegacySubscriptionId}");
                    getScheduleMigrationRequest.Headers.Add("MS-CorrelationId", Guid.NewGuid().ToString());

                    migrationResponse = await httpClient.SendAsync(getScheduleMigrationRequest).ConfigureAwait(false);
                }

                migrationResponse.EnsureSuccessStatusCode();
                var newCommerceMigrationSchedule = (await migrationResponse.Content.ReadFromJsonAsync<IEnumerable<NewCommerceMigrationSchedule>>().ConfigureAwait(false)).SingleOrDefault();

                migrationRequests.Add(PrepareMigrationRequest(subscription, newCommerceMigrationSchedule));

                if (newCommerceMigrationSchedule?.AddOnMigrationSchedules.Any() ?? false)
                {
                    addOnEligibilityList.Add(newCommerceMigrationSchedule.AddOnMigrationSchedules);
                }
            });

            foreach (var addOn in addOns)
            {
                var addOnEligibility = addOnEligibilityList.SelectMany(a => a).SingleOrDefault(m => m.CurrentSubscriptionId.Equals(addOn.LegacySubscriptionId, StringComparison.OrdinalIgnoreCase));
                migrationRequests.Add(PrepareMigrationRequest(addOn, addOnEligibility));
            }

            return migrationRequests;
        }

        private static ScheduleMigrationRequest PrepareMigrationRequest(CompanyProfile companyProfile, Subscription subscription, NewCommerceEligibility newCommerceEligibility)
        {
            return new ScheduleMigrationRequest
            {
                PartnerTenantId = partnerTenantId,
                IndirectResellerMpnId = subscription.PartnerId,
                CustomerName = companyProfile.CompanyName,
                CustomerTenantId = companyProfile.TenantId,
                LegacySubscriptionId = subscription.Id,
                LegacySubscriptionName = subscription.FriendlyName,
                LegacyProductName = subscription.OfferName,
                ExpirationDate = subscription.CommitmentEndDate,
                AutoRenewEnabled = subscription.AutoRenewEnabled,
                MigrationEligible = newCommerceEligibility.IsEligible,
                NcePsa = newCommerceEligibility.CatalogItemId,
                CurrentTerm = subscription.TermDuration,
                CurrentBillingPlan = subscription.BillingCycle.ToString(),
                CurrentSeatCount = subscription.Quantity,
                StartNewTermInNce = false,
                Term = subscription.TermDuration,
                BillingPlan = subscription.BillingCycle.ToString(),
                SeatCount = subscription.Quantity,
                CustomTermEndDate = newCommerceEligibility.CustomTermEndDate,
                AddOn = !string.IsNullOrWhiteSpace(subscription.ParentSubscriptionId),
                BaseSubscriptionId = subscription.ParentSubscriptionId,
                MigrationIneligibilityReason = newCommerceEligibility.Errors.Any() ?
                                string.Join(";", newCommerceEligibility.Errors.Select(e => e.Description)) :
                                string.Empty
            };
        }

        private static ScheduleMigrationRequest PrepareMigrationRequest(ScheduleMigrationRequest scheduleMigrationRequest, NewCommerceMigrationSchedule? newCommerceMigrationSchedule)
        {
            if (newCommerceMigrationSchedule is not null)
            {
                return scheduleMigrationRequest with
                {
                    StartNewTermInNce = newCommerceMigrationSchedule.PurchaseFullTerm,
                    Term = newCommerceMigrationSchedule.TermDuration,
                    BillingPlan = newCommerceMigrationSchedule.BillingCycle,
                    SeatCount = newCommerceMigrationSchedule.Quantity,
                    CustomTermEndDate = newCommerceMigrationSchedule.CustomTermEndDate,
                    TargetDate = newCommerceMigrationSchedule.TargetDate,
                    MigrateOnRenewal = newCommerceMigrationSchedule.MigrateOnRenewal,
                };
            }

            return scheduleMigrationRequest;
        }

        private async Task<List<ScheduleMigrationResult>> PostNewCommerceMigrationScheduleAsync(HttpClient httpClient, ScheduleMigrationRequest migrationRequest, IEnumerable<ScheduleMigrationRequest> addOnMigrationRequests, string batchId, CancellationToken cancellationToken)
        {
            var newCommerceMigrationRequest = new HttpRequestMessage(HttpMethod.Post, string.Format(Routes.PostNewCommerceMigrationSchedule, migrationRequest.CustomerTenantId));

            var getSchedulesRoute = $"{Routes.GetNewCommerceMigrationSchedules}?CurrentSubscriptionId={migrationRequest.LegacySubscriptionId}";
            var getSchedulesRequest = new HttpRequestMessage(HttpMethod.Get, getSchedulesRoute);
            getSchedulesRequest.Headers.Add("MS-CorrelationId", Guid.NewGuid().ToString());
            var existingNewCommerceMigrationScheduleResponse = await httpClient.SendAsync(getSchedulesRequest).ConfigureAwait(false);
            NewCommerceMigrationSchedule? existingSchedule = null;

            if (existingNewCommerceMigrationScheduleResponse.IsSuccessStatusCode)
            {
                existingSchedule = (await existingNewCommerceMigrationScheduleResponse.Content.ReadFromJsonAsync<IEnumerable<NewCommerceMigrationSchedule>>().ConfigureAwait(false))?.FirstOrDefault();
            }

            newCommerceMigrationRequest.Headers.Add("MS-CorrelationId", Guid.NewGuid().ToString());

            var newCommerceMigrationSchedule = new NewCommerceMigrationSchedule
            {
                CurrentSubscriptionId = migrationRequest.LegacySubscriptionId,
                Quantity = migrationRequest.SeatCount,
                BillingCycle = migrationRequest.BillingPlan,
                TermDuration = migrationRequest.Term,
                CustomTermEndDate = migrationRequest.CustomTermEndDate,
                TargetDate = migrationRequest.TargetDate,
                MigrateOnRenewal = migrationRequest.MigrateOnRenewal,
                ExternalReferenceId = batchId,
            };

            // If they want to start a new term, then we should take the input from the file.
            if (migrationRequest.StartNewTermInNce)
            {
                newCommerceMigrationSchedule.PurchaseFullTerm = true;
            }

            if (existingSchedule is not null)
            {
                newCommerceMigrationRequest = new HttpRequestMessage(HttpMethod.Put, string.Format(Routes.UpdateNewCommerceMigrationSchedule, migrationRequest.CustomerTenantId, existingSchedule.Id));
                newCommerceMigrationSchedule = newCommerceMigrationSchedule with
                {
                    Id = existingSchedule.Id,
                };
            }

            newCommerceMigrationSchedule.AddOnMigrationSchedules = GetAddOnMigrationSchedules(migrationRequest.LegacySubscriptionId, addOnMigrationRequests);

            newCommerceMigrationRequest.Content = new StringContent(JsonSerializer.Serialize(newCommerceMigrationSchedule), Encoding.UTF8, "application/json");

            var migrationResponse = await httpClient.SendAsync(newCommerceMigrationRequest, cancellationToken).ConfigureAwait(false);
            if (migrationResponse.StatusCode == HttpStatusCode.Unauthorized)
            {
                var authenticationResult = await this.tokenProvider.GetTokenAsync();
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticationResult.AccessToken);
                httpClient.DefaultRequestHeaders.Add(Constants.PartnerCenterClientHeader, Constants.ClientName);
                if (existingSchedule is not null)
                {
                    newCommerceMigrationRequest = new HttpRequestMessage(HttpMethod.Put, string.Format(Routes.UpdateNewCommerceMigrationSchedule, migrationRequest.CustomerTenantId, existingSchedule.Id));
                    newCommerceMigrationSchedule = newCommerceMigrationSchedule with
                    {
                        Id = existingSchedule.Id,
                    };
                }
                else
                {
                    newCommerceMigrationRequest = new HttpRequestMessage(HttpMethod.Post, string.Format(Routes.PostNewCommerceMigrationSchedule, migrationRequest.CustomerTenantId));
                }

                newCommerceMigrationRequest.Content = new StringContent(JsonSerializer.Serialize(newCommerceMigrationSchedule), Encoding.UTF8, "application/json");

                newCommerceMigrationRequest.Headers.Add("MS-CorrelationId", Guid.NewGuid().ToString());
                migrationResponse = await httpClient.SendAsync(newCommerceMigrationRequest).ConfigureAwait(false);
            }

            NewCommerceMigrationError? migrationError = null;
            NewCommerceMigrationSchedule? migration = null;

            if (migrationResponse.IsSuccessStatusCode)
            {
                migration = await migrationResponse.Content.ReadFromJsonAsync<NewCommerceMigrationSchedule>().ConfigureAwait(false);
            }
            else
            {
                migrationError = await migrationResponse.Content.ReadFromJsonAsync<NewCommerceMigrationError>().ConfigureAwait(false);
            }

            return this.PrepareMigrationResult(migrationRequest, addOnMigrationRequests, batchId, migration, migrationError);
        }

        private async Task<NewCommerceMigrationSchedule> CancelNewCommerceMigrationScheduleAsync(HttpClient httpClient, NewCommerceMigrationSchedule newCommerceMigrationSchedule, CancellationToken cancellationToken)
        {
            var cancelNewCommerceMigrationRequest = new HttpRequestMessage(HttpMethod.Post, string.Format(Routes.CancelNewCommerceMigrationSchedule, newCommerceMigrationSchedule.CustomerTenantId, newCommerceMigrationSchedule.Id));
            cancelNewCommerceMigrationRequest.Headers.Add("MS-CorrelationId", Guid.NewGuid().ToString());

            var migrationScheduleResponse = await httpClient.SendAsync(cancelNewCommerceMigrationRequest, cancellationToken).ConfigureAwait(false);
            if (migrationScheduleResponse.StatusCode == HttpStatusCode.Unauthorized)
            {
                var authenticationResult = await this.tokenProvider.GetTokenAsync();
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticationResult.AccessToken);
                httpClient.DefaultRequestHeaders.Add(Constants.PartnerCenterClientHeader, Constants.ClientName);
                cancelNewCommerceMigrationRequest = new HttpRequestMessage(HttpMethod.Post, string.Format(Routes.CancelNewCommerceMigrationSchedule, newCommerceMigrationSchedule.CustomerTenantId, newCommerceMigrationSchedule.Id));
                cancelNewCommerceMigrationRequest.Headers.Add("MS-CorrelationId", Guid.NewGuid().ToString());
                cancelNewCommerceMigrationRequest.Content = new StringContent(JsonSerializer.Serialize(newCommerceMigrationSchedule), Encoding.UTF8, "application/json");
                migrationScheduleResponse = await httpClient.SendAsync(cancelNewCommerceMigrationRequest).ConfigureAwait(false);
            }

            if (migrationScheduleResponse.IsSuccessStatusCode)
            {
                newCommerceMigrationSchedule = await migrationScheduleResponse.Content.ReadFromJsonAsync<NewCommerceMigrationSchedule>().ConfigureAwait(false);
            }
            else
            {
                var migrationScheduleError = await migrationScheduleResponse.Content.ReadFromJsonAsync<NewCommerceMigrationError>().ConfigureAwait(false);
                newCommerceMigrationSchedule.ErrorCode = migrationScheduleError.Code;
                newCommerceMigrationSchedule.ErrorDescription = migrationScheduleError.Description;
            }

            return newCommerceMigrationSchedule;
        }

        private static IEnumerable<NewCommerceMigrationSchedule> GetAddOnMigrationSchedules(string currentSubscriptionId, IEnumerable<ScheduleMigrationRequest> addOnMigrationRequests)
        {
            if (!addOnMigrationRequests.Any())
            {
                return Enumerable.Empty<NewCommerceMigrationSchedule>();
            }

            var allAddOnMigrations = new List<NewCommerceMigrationSchedule>();

            var childAddOns = addOnMigrationRequests.Where(a => a.BaseSubscriptionId.Equals(currentSubscriptionId, StringComparison.OrdinalIgnoreCase));

            if (childAddOns.Any())
            {
                var addOnNewCommerceMigrations = childAddOns.Select(request => new NewCommerceMigrationSchedule
                {
                    CurrentSubscriptionId = request.LegacySubscriptionId,
                    Quantity = request.SeatCount,
                    BillingCycle = request.BillingPlan,
                    TermDuration = request.Term,
                    PurchaseFullTerm = request.StartNewTermInNce,
                    CustomTermEndDate = request.CustomTermEndDate,
                    TargetDate = request.TargetDate,
                    MigrateOnRenewal = request.MigrateOnRenewal,
                });

                allAddOnMigrations.AddRange(addOnNewCommerceMigrations);

                foreach (var item in childAddOns)
                {
                    var multiLevelAddons = GetAddOnMigrationSchedules(item.LegacySubscriptionId, addOnMigrationRequests);
                    allAddOnMigrations.AddRange(multiLevelAddons);
                }
            }

            return allAddOnMigrations;
        }

        private List<ScheduleMigrationResult> PrepareMigrationResult(ScheduleMigrationRequest migrationRequest, IEnumerable<ScheduleMigrationRequest> addOnMigrationRequests, string batchId, NewCommerceMigrationSchedule? newCommerceMigrationSchedule = null, NewCommerceMigrationError? newCommerceMigrationError = null)
        {
            var migrationResults = new List<ScheduleMigrationResult>();
            PrepareMigrationResult(migrationRequest, batchId, newCommerceMigrationSchedule, newCommerceMigrationError, migrationResults);

            if (newCommerceMigrationSchedule?.AddOnMigrationSchedules.Any() == true)
            {
                PrepareAddOnMigrationResult(addOnMigrationRequests, batchId, newCommerceMigrationSchedule, newCommerceMigrationError, migrationResults);
            }

            return migrationResults;
        }

        private static void PrepareMigrationResult(ScheduleMigrationRequest migrationRequest, string batchId, NewCommerceMigrationSchedule? newCommerceMigrationSchedule, NewCommerceMigrationError? newCommerceMigrationError, List<ScheduleMigrationResult> migrationResults)
        {
            if (newCommerceMigrationError != null)
            {
                var migrationResult = new ScheduleMigrationResult
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
                    CustomTermEndDate = migrationRequest.CustomTermEndDate,
                    TargetDate = migrationRequest.TargetDate,
                    MigrateOnRenewal = migrationRequest.MigrateOnRenewal,
                    ErrorCode = newCommerceMigrationError.Code,
                    ErrorReason = newCommerceMigrationError.Description,
                };

                migrationResults.Add(migrationResult);
            }

            if (newCommerceMigrationSchedule != null)
            {
                var migrationResult = new ScheduleMigrationResult
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
                    NCETermDuration = newCommerceMigrationSchedule.TermDuration,
                    NCEBillingPlan = newCommerceMigrationSchedule.BillingCycle,
                    NCESeatCount = newCommerceMigrationSchedule.Quantity,
                    CustomTermEndDate = migrationRequest.CustomTermEndDate,
                    TargetDate = migrationRequest.TargetDate,
                    MigrateOnRenewal = migrationRequest.MigrateOnRenewal,
                    BatchId = batchId,
                    MigrationScheduleId = newCommerceMigrationSchedule.Id,
                    MigrationScheduleStatus = newCommerceMigrationSchedule.Status,
                };

                migrationResults.Add(migrationResult);
            }
        }

        private List<ScheduleMigrationResult> PrepareAddOnMigrationResult(IEnumerable<ScheduleMigrationRequest> addOnMigrationRequests, string batchId, NewCommerceMigrationSchedule? newCommerceMigrationSchedule, NewCommerceMigrationError? newCommerceMigrationError, List<ScheduleMigrationResult> migrationResults)
        {
            if (newCommerceMigrationSchedule != null && addOnMigrationRequests?.Any() == true)
            {
                foreach (var addOnMigrationResponse in newCommerceMigrationSchedule.AddOnMigrationSchedules)
                {
                    var addOnMigrationRequest = addOnMigrationRequests.SingleOrDefault(n => n.LegacySubscriptionId.Equals(addOnMigrationResponse.CurrentSubscriptionId, StringComparison.OrdinalIgnoreCase));
                    addOnMigrationResponse.Status = newCommerceMigrationSchedule.Status;
                    addOnMigrationResponse.Id = newCommerceMigrationSchedule.Id;
                    PrepareMigrationResult(addOnMigrationRequest, batchId, addOnMigrationResponse, newCommerceMigrationError, migrationResults);
                }
            }

            return migrationResults;
        }

        private async Task<(ScheduleMigrationResult BaseMigrationResult, IEnumerable<NewCommerceMigrationSchedule> AddOnMigrationsResult)> GetNewCommerceMigrationScheduleByScheduleIdAsync(HttpClient httpClient, ScheduleMigrationResult migrationResult, CancellationToken cancellationToken)
        {
            // Validate that the migration result has a migrationId, if a migration didn't initiate the migrationId will be empty.
            if (string.IsNullOrWhiteSpace(migrationResult.MigrationScheduleId))
            {
                // We cannot determine the status, we should return this migration result.
                return (migrationResult, Enumerable.Empty<NewCommerceMigrationSchedule>());
            }

            var getNewCommerceMigrationSchedule = new HttpRequestMessage(HttpMethod.Get, string.Format(Routes.GetNewCommerceMigrationSchedule, migrationResult.CustomerTenantId, migrationResult.MigrationScheduleId));

            getNewCommerceMigrationSchedule.Headers.Add("MS-CorrelationId", Guid.NewGuid().ToString());

            var migrationResponse = await httpClient.SendAsync(getNewCommerceMigrationSchedule, cancellationToken).ConfigureAwait(false);
            if (migrationResponse.StatusCode == HttpStatusCode.Unauthorized)
            {
                var authenticationResult = await this.tokenProvider.GetTokenAsync();
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticationResult.AccessToken);
                httpClient.DefaultRequestHeaders.Add(Constants.PartnerCenterClientHeader, Constants.ClientName);
                getNewCommerceMigrationSchedule = new HttpRequestMessage(HttpMethod.Get, string.Format(Routes.GetNewCommerceMigrationSchedule, migrationResult.CustomerTenantId, migrationResult.MigrationScheduleId));
                getNewCommerceMigrationSchedule.Headers.Add("MS-CorrelationId", Guid.NewGuid().ToString());
                migrationResponse = await httpClient.SendAsync(getNewCommerceMigrationSchedule).ConfigureAwait(false);
            }

            NewCommerceMigrationError? migrationError = null;
            NewCommerceMigrationSchedule? migration = null;

            if (migrationResponse.IsSuccessStatusCode)
            {
                migration = await migrationResponse.Content.ReadFromJsonAsync<NewCommerceMigrationSchedule>().ConfigureAwait(false);
            }

            var result = this.PrepareMigrationResult(migrationResult, migrationResult.BatchId, migration, migrationError);
            return (result, migration?.AddOnMigrationSchedules);
        }

        private ScheduleMigrationResult PrepareMigrationResult(ScheduleMigrationResult migrationResult, string batchId, NewCommerceMigrationSchedule? newCommerceMigrationSchedule = null, NewCommerceMigrationError? newCommerceMigrationError = null)
        {
            ScheduleMigrationResult result = new ScheduleMigrationResult();

            if (newCommerceMigrationError != null)
            {
                result = new ScheduleMigrationResult
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
                    CustomTermEndDate = migrationResult.CustomTermEndDate,
                    ErrorCode = newCommerceMigrationError.Code,
                    ErrorReason = newCommerceMigrationError.Description,
                };
            }

            if (newCommerceMigrationSchedule != null)
            {
                result = new ScheduleMigrationResult
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
                    NCETermDuration = newCommerceMigrationSchedule.TermDuration,
                    NCEBillingPlan = newCommerceMigrationSchedule.BillingCycle,
                    NCESeatCount = newCommerceMigrationSchedule.Quantity,
                    CustomTermEndDate = newCommerceMigrationSchedule.CustomTermEndDate,
                    TargetDate = newCommerceMigrationSchedule.TargetDate,
                    MigrateOnRenewal = newCommerceMigrationSchedule.MigrateOnRenewal,
                    BatchId = batchId,
                    MigrationScheduleId = newCommerceMigrationSchedule.Id,
                    MigrationScheduleStatus = newCommerceMigrationSchedule.Status,
                    ErrorCode = newCommerceMigrationSchedule.ErrorCode,
                    ErrorReason = newCommerceMigrationSchedule.ErrorDescription,
                };
            }

            return result;
        }
    }
}