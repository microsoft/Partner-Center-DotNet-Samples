// <copyright file="TokenProvider.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MCARefreshBulkAttestationCLITool.Providers
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Identity.Client;
    using MCARefreshBulkAttestationCLITool.Interfaces;
    using Microsoft.Extensions.Logging;

    public class TokenProvider : ITokenProvider
    {
        private readonly AppSettings appSettings;
        private readonly ILogger<TokenProvider> logger;

        private AuthenticationResult? authenticationResult;

        public TokenProvider(AppSettings appSettings, ILogger<TokenProvider> logger)
        {
            this.appSettings = appSettings;
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string> GetTokenAsync()
        {
            if (authenticationResult != null && authenticationResult.ExpiresOn > DateTimeOffset.UtcNow.AddMinutes(5))
            {
                return authenticationResult.AccessToken;
            }

            await GetAuthenticationResult();

            if (authenticationResult != null)
            {
                return authenticationResult.AccessToken;
            }

            throw new Exception($"Unable to acquire token.");
        }

        public async Task<string> GetTenantIdAsync()
        {
            if (authenticationResult != null)
            {
                return authenticationResult.TenantId;
            }

            await GetAuthenticationResult();

            if (authenticationResult != null)
            {
                return authenticationResult.TenantId;
            }

            throw new Exception($"Unable to retrieve tenant ID.");
        }

        private async Task GetAuthenticationResult()
        {
            var scope = "https://api.partnercenter.microsoft.com/.default";
            var authority = "https://login.microsoftonline.com";

            var scopes = new string[] { scope };
            var app = PublicClientApplicationBuilder.Create(appSettings.ApplicationId)
                .WithAuthority(authority, appSettings.Domain, true)
                .WithRedirectUri("http://localhost/")
                .Build();

            var accounts = await app.GetAccountsAsync();
            AuthenticationResult result;

            try
            {
                result = await app.AcquireTokenSilent(scopes, accounts.FirstOrDefault())
                       .ExecuteAsync();
            }
            catch (MsalUiRequiredException ex)
            {
                this.logger.LogError(ex, "Interaction required for authentication.");

                try
                {
                    result = await app.AcquireTokenInteractive(scopes)
                        .WithLoginHint(appSettings.UserPrincipalName)
                        .WithPrompt(Prompt.NoPrompt)
                        .ExecuteAsync();
                }
                catch (MsalException msalex)
                {
                    this.logger.LogError(msalex, "Interactive token acquisition failed.");
                    throw;
                }

                if (!this.appSettings.IsMfaExcluded)
                {
                    var handler = new JwtSecurityTokenHandler();
                    var accessToken = handler.ReadJwtToken(result.AccessToken);
                    var amrClaims = accessToken.Claims.Where(c => c.Type == "amr");

                    if (!amrClaims.Any(c => c.Value.EndsWith("mfa")))
                    {
                        this.logger.LogError("MFA claim is missing from the token. MFA must be enforced in order to access Partner Center Customer Workspace APIs.");
                        throw new Exception("Partner account is not MFA enforced.");
                    }
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Silent token acquisition failed.");
                throw;
            }

            authenticationResult = result;
        }
    }
}
