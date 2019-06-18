// -----------------------------------------------------------------------
// <copyright file="KeyVaultProvider.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace CPVApplication.Utilities
{
    using System.Configuration;
    using System.Threading.Tasks;
    using Microsoft.Azure.KeyVault;
    using Microsoft.Azure.KeyVault.Models;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Provider for accessing secrets from the Azure KeyVault
    /// </summary>
    public class KeyVaultProvider
    {
        /// <summary>
        /// The client used to perform HTTP operations. 
        /// </summary>
        private static readonly HttpClient httpClient = new HttpClient();

        /// <summary>
        /// The base address for the instance of Azure Key Vault.
        /// </summary>
        private readonly string BaseUrl = ConfigurationManager.AppSettings["KeyVaultEndpoint"];

        /// <summary>
        /// The identifier for the Azure AD application configured to access the instance of Azure Key Vault.
        /// </summary>
        private readonly string KeyVaultClientId = ConfigurationManager.AppSettings["ida:KeyVaultClientId"];

        /// <summary>
        /// The application secret for the Azure AD application configured to access the instance of Azure Key Vault.
        /// </summary>
        private readonly string KeyVaultClientSecret = ConfigurationManager.AppSettings["ida:KeyVaultClientSecret"];

        /// <summary>
        /// The client used to interact with the instance of Azure Key Vault.
        /// </summary>
        private readonly KeyVaultClient keyVaultClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyVaultProvider" /> class.
        /// </summary>
        public KeyVaultProvider()
        {
            keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(GetToken), httpClient);
        }

        /// <summary>
        /// Gets the specified entity from the vault. 
        /// </summary>
        /// <param name="key">Identifier of the entity to be retrieved.</param>
        /// <returns>The value retrieved from the vault.</returns>
        public async Task<string> GetSecretAsync(string key)
        {
            SecretBundle secret = await keyVaultClient.GetSecretAsync(BaseUrl, key.Replace("@", string.Empty).Replace(".", string.Empty)).ConfigureAwait(false);
            return secret.Value;
        }

        private async Task<string> GetToken(string authority, string resource, string scope)
        {
            JObject tokenResult = await AuthorizationUtilities.GetADAppToken(authority, resource, KeyVaultClientId, KeyVaultClientSecret);
            return tokenResult["access_token"].ToString();
        }
    }
}