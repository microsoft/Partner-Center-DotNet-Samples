namespace NCEBulkMigrationTool;

internal interface ISubscriptionProvider
{
    Task<bool> ExportLegacySubscriptionsAsync();

    Task<bool> ExportModernSubscriptionsAsync();
}