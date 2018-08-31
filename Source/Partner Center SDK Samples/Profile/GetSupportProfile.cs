// -----------------------------------------------------------------------
// <copyright file="GetSupportProfile.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Profile
{
    /// <summary>
    /// A scenario that retrieves the partner's support profile.
    /// </summary>
    public class GetSupportProfile : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetSupportProfile"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetSupportProfile(IScenarioContext context) : base("Get partner support profile", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;

            this.Context.ConsoleHelper.StartProgress("Retrieving partner support profile");

            var billingProfile = partnerOperations.Profiles.SupportProfile.Get();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(billingProfile, "partner support profile");
        }
    }
}
