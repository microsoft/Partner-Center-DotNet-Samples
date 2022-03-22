// -----------------------------------------------------------------------
// <copyright file="ISubscriptionProvider.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCEBulkMigrationTool;

/// <summary>
/// The ISubscriptionProvider interface.
/// </summary>
internal interface ISubscriptionProvider
{
    /// <summary>
    /// Exports legacy commerce subscriptions with their migration eligibility to an output CSV file.
    /// </summary>
    /// <returns>Bool indicating success/ failure.</returns>
    Task<bool> ExportLegacySubscriptionsAsync();

    /// <summary>
    /// Exports NCE subscriptions to an output CSV file.
    /// </summary>
    /// <returns>Bool indicating success/ failure.</returns>
    Task<bool> ExportModernSubscriptionsAsync();
}