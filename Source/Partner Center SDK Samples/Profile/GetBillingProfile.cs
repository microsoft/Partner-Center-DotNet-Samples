// -----------------------------------------------------------------------
// <copyright file="GetBillingProfile.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Profile
{
    /// <summary>
    /// A scenario that retrieves the partner's billing profile.
    /// </summary>
    public class GetBillingProfile : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetBillingProfile"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetBillingProfile(IScenarioContext context) : base("Get partner's billing profile", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;

            this.Context.ConsoleHelper.StartProgress("Retrieving partner's billing profile");

            var billingProfile = partnerOperations.Profiles.BillingProfile.Get();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(billingProfile, "Partner's billing profile");
        }
    }
}
