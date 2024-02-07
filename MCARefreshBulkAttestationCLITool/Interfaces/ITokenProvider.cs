// <copyright file="ITokenProvider.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MCARefreshBulkAttestationCLITool.Interfaces
{
    using System.Threading.Tasks;

    public interface ITokenProvider
    {
        Task<string> GetTokenAsync();

        Task<string> GetTenantIdAsync();
    }
}
