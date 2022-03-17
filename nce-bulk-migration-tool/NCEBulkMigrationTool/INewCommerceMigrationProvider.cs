namespace NCEBulkMigrationTool;

internal interface INewCommerceMigrationProvider
{
    Task<bool> UploadNewCommerceMigrationsAsync();

    Task<bool> ExportNewCommerceMigrationStatusAsync();
}
