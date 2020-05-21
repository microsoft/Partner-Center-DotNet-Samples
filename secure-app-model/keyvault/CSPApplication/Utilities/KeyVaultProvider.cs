// -----------------------------------------------------------------------
// <copyright file="KeyVaultProvider.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace CSPApplication.Utilities
{
    using Azure.Identity;
    using Azure.Security.KeyVault.Secrets;
    using System;
    using System.Configuration;
    using System.Threading.Tasks;

    /// <summary>
    /// Provider for accessing secrets from the Azure KeyVault
    /// </summary>
    public class KeyVaultProvider
    {
        private readonly string keyVaultUrl = ConfigurationManager.AppSettings["KeyVaultUrl"];
        private readonly string tenantId = ConfigurationManager.AppSettings["ida:KeyVaultTenantId"];
        private readonly string clientId = ConfigurationManager.AppSettings["ida:KeyVaultClientId"];
        private readonly string clientSecret = ConfigurationManager.AppSettings["ida:KeyVaultClientSecret"];

        public async Task<string> GetSecretAsync(string keyName)
        {
            ClientSecretCredential credential = new ClientSecretCredential(tenantId, clientId, clientSecret);

            SecretClient client = new SecretClient(new Uri(keyVaultUrl), credential);
            KeyVaultSecret secret = await client.GetSecretAsync(keyName);

            return secret.Value;
        }
    }
}
