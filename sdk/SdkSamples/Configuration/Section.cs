// -----------------------------------------------------------------------
// <copyright file="Section.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Configuration
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Globalization;

    /// <summary>
    /// Encapsulates a configuration section read from app.config.
    /// </summary>
    public abstract class Section
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Section"/> class.
        /// </summary>
        /// <param name="sectionName">The configuration section name.</param>
        protected Section(string sectionName)
        {
            if (string.IsNullOrWhiteSpace(sectionName))
            {
                throw new ArgumentException("sectionName must be set");
            }

            this.ConfigurationSection = System.Configuration.ConfigurationManager.GetSection(sectionName) as NameValueCollection;

            if (this.ConfigurationSection == null)
            {
                throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "Could not read section: {0} from configuration", sectionName));
            }
        }

        /// <summary>
        /// Gets the configuration section.
        /// </summary>
        protected NameValueCollection ConfigurationSection { get; private set; }
    }
}
