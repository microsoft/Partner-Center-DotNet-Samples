namespace NCEBulkMigrationTool;

internal class TokenProvider : ITokenProvider
{
    public TokenProvider(AppSettings appSettings)
    {
        this.appSettings = appSettings;
    }

    private readonly AppSettings appSettings;
    private AuthenticationResult? authenticationResult;

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