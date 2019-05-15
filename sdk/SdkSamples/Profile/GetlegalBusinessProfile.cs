// -----------------------------------------------------------------------
// <copyright file="GetLegalBusinessProfile.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Profile
{
    /// <summary>
    /// A scenario that retrieves the partner's legal business profile.
    /// </summary>
    public class GetLegalBusinessProfile : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetLegalBusinessProfile"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetLegalBusinessProfile(IScenarioContext context) : base("Get partner legal business profile", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;

            this.Context.ConsoleHelper.StartProgress("Retrieving partner legal business profile");

            var billingProfile = partnerOperations.Profiles.LegalBusinessProfile.Get();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(billingProfile, "Partner legal business profile");
        }
    }
}
