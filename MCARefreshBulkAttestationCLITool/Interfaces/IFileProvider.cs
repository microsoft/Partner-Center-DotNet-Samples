// <copyright file="IFileProvider.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MCARefreshBulkAttestationCLITool.Interfaces
{
    using System.Collections.Generic;
    using MCARefreshBulkAttestationCLITool.Models;

    public interface IFileProvider
    {
        /// <summary>
        /// Reads the file from the local file system.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        Task<IEnumerable<CustomerAgreementRecord>> ReadFromLocalFile(string fileName);
    }
}
