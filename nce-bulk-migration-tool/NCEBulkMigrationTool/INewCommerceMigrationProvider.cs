// -----------------------------------------------------------------------
// <copyright file="INewCommerceMigrationProvider.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCEBulkMigrationTool;

/// <summary>
/// The INewCommerceMigrationProvider interface.
/// </summary>
internal interface INewCommerceMigrationProvider
{
    /// <summary>
    /// Uploads New Commerce Migrations based on CSV files in the input folders and writes the migration data to a new CSV file.
    /// </summary>
    /// <returns>Bool indicating success/ failure.</returns>
    Task<bool> UploadNewCommerceMigrationsAsync();

    /// <summary>
    /// Exports the latest New Commerce Migration Status for given input migrations into a new CSV file.
    /// </summary>
    /// <returns>Bool indicating success/ failure.</returns>
    Task<bool> ExportNewCommerceMigrationStatusAsync();
}