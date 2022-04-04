// -----------------------------------------------------------------------
// <copyright file="TokenProvider.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCEBulkMigrationTool;

/// <summary>
/// The TokenProvider class.
/// </summary>
internal class TokenProvider : ITokenProvider
{
    /// <summary>
    /// TokenProvider constructor.
    /// </summary>
    /// <param name="appSettings">The app settings.</param>
    public TokenProvider(AppSettings appSettings)
    {
        this.appSettings = appSettings;
    }

    private readonly AppSettings appSettings;
    private AuthenticationResult? authenticationResult;

    /// <inheritdoc/>
    public async Task<AuthenticationResult> GetTokenAsync()
    {
        if (authenticationResult != null && authenticationResult.ExpiresOn > DateTimeOffset.UtcNow.AddMinutes(5))
        {
            return authenticationResult;
        }

        var scopes = new string[] { $"https://api.partnercenter.microsoft.com/.default" };
        var app = PublicClientApplicationBuilder.Create(this.appSettings.AppId)
            .WithAuthority("https://login.microsoftonline.com", this.appSettings.Domain, true)
            .WithRedirectUri("http://localhost")
            .Build();

        var accounts = await app.GetAccountsAsync();
        AuthenticationResult result;

        try
        {
            result = await app.AcquireTokenSilent(scopes, accounts.FirstOrDefault())
                   .ExecuteAsync();
        }
        catch (MsalUiRequiredException)
        {
            try
            {
                result = await app.AcquireTokenInteractive(scopes)
                    .WithLoginHint(this.appSettings.Upn)
                    .WithPrompt(Prompt.NoPrompt)
                    .ExecuteAsync();
            }
            catch (MsalException msalex)
            {
                throw msalex;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception while generating token {ex}");
            throw;
        }

        authenticationResult = result;

        return result;
    }
}