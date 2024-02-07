// <copyright file="CustomerAgreementRecord.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MCARefreshBulkAttestationCLITool.Models
{
    using System.Text.Json.Serialization;

    public class CustomerAgreementRecord
    {
        [JsonPropertyName("customerTenantId")]
        public string CustomerTenantId { get; set; }

        [JsonPropertyName("lastAgreementPrimaryContact")]
        public LastAgreementPrimaryContact LastAgreementPrimaryContact { get; set; }

        [JsonPropertyName("lastAgreementDateEpoch")]
        public long LastAgreementDateEpoch { get; set; }

        public string LastAgreementDate
        {
            get
            {
                if (this.LastAgreementDateEpoch == default)
                {
                    return string.Empty;
                }

                var offset = $"{this.LastAgreementDateEpoch}".Length > 10 ? DateTimeOffset.FromUnixTimeMilliseconds(this.LastAgreementDateEpoch) : DateTimeOffset.FromUnixTimeSeconds(this.LastAgreementDateEpoch);
                return offset.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        [JsonPropertyName("partnerAttestationCompleted")]
        public bool PartnerAttestationCompleted { get; set; } = false;

        public string CustomerAccountLink
        {
            get => $"https://partner.microsoft.com/en-us/dashboard/commerce2/customers/{this.CustomerTenantId}/account";
        }
    }
}
