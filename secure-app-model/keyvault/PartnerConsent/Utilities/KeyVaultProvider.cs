// -----------------------------------------------------------------------
// <copyright file="KeyVaultProvider.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace PartnerConsent.Utilities
{
    using System;
    using System.Configuration;
    using System.Threading.Tasks;
    using Azure.Identity;
    using Azure.Security.KeyVault.Secrets;


    /// <summary>
    /// Provider for accessing secrets from the Azure KeyVault
    /// </summary>
    public class KeyVaultProvider
    {
        private readonly string keyVaultUrl = ConfigurationManager.AppSettings["ida:keyVaultUrl"];
        private readonly string tenantId = ConfigurationManager.AppSettings["ida:KeyVaultTenantId"];
        private readonly string clientId = ConfigurationManager.AppSettings["ida:KeyVaultClientId"];
        private readonly string clientSecret = ConfigurationManager.AppSettings["ida:KeyVaultClientSecret"];

        /// <summary>
        /// Get secret value from azure key vault
        /// </summary>
        /// <param name="key">key name</param>
        /// <returns>key value in string format</returns>
        public async Task<string> GetSecretAsync(string key)
        {
            ClientSecretCredential credential = new ClientSecretCredential(tenantId, clientId, clientSecret);

            SecretClient client = new SecretClient(new Uri(keyVaultUrl), credential);
            KeyVaultSecret secret = await client.GetSecretAsync(key);

            return secret.Value;
        }

        /// <summary>
        /// Add or overrides a key value in Azure key vault
        /// </summary>
        /// <param name="key">key name</param>
        /// <param name="value">secret value</param>
        /// <returns></returns>
        public async Task AddSecretAsync(string key, string value)
        {
            ClientSecretCredential credential = new ClientSecretCredential(tenantId, clientId, clientSecret);

            SecretClient client = new SecretClient(new Uri(keyVaultUrl), credential);
            KeyVaultSecret secret = await client.SetSecretAsync(key, value);
        }
    }
}