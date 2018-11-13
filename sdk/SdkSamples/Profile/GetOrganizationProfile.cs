// -----------------------------------------------------------------------
// <copyright file="GetOrganizationProfile.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Profile
{
    /// <summary>
    /// A scenario that retrieves the partner's organization profile.
    /// </summary>
    public class GetOrganizationProfile : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetOrganizationProfile"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetOrganizationProfile(IScenarioContext context) : base("Get partner organization profile", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;

            this.Context.ConsoleHelper.StartProgress("Retrieving partner organization profile");

            var billingProfile = partnerOperations.Profiles.OrganizationProfile.Get();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(billingProfile, "Partner organization profile");
        }
    }
}
