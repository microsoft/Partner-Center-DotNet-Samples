// -----------------------------------------------------------------------
// <copyright file="SubscriptionProvider.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCEBulkMigrationTool;

/// <summary>
/// The SubscriptionProvider class.
/// </summary>
internal class SubscriptionProvider : ISubscriptionProvider
{

    private static string partnerTenantId = string.Empty;
    private readonly ITokenProvider tokenProvider;
    private long subscriptionsCntr = 0;

    /// <summary>
    /// SubscriptionProvider constructor.
    /// </summary>
    /// <param name="tokenProvider"></param>
    public SubscriptionProvider(ITokenProvider tokenProvider)
    {
        this.tokenProvider = tokenProvider;
    }

    /// <inheritdoc/>
    public async Task<bool> ExportLegacySubscriptionsAsync()
    {
        var csvProvider = new CsvProvider();

        using TextReader fileReader = File.OpenText($"{Constants.InputFolderPath}/customers.csv");
        using var csvReader = new CsvReader(fileReader, CultureInfo.InvariantCulture, leaveOpen: true);
        var inputCustomers = csvReader.GetRecordsAsync<CompanyProfile>();

        ConcurrentBag<IEnumerable<MigrationRequest>> allMigrationRequests = new ConcurrentBag<IEnumerable<MigrationRequest>>();
        var failedCustomersBag = new ConcurrentBag<CompanyProfile>();
        
        var authenticationResult = await this.tokenProvider.GetTokenAsync();
        partnerTenantId = authenticationResult.TenantId;

        var httpClient = new HttpClient();
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
                allMigrationRequests.Add(migrationEligibilities);
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
        await csvProvider.ExportCsv(allMigrationRequests.SelectMany(m => m), $"{Constants.OutputFolderPath}/subscriptions.csv");
        Console.WriteLine($"Exported subscriptions at {Environment.CurrentDirectory}/{Constants.OutputFolderPath}/subscriptions.csv");

        if (failedCustomersBag.Count > 0)
        {
            Console.WriteLine("Exporting failed customers");
            await csvProvider.ExportCsv(failedCustomersBag, $"{Constants.OutputFolderPath}/failedCustomers.csv");
            Console.WriteLine($"Exported failed customers at {Environment.CurrentDirectory}/{Constants.OutputFolderPath}/failedCustomers.csv");
        }

        return true;
    }

    /// <inheritdoc/>
    public async Task<bool> ExportModernSubscriptionsAsync()
    {
        var csvProvider = new CsvProvider();

        using TextReader fileReader = File.OpenText($"{Constants.InputFolderPath}/customers.csv");
        using var csvReader = new CsvReader(fileReader, CultureInfo.InvariantCulture, leaveOpen: true);
        var inputCustomers = csvReader.GetRecordsAsync<CompanyProfile>();

        ConcurrentBag<IEnumerable<ModernSubscription>> allSubscriptions = new ConcurrentBag<IEnumerable<ModernSubscription>>();
        var failedCustomersBag = new ConcurrentBag<CompanyProfile>();

        var authenticationResult = await this.tokenProvider.GetTokenAsync();
        partnerTenantId = authenticationResult.TenantId;

        var httpClient = new HttpClient();
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
                var subscriptions = await GetModernSubscriptionsAsync(httpClient, customer, cancellationToken);
                allSubscriptions.Add(subscriptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to fetch subscriptions for customer {customer.CompanyName} {ex}");
                failedCustomersBag.Add(customer);
            }
        });

        csvReader.Dispose();
        fileReader.Close();

        Console.WriteLine("Exporting subscriptions");
        await csvProvider.ExportCsv(allSubscriptions.SelectMany(m => m), $"{Constants.OutputFolderPath}/ncesubscriptions.csv");
        Console.WriteLine($"Exported subscriptions at {Environment.CurrentDirectory}/{Constants.OutputFolderPath}/ncesubscriptions.csv");

        if (failedCustomersBag.Count > 0)
        {
            Console.WriteLine("Exporting failed customers");
            await csvProvider.ExportCsv(failedCustomersBag, "failedCustomers.csv");
            Console.WriteLine($"Exported failed customers at {Environment.CurrentDirectory}/failedCustomers.csv");
        }

        return true;
    }

    /// <summary>
    /// Gets legacy subscriptions for a given customer.
    /// </summary>
    /// <param name="httpClient">The httpClient.</param>
    /// <param name="customer">The customer.</param>
    /// <param name="cancellationToken">The cancellationToken.</param>
    /// <returns>Legacy subscriptions.</returns>
    private async Task<IEnumerable<Subscription>> GetLegacySubscriptionsAsync(HttpClient httpClient, CompanyProfile customer, CancellationToken cancellationToken)
    {
        var allSubscriptions = await this.GetSubscriptionsAsync(httpClient, customer, cancellationToken).ConfigureAwait(false);
        var subscriptions = allSubscriptions.Where(s => Guid.TryParse(s.OfferId, out _));

        return subscriptions;
    }

    private async Task<IEnumerable<ModernSubscription>> GetModernSubscriptionsAsync(HttpClient httpClient, CompanyProfile customer, CancellationToken cancellationToken)
    {
        var allSubscriptions = await this.GetSubscriptionsAsync(httpClient, customer, cancellationToken).ConfigureAwait(false);
        var subscriptions = allSubscriptions!.Where(s => s.ProductType.DisplayName.Equals("OnlineServicesNCE", StringComparison.Ordinal) && s.OfferId.Contains(':'));
        var modernSubscriptions = subscriptions.Select(s => PrepareModernSubscription(customer, s));

        return modernSubscriptions;
    }

    /// <summary>
    /// Gets subscriptions for a given customer.
    /// </summary>
    /// <param name="httpClient">The htppClient.</param>
    /// <param name="customer">The customer.</param>
    /// <param name="cancellationToken">The cancellationToken.</param>
    /// <returns>Subscriptions associated with the given customer.</returns>
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
            subscriptionRequest = new HttpRequestMessage(HttpMethod.Get, Routes.GetCustomers);
            subscriptionRequest.Headers.Add("MS-CorrelationId", Guid.NewGuid().ToString());

            subscriptionResponse = await httpClient.SendAsync(subscriptionRequest).ConfigureAwait(false);
        }

        subscriptionResponse.EnsureSuccessStatusCode();
        var subscriptionCollection = await subscriptionResponse.Content.ReadFromJsonAsync<ResourceCollection<Subscription>>().ConfigureAwait(false);

        return subscriptionCollection!.Items;
    }

    /// <summary>
    /// Validates migration eligibility for a given customer and subscriptions.
    /// </summary>
    /// <param name="customer">The customer.</param>
    /// <param name="httpClient">The httpClient.</param>
    /// <param name="options">The ParallelOptions.</param>
    /// <param name="subscriptions">The subscriptions.</param>
    /// <returns>The migration eligibility.</returns>
    private async Task<ConcurrentBag<MigrationRequest>> ValidateMigrationEligibility(CompanyProfile customer, HttpClient httpClient, ParallelOptions options, IEnumerable<Subscription> subscriptions)
    {
        var baseSubscriptions = subscriptions.Where(s => string.IsNullOrWhiteSpace(s.ParentSubscriptionId));
        var addOns = subscriptions.Where(s => !string.IsNullOrWhiteSpace(s.ParentSubscriptionId));
        var migrationRequests = new ConcurrentBag<MigrationRequest>();
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
                migrationRequest = new HttpRequestMessage(HttpMethod.Get, Routes.GetCustomers);
                migrationRequest.Headers.Add("MS-CorrelationId", Guid.NewGuid().ToString());

                migrationResponse = await httpClient.SendAsync(migrationRequest).ConfigureAwait(false);
            }

            migrationResponse.EnsureSuccessStatusCode();
            var newCommerceEligibility = await migrationResponse.Content.ReadFromJsonAsync<NewCommerceEligibility>().ConfigureAwait(false);
            if (newCommerceEligibility!.AddOnMigrations.Any())
            {
                addOnEligibilityList.Add(newCommerceEligibility.AddOnMigrations);
            }

            migrationRequests.Add(PrepareMigrationRequest(customer, subscription, newCommerceEligibility!));

            Interlocked.Increment(ref subscriptionsCntr);
            Console.WriteLine($"Validated migration eligibility for {subscriptionsCntr} subscriptions.");
        });

        foreach(var addOn in addOns)
        {
            var addOnEligibility = addOnEligibilityList.SelectMany(a => a).SingleOrDefault(m => m.CurrentSubscriptionId.Equals(addOn.Id, StringComparison.OrdinalIgnoreCase));
            if (addOnEligibility != null)
            {
                migrationRequests.Add(PrepareMigrationRequest(customer, addOn, addOnEligibility));
            }
        }

        return migrationRequests;
    }

    /// <summary>
    /// Prepares the migration request for CSV output.
    /// </summary>
    /// <param name="companyProfile">The company profile.</param>
    /// <param name="subscription">The subscription.</param>
    /// <param name="newCommerceEligibility">The new commerce eligibility.</param>
    /// <returns>The migration request.</returns>
    private static MigrationRequest PrepareMigrationRequest(CompanyProfile companyProfile, Subscription subscription, NewCommerceEligibility newCommerceEligibility)
    {
        return new MigrationRequest
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
            AddOn = !string.IsNullOrWhiteSpace(subscription.ParentSubscriptionId),
            BaseSubscriptionId = subscription.ParentSubscriptionId,
            MigrationIneligibilityReason = newCommerceEligibility.Errors.Any() ?
                            string.Join(";", newCommerceEligibility.Errors.Select(e => e.Description)) :
                            string.Empty
        };
    }

    /// <summary>
    /// Prepares the modern subscription for CSV output.
    /// </summary>
    /// <param name="companyProfile">The company profile.</param>
    /// <param name="subscription">The subscription.</param>
    /// <returns>The Modern Subscription.</returns>
    private static ModernSubscription PrepareModernSubscription(CompanyProfile companyProfile, Subscription subscription)
    {
        return new ModernSubscription
        {
            PartnerTenantId = partnerTenantId,
            IndirectResellerMpnId = subscription.PartnerId,
            CustomerName = companyProfile.CompanyName,
            CustomerTenantId = companyProfile.TenantId,
            SubscriptionId = subscription.Id,
            SubscriptionName = subscription.FriendlyName,
            ProductName = subscription.OfferName,
            ExpirationDate = subscription.CommitmentEndDate,
            Psa = subscription.OfferId,
            Term = subscription.TermDuration,
            BillingPlan = subscription.BillingCycle.ToString(),
            SeatCount = subscription.Quantity,
            MigratedFromSubscriptionId = subscription.MigratedFromSubscriptionId,
        };
    }
}