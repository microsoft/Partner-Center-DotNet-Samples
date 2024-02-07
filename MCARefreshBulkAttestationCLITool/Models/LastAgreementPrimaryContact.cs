// <copyright file="LastAgreementPrimaryContact.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MCARefreshBulkAttestationCLITool.Models
{
    using System.Text.Json.Serialization;

    public class LastAgreementPrimaryContact
    {
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("phoneNumber")]
        public string PhoneNumber { get; set; }
    }
}
