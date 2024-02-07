// <copyright file="AppSettings.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MCARefreshBulkAttestationCLITool
{
    public record AppSettings
    {
        public string ApplicationId { get; init; } = string.Empty;

        public string UserPrincipalName { get; init; } = string.Empty;

        public bool IsMfaExcluded { get; init; }

        public string Domain
        {
            get
            {
                var index = this.UserPrincipalName.LastIndexOf("@");
                if (index == -1) { return string.Empty; }
                return this.UserPrincipalName[++index..];
            }
        }
    }
}
