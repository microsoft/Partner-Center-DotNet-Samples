// <copyright file="FetchCustomerAgreementRecordResponse.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MCARefreshBulkAttestationCLITool.Models
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class FetchCustomerAgreementRecordResponse
    {
        [JsonPropertyName("customerAgreementRecords")]
        public List<CustomerAgreementRecord> CustomerAgreementRecords { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("continuationToken")]
        public string ContinuationToken { get; set; }
    }
}
