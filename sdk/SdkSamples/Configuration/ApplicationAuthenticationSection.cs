// -----------------------------------------------------------------------
// <copyright file="ApplicationAuthenticationSection.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Configuration
{
    /// <summary>
    /// Holds an application authentication section settings.
    /// </summary>
    public class ApplicationAuthenticationSection : Section
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationAuthenticationSection"/> class.
        /// </summary>
        /// <param name="sectionName">The application authentication section name.</param>
        public ApplicationAuthenticationSection(string sectionName) : base(sectionName)
        {
        }

        /// <summary>
        /// Gets the AAD application ID.
        /// </summary>
        public string ApplicationId
        {
            get
            {
                return this.ConfigurationSection["ApplicationId"];
            }
        }

        /// <summary>
        /// Gets AAD application secret.
        /// </summary>
        public string ApplicationSecret
        {
            get
            {
                return this.ConfigurationSection["ApplicationSecret"];
            }
        }

        /// <summary>
        /// Gets AAD Domain which hosts the application.
        /// </summary>
        public string Domain
        {
            get
            {
                return this.ConfigurationSection["Domain"];
            }
        }
    }
}
