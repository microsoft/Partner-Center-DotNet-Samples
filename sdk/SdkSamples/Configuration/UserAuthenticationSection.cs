// -----------------------------------------------------------------------
// <copyright file="UserAuthenticationSection.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Configuration
{
    using System;

    /// <summary>
    /// Holds a user authentication section settings.
    /// </summary>
    public class UserAuthenticationSection : Section
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserAuthenticationSection"/> class.
        /// </summary>
        /// <param name="sectionName">The application authentication section name.</param>
        public UserAuthenticationSection(string sectionName) : base(sectionName)
        {
        }

        /// <summary>
        /// Gets the AAD application ID.
        /// </summary>
        public string ApplicationId => this.ConfigurationSection["ApplicationId"];

        /// <summary>
        /// Gets the resource the application is attempting to access, i.e. the partner API service.
        /// </summary>
        public Uri ResourceUrl => new Uri(this.ConfigurationSection["ResourceUrl"]);

        /// <summary>
        /// Gets the application redirect URL.
        /// </summary>
        public Uri RedirectUrl => new Uri(this.ConfigurationSection["RedirectUrl"]);
    }
}
