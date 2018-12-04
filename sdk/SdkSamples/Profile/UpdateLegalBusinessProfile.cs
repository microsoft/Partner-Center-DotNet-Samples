// -----------------------------------------------------------------------
// <copyright file="UpdateLegalBusinessProfile.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Profile
{
    using System;
    using System.Globalization;

    /// <summary>
    /// A scenario that updates the partner's legal business profile.
    /// </summary>
    public class UpdateLegalBusinessProfile : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateLegalBusinessProfile"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public UpdateLegalBusinessProfile(IScenarioContext context) : base("Update partner legal business profile", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;

            this.Context.ConsoleHelper.StartProgress("Displaying partner legal business profile");

            var legalBusinessProfile = partnerOperations.Profiles.LegalBusinessProfile.Get();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(legalBusinessProfile);

            // Generating a random phone number to update in the legal busines profile
            legalBusinessProfile.CompanyApproverAddress.PhoneNumber = ((long)(new Random().NextDouble() * 9000000000) + 1000000000).ToString(CultureInfo.InvariantCulture);

            this.Context.ConsoleHelper.StartProgress("Updating partner legal business profile");
            var updatedLegalBusinessProfile = partnerOperations.Profiles.LegalBusinessProfile.Update(legalBusinessProfile);

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(updatedLegalBusinessProfile, "Updated partner legal business profile");
        }
    }
}
