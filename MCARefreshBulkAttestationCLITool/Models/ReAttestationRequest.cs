// <copyright file="ReAttestationRequest.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MCARefreshBulkAttestationCLITool.Models
{
    using System.Text.Json.Serialization;

    public class ReAttestationRequest
    {
        [JsonPropertyName("partnerId")]
        public string PartnerId { get; set; }

        [JsonPropertyName("customerId")]
        public string CustomerId { get; set; }

        [JsonPropertyName("agreement")]
        public Agreement Agreement { get; set; }

        [JsonPropertyName("customerDirectAcceptance")]
        public bool CustomerDirectAcceptance { get; set; }
    }

    public class Agreement
    {
        public static readonly string MicrosoftCustomerAgreementTemplateId = "117a77b0-9360-443b-8795-c6dedc750cf9";

        [JsonPropertyName("templateId")]
        public string TemplateId { get; set; }

        [JsonPropertyName("primaryContact")]
        public LastAgreementPrimaryContact PrimaryContact { get; set; }

        [JsonPropertyName("dateAgreed")]
        public DateTime DateAgreed { get; set; }
    }
}
