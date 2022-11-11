// -----------------------------------------------------------------------
// <copyright file="CustomerProvider.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCEBulkMigrationTool;

/// <summary>
/// The CustomerProvider class.
/// </summary>
internal class CustomerProvider : ICustomerProvider
{
    /// <summary>
    /// The token provider.
    /// </summary>
    private readonly ITokenProvider tokenProvider;

    /// <summary>
    /// CustomerProvider constructor.
    /// </summary>
    /// <param name="tokenProvider">The token provider.</param>
    public CustomerProvider(ITokenProvider tokenProvider)
    {
        this.tokenProvider = tokenProvider;
    }

    /// <inheritdoc/>
    public async Task<bool> ExportCustomersAsync()
    {
        Console.WriteLine("Generating token...");
        var authenticationResult = await this.tokenProvider.GetTokenAsync();
        Console.WriteLine("Token generated...");

        var httpClient = new HttpClient
        {
            BaseAddress = new Uri(Routes.BaseUrl)
        };
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticationResult.AccessToken);
        httpClient.DefaultRequestHeaders.Add(Constants.PartnerCenterClientHeader, Constants.ClientName);

        Console.WriteLine("Getting customers");

        var allCustomers = new List<CompanyProfile>();
        string continuationToken = string.Empty;
        string route = Routes.GetCustomers;

        do
        {
            var request = new HttpRequestMessage(HttpMethod.Get, route);
            request.Headers.Add("MS-CorrelationId", Guid.NewGuid().ToString());
            if (!string.IsNullOrWhiteSpace(continuationToken))
            {
                request.Headers.Add("MS-ContinuationToken", continuationToken);
                route = $"{Routes.GetCustomers}&seekOperation=next";
            }

            request.RequestUri = new Uri(route, UriKind.Relative);

            var response = await httpClient.SendAsync(request).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var customerErrorResponse = await response.Content.ReadFromJsonAsync<CustomerErrorResponse>();
                //// If we get error code 2000 then it means we fetched all the records.
                if (customerErrorResponse?.Code == 2000)
                {
                    break;
                }
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                authenticationResult = await this.tokenProvider.GetTokenAsync();
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticationResult.AccessToken);
                httpClient.DefaultRequestHeaders.Add(Constants.PartnerCenterClientHeader, Constants.ClientName);
                request = new HttpRequestMessage(HttpMethod.Get, route);
                request.Headers.Add("MS-CorrelationId", Guid.NewGuid().ToString());
                request.Headers.Add("MS-ContinuationToken", continuationToken);

                response = await httpClient.SendAsync(request).ConfigureAwait(false);
            }

            response.EnsureSuccessStatusCode();
            var customerCollection = await response.Content.ReadFromJsonAsync<ResourceCollection<Customer>>();

            if (customerCollection?.Items.Any() == false)
            {
                Console.WriteLine("No customers found");
                Console.ReadLine();
                return true;
            }

            continuationToken = customerCollection!.ContinuationToken;

            var customers = customerCollection!.Items.Select(customer => customer.CompanyProfile);
            allCustomers.AddRange(customers);
        } while (!string.IsNullOrWhiteSpace(continuationToken));

        var csvProvider = new CsvProvider();

        Console.WriteLine("Exporting customers");
        await csvProvider.ExportCsv(allCustomers, $"{Constants.OutputFolderPath}/customers.csv");
        Console.WriteLine($"Exported customers at {Environment.CurrentDirectory}/{Constants.OutputFolderPath}/customers.csv");

        return true;
    }
}