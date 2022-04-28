// -----------------------------------------------------------------------
// <copyright file="ICustomerProvider.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCEBulkMigrationTool;

/// <summary>
/// The ICustomerProvider interface.
/// </summary>
internal interface ICustomerProvider
{
    /// <summary>
    /// Export all customers into CSV file.
    /// </summary>
    /// <returns>Bool for success/ failure.</returns>
    Task<bool> ExportCustomersAsync();
}