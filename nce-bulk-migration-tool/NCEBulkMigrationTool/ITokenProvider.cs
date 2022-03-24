// -----------------------------------------------------------------------
// <copyright file="ITokenProvider.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCEBulkMigrationTool;

/// <summary>
/// The ITokenProvider interface.
/// </summary>
internal interface ITokenProvider
{
    /// <summary>
    /// Gets a Partner token to authenticate with Partner Center APIs.
    /// </summary>
    /// <returns>AuthenticationResult containing the token.</returns>
    Task<AuthenticationResult> GetTokenAsync();
}