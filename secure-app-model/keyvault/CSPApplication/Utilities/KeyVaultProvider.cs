// -----------------------------------------------------------------------
// <copyright file="KeyVaultProvider.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace CSPApplication.Utilities
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
        private readonly string KeyVaultClientId = ConfigurationManager.AppSettings["ida:KeyVaultClientId"];
        private readonly string KeyVaultClientSecret = ConfigurationManager.AppSettings["ida:KeyVaultClientSecret"];
        private readonly string BaseUrl = ConfigurationManager.AppSettings["KeyVaultEndpoint"];

        public async Task<string> GetSecretAsync(string key)
        {
            KeyVaultClient keyVault = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(this.GetToken), new System.Net.Http.HttpClient());
            SecretBundle secret = await keyVault.GetSecretAsync(this.BaseUrl, key.Replace("@", string.Empty).Replace(".", string.Empty));
            return secret.Value;
        }

        private async Task<string> GetToken(string authority, string resource, string scope)
        {
            JObject tokenResult = await AuthorizationUtilities.GetADAppToken(authority, resource, this.KeyVaultClientId, this.KeyVaultClientSecret);
            return tokenResult["access_token"].ToString();
        }
    }
}