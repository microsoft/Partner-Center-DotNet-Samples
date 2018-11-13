// -----------------------------------------------------------------------
// <copyright file="GetMPNProfile.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Profile
{
    /// <summary>
    /// A scenario that retrieves the partner's MPN profile.
    /// </summary>
    public class GetMPNProfile : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetMPNProfile"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetMPNProfile(IScenarioContext context) : base("Get partner MPN profile", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;

            this.Context.ConsoleHelper.StartProgress("Retrieving partner MPN profile");

            var billingProfile = partnerOperations.Profiles.MpnProfile.Get();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(billingProfile, "Partner MPN profile");
        }
    }
}
