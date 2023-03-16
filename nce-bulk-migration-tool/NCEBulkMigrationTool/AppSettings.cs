// -----------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCEBulkMigrationTool;

/// <summary>
/// The app settings record.
/// </summary>
internal record AppSettings
{
    /// <summary>
    /// Gets or sets the application id.
    /// </summary>
    public string AppId { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the user principal name.
    /// </summary>
    public string Upn { get; init; } = string.Empty;

    /// <summary>
    /// Gets the domain from the user principal name.
    /// </summary>
    public string Domain
    {
        get
        {
            var index = this.Upn.LastIndexOf("@");
            if (index == -1) { return string.Empty; }
            return this.Upn[++index..];
        }
    }

    /// <summary>
    /// Gets or sets a flag indicating whether to use app only token.
    /// </summary>
    public bool UseAppToken { get; set; }
}