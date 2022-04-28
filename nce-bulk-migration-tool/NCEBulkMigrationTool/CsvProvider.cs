// -----------------------------------------------------------------------
// <copyright file="CsvProvider.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCEBulkMigrationTool;

/// <summary>
/// The CsvProvider class.
/// </summary>
internal class CsvProvider
{
    /// <summary>
    /// Exports provided data to CSV format.
    /// </summary>
    /// <typeparam name="T">The type of data to be translated to CSV format.</typeparam>
    /// <param name="data">The list of data to export to CSV.</param>
    /// <param name="fileName">The filename to write CSV data to.</param>
    /// <returns>No return.</returns>
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