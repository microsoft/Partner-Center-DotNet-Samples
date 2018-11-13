// -----------------------------------------------------------------------
// <copyright file="VerifyPartnerMpnId.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.IndirectPartners
{
    using System;
    using System.Globalization;

    /// <summary>
    /// A scenario that verifies a partner MPN ID.
    /// </summary>
    public class VerifyPartnerMpnId : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VerifyPartnerMpnId"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public VerifyPartnerMpnId(IScenarioContext context) : base("Verify partner MPN ID", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;
            this.Context.ConsoleHelper.StartProgress("Getting the logged in partner's profile");

            var currentPartnerProfile = partnerOperations.Profiles.MpnProfile.Get();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(currentPartnerProfile, "Logged in partner profile");

            string partnerMpnId = this.ObtainMpnId("Enter the MPN ID to verify");

            this.Context.ConsoleHelper.StartProgress(string.Format(CultureInfo.InvariantCulture, "Getting the partner profile for MPN ID: {0}", partnerMpnId));

            var partnerProfile = partnerOperations.Profiles.MpnProfile.Get(partnerMpnId);

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(partnerProfile, "Partner profile");
        }
    }
}
