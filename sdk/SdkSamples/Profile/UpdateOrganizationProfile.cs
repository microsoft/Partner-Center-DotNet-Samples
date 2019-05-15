// -----------------------------------------------------------------------
// <copyright file="UpdateOrganizationProfile.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Profile
{
    using System;
    using System.Globalization;

    /// <summary>
    /// A scenario that updates the partner's organization profile.
    /// </summary>
    public class UpdateOrganizationProfile : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateOrganizationProfile"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public UpdateOrganizationProfile(IScenarioContext context) : base("Update partner organization profile", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;

            this.Context.ConsoleHelper.StartProgress("Displaying partner organization profile");

            var organizationProfile = partnerOperations.Profiles.OrganizationProfile.Get();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(organizationProfile);

            // Generating a random phone number to update in the organization profile
            organizationProfile.DefaultAddress.PhoneNumber = ((long)(new Random().NextDouble() * 9000000000) + 1000000000).ToString(CultureInfo.InvariantCulture);

            this.Context.ConsoleHelper.StartProgress("Updating partner organization profile");
            var updatedPartnerOrganizationProfile = partnerOperations.Profiles.OrganizationProfile.Update(organizationProfile);

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(updatedPartnerOrganizationProfile, "Updated partner organization profile");
        }
    }
}
