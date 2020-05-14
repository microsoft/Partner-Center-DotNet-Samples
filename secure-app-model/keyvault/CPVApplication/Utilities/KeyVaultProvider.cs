// -----------------------------------------------------------------------
// <copyright file="KeyVaultProvider.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace CPVApplication.Utilities
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
        /// <summary>
        /// The base address for the instance of Azure Key Vault.
        /// </summary>
        private readonly string keyVaultUrl = ConfigurationManager.AppSettings["ida:keyVaultUrl"];

        /// <summary>
        /// The clientId for the Azure AD application configured to access the instance of Azure Key Vault.
        /// </summary>
        private readonly string clientId = ConfigurationManager.AppSettings["ida:KeyVaultClientId"];

        /// <summary>
        /// The tenantId for the Azure AD application configured to access the instance of Azure Key Vault.
        /// </summary>
        private readonly string tenantId = ConfigurationManager.AppSettings["ida:KeyVaultTenantId"];

        /// <summary>
        /// The application secret for the Azure AD application configured to access the instance of Azure Key Vault.
        /// </summary>
        private readonly string clientSecret = ConfigurationManager.AppSettings["ida:KeyVaultClientSecret"];

        /// <summary>
        /// The client used to interact with the instance of Azure Key Vault.
        /// </summary>
        private readonly SecretClient secretClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyVaultProvider" /> class.
        /// </summary>
        public KeyVaultProvider()
        {
            ClientSecretCredential credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
            secretClient = new SecretClient(new Uri(keyVaultUrl), credential);
        }

        /// <summary>
        /// Gets the specified entity from the vault. 
        /// </summary>
        /// <param name="key">Identifier of the entity to be retrieved.</param>
        /// <returns>The value retrieved from the vault.</returns>
        public async Task<string> GetSecretAsync(string keyName)
        {
            KeyVaultSecret secret = await secretClient.GetSecretAsync(keyName);
            return secret.Value;
        }
    }
}
