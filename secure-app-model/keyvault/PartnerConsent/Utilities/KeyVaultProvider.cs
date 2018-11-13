using System.Configuration;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;

namespace PartnerConsent.Utilities
{

    /// <summary>
    /// Provider for accessing secrets from the Azure KeyVault
    /// </summary>
    public class KeyVaultProvider
    {
        private string KeyVaultClientId = ConfigurationManager.AppSettings["ida:KeyVaultClientId"];
        private string KeyVaultClientSecret = ConfigurationManager.AppSettings["ida:KeyVaultClientSecret"];
        private string BaseUrl = @"https://cpvkeyvault.vault.azure.net/";

        /// <summary>
        /// Get secret value from azure key vault
        /// </summary>
        /// <param name="key">key name</param>
        /// <returns>key value in string format</returns>
        public async Task<string> GetSecretAsync(string key)
        {
            var keyVault = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(this.GetToken), new System.Net.Http.HttpClient());

            var secretIdentifier = this.BaseUrl + "/secrets/" + key.Replace("@", string.Empty).Replace(".", string.Empty);
            var secret = await keyVault.GetSecretAsync(secretIdentifier);

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
            var keyVault = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(this.GetToken), new System.Net.Http.HttpClient());
            await keyVault.SetSecretAsync(this.BaseUrl, key.Replace("@", string.Empty).Replace(".", string.Empty), value);
        }

        /// <summary>
        /// Get application token for the app which is given access to Azure keyvault
        /// Called by delegate in azure key vault SDK
        /// </summary>
        /// <param name="authority">AAD authority</param>
        /// <param name="resource">Azure key vault resource</param>
        /// <param name="scope">authorization scope</param>
        /// <returns></returns>
        private async Task<string> GetToken(string authority, string resource, string scope)
        {
            var tokenResult = await AuthorizationUtilities.GetADAppToken(authority, resource, this.KeyVaultClientId, this.KeyVaultClientSecret);
            return tokenResult["access_token"].ToString();
        }
    }
}