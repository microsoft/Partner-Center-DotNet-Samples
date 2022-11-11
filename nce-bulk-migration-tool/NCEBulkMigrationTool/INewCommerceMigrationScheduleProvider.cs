// -----------------------------------------------------------------------
// <copyright file="INewCommerceMigrationScheduleProvider.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCEBulkMigrationTool
{
    internal interface INewCommerceMigrationScheduleProvider
    {
        /// <summary>
        /// Exports legacy commerce subscriptions with their migration eligibility to an output CSV file so that 
        /// they can be scheduled as needed.
        /// </summary>
        /// <returns>Bool indicating success/ failure.</returns>
        Task<bool> ValidateAndGetSubscriptionsToScheduleMigrationAsync();

        /// <summary>
        /// Uploads New Commerce Migration Schedules based on CSV files in the input folders and writes the schedule migration data to a new CSV file.
        /// </summary>
        /// <returns>Bool indicating success/ failure.</returns>
        Task<bool> UploadNewCommerceMigrationSchedulesAsync();

        /// <summary>
        /// Exports all the New Commerce Migration Schedules for the given input list of customers.
        /// </summary>
        /// <returns>Bool indicating success/ failure.</returns>
        Task<bool> ExportNewCommerceMigrationSchedulesAsync();

        /// <summary>
        /// Cancels the New Commerce Migration Schedules based on the CSV input files and writes the output with the updated Schedule Migration Status.
        /// </summary>
        /// <returns>Bool indicating success/ failure.</returns>
        Task<bool> CancelNewCommerceMigrationSchedulesAsync();
    }
}