namespace NCEBulkMigrationTool;

internal interface ICustomerProvider
{
    Task<bool> ExportCustomersAsync();
}