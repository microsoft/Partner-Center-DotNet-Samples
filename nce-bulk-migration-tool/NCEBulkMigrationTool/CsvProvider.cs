namespace NCEBulkMigrationTool;

internal class CsvProvider
{
    public async Task ExportCsv<T>(IEnumerable<T> data, string fileName)
    {
        int index = fileName.LastIndexOf('/');
        var directory = fileName[..index];
        Directory.CreateDirectory(directory);

        using var subscriptionsWriter = new StreamWriter(fileName);
        using var subscriptionsCsvWriter = new CsvWriter(subscriptionsWriter, CultureInfo.InvariantCulture);
        await subscriptionsCsvWriter.WriteRecordsAsync(data);
    }
}