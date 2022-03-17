namespace NCEBulkMigrationTool;

internal interface ITokenProvider
{
    Task<AuthenticationResult> GetTokenAsync();
}