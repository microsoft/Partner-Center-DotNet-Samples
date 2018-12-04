// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace CSPApplication
{
    using System;
    using System.Configuration;
    using System.Threading.Tasks;
    using CSPApplication.Utilities;
    using Microsoft.Store.PartnerCenter;
    using Microsoft.Store.PartnerCenter.Extensions;
    using Microsoft.Store.PartnerCenter.Models;
    using Microsoft.Store.PartnerCenter.Models.Users;
    using Newtonsoft.Json;

    internal class Program
    {
        /// <summary>
        /// The following code assumes that the context of the partner is pre determined by some external code 
        /// (based on marketplace application logic: either by product or by partner context)
        /// </summary>

        private static readonly string CSPApplicationId = ConfigurationManager.AppSettings["ida:CSPApplicationId"];
        private static readonly string CSPApplicationSecret = ConfigurationManager.AppSettings["ida:CSPApplicationSecret"];

        private static void Main(string[] args)
        {
            Task.Run(() => Run()).Wait();
        }

        private static async Task Run()
        {
            // The following properties indicate which partner and customer context the calls are going to be made.
            string PartnerId = "<Partner tenant id>";
            string CustomerId = "<Customer tenant id>";

            Console.WriteLine(" ===================== Partner center  API calls ============================", DateTime.Now);
            IAggregatePartner ops = await GetUserPartnerOperationsAsync(PartnerId);
            SeekBasedResourceCollection<CustomerUser> customerUsers = ops.Customers.ById(CustomerId).Users.Get();
            Console.WriteLine(JsonConvert.SerializeObject(customerUsers));

            Console.WriteLine(" ===================== Partner graph  API calls ============================", DateTime.Now);
            Tuple<string, DateTimeOffset> tokenResult = await LoginToGraph(PartnerId);
            Newtonsoft.Json.Linq.JObject mydetails = await ApiCalls.GetAsync(tokenResult.Item1, "https://graph.microsoft.com/v1.0/me");
            Console.WriteLine(JsonConvert.SerializeObject(mydetails));

            // CSP partner applications are pre-consented into customer tenant, so they do not need a custom consent from customer.
            // direct token acquire to a customer tenant should work.
            Console.WriteLine(" ===================== Customer graph  API calls ============================", DateTime.Now);
            Tuple<string, DateTimeOffset> tokenCustomerResult = await LoginToCustomerGraph(PartnerId, CustomerId);
            Newtonsoft.Json.Linq.JObject customerDomainsUsingGraph = await ApiCalls.GetAsync(tokenCustomerResult.Item1, "https://graph.windows.net/" + CustomerId + "/domains?api-version=1.6");
            Console.WriteLine(JsonConvert.SerializeObject(customerDomainsUsingGraph));

            Console.ReadLine();
        }

        public static async Task<Tuple<string, DateTimeOffset>> LoginToPartnerCenter(string tenantId)
        {
            KeyVaultProvider provider = new KeyVaultProvider();
            string refreshToken = await provider.GetSecretAsync(tenantId);

            Newtonsoft.Json.Linq.JObject token = await AuthorizationUtilities.GetAADTokenFromRefreshToken(
                "https://login.microsoftonline.com/" + tenantId,
                "https://api.partnercenter.microsoft.com",
                CSPApplicationId,
                CSPApplicationSecret,
                refreshToken);

            return new Tuple<string, DateTimeOffset>(token["access_token"].ToString(), DateTimeOffset.UtcNow + TimeSpan.FromTicks(long.Parse(token["expires_on"].ToString())));
        }

        /// <summary>
        /// Generates a token for Partner tenant using partner user refresh token
        /// The expiry time calculation explined below is not strong. 
        /// please use stardard ADAL library to gain access token by refresh token, which provides strongly typed classes with proper expirty time calculation.
        /// </summary>
        /// <param name="partnerTenantId">partner tenant id</param>
        /// <returns>
        /// Access token and expiry time. 
        /// </returns>
        public static async Task<Tuple<string, DateTimeOffset>> LoginToGraph(string partnerTenantId)
        {
            KeyVaultProvider provider = new KeyVaultProvider();
            string refreshToken = await provider.GetSecretAsync(partnerTenantId);

            Newtonsoft.Json.Linq.JObject token = await AuthorizationUtilities.GetAADTokenFromRefreshToken(
                "https://login.microsoftonline.com/" + partnerTenantId,
                "https://graph.microsoft.com",
                CSPApplicationId,
                CSPApplicationSecret,
                refreshToken);

            return new Tuple<string, DateTimeOffset>(token["access_token"].ToString(), DateTimeOffset.UtcNow + TimeSpan.FromTicks(long.Parse(token["expires_on"].ToString())));
        }

        /// <summary>
        /// Generates a token for Customer tenant using partner user refresh token
        /// The expiry time calculation explined below is not strong. 
        /// please use stardard ADAL library to gain access token by refresh token, which provides strongly typed classes with proper expirty time calculation.
        /// </summary>
        /// <param name="partnerTenantId">partner tenant id</param>
        /// <param name="customerTenantId">customer tenant id</param>
        /// <returns>
        /// Access token and expiry time. 
        /// </returns>
        public static async Task<Tuple<string, DateTimeOffset>> LoginToCustomerGraph(string partnerTenantId, string customerTenantId)
        {
            KeyVaultProvider provider = new KeyVaultProvider();
            string refreshToken = await provider.GetSecretAsync(partnerTenantId);

            Newtonsoft.Json.Linq.JObject token = await AuthorizationUtilities.GetAADTokenFromRefreshToken(
                "https://login.microsoftonline.com/" + customerTenantId,
                "https://graph.windows.net",
                CSPApplicationId,
                CSPApplicationSecret,
                refreshToken);

            return new Tuple<string, DateTimeOffset>(token["access_token"].ToString(), DateTimeOffset.UtcNow + TimeSpan.FromTicks(long.Parse(token["expires_on"].ToString())));
        }

        /// <summary>
        /// Using Partner center .NET SDK to make partner center calls. If you are using java application, you can either us REST calls or use Java SDK to make partner center calls
        /// In all the mentioned cases, the token generated by LoginToPartnerCenter method should work.
        /// </summary>
        /// <param name="PartnerId">partner tenant id</param>
        /// <returns>SDK reference for contextual calls</returns>
        public static async Task<IAggregatePartner> GetUserPartnerOperationsAsync(string PartnerId)
        {
            Tuple<string, DateTimeOffset> aadAuthenticationResult = await LoginToPartnerCenter(PartnerId);

            // Authenticate by user context with the partner service
            IPartnerCredentials userCredentials = PartnerCredentials.Instance.GenerateByUserCredentials(
                CSPApplicationId,
                new AuthenticationToken(
                    aadAuthenticationResult.Item1,
                    aadAuthenticationResult.Item2),
                async delegate
                {
                    // token has expired, re-Login to Azure Active Directory
                    Tuple<string, DateTimeOffset> aadToken = await LoginToPartnerCenter(PartnerId);
                    return new AuthenticationToken(aadToken.Item1, aadToken.Item2);
                });

            return PartnerService.Instance.CreatePartnerOperations(userCredentials);
        }
    }
}