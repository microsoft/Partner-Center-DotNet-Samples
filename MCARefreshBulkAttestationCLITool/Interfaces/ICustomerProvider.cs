// <copyright file="ICustomerProvider.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MCARefreshBulkAttestationCLITool.Interfaces
{
    using System.Threading.Tasks;
    using MCARefreshBulkAttestationCLITool.Models;
    using Refit;

    public interface ICustomerProvider
    {
        Task<bool> FetchAndSaveCustomerAgreementRecords();

        Task<bool> UpdateCustomerAgreementRecords(string partnerTenantId);
    }

    public interface ICustomerAgreementsClient
    {
        [Get("/v1/partners/customeragreementrecords")]
        Task<FetchCustomerAgreementRecordResponse> GetCustomerAgreementRecords([Query][AliasAs("continuation_token")] string? continuationToken = null, CancellationToken cancellationToken = default);

        [Post("/v1/CreateBulkReAttestation")]
        Task CreateBulkReAttestation(IEnumerable<ReAttestationRequest> request, CancellationToken cancellationToken = default);
    }
}
