// -----------------------------------------------------------------------
// <copyright file="PartnerServiceSettingsSection.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Configuration
{
    using System;

    /// <summary>
    /// Holds the partner service settings section.
    /// </summary>
    public class PartnerServiceSettingsSection : Section
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PartnerServiceSettingsSection"/> class.
        /// </summary>
        public PartnerServiceSettingsSection() : base("PartnerServiceSettings")
        {
        }

        /// <summary>
        /// Gets the partner service API endpoint.
        /// </summary>
        public Uri PartnerServiceApiEndpoint
        {
            get
            {
                return new Uri(this.ConfigurationSection["PartnerServiceApiEndpoint"]);
            }
        }

        /// <summary>
        /// Gets the authentication authority (AAD) endpoint.
        /// </summary>
        public Uri AuthenticationAuthorityEndpoint
        {
            get
            {
                return new Uri(this.ConfigurationSection["AuthenticationAuthorityEndpoint"]);
            }
        }

        /// <summary>
        /// Gets the graph API end point.
        /// </summary>
        public Uri GraphEndpoint
        {
            get
            {
                return new Uri(this.ConfigurationSection["GraphEndpoint"]);
            }
        }

        /// <summary>
        /// Gets the AAD common domain.
        /// </summary>
        public string CommonDomain
        {
            get
            {
                return this.ConfigurationSection["CommonDomain"];
            }
        }
    }
}
