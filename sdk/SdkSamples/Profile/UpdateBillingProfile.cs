// -----------------------------------------------------------------------
// <copyright file="UpdateBillingProfile.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Profile
{
    using System;
    using System.Globalization;

    /// <summary>
    /// A scenario that updates the partner's billing profile.
    /// </summary>
    public class UpdateBillingProfile : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateBillingProfile"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public UpdateBillingProfile(IScenarioContext context) : base("Update partner billing profile", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;

            this.Context.ConsoleHelper.StartProgress("Retrieving partner billing profile");

            var billingProfile = partnerOperations.Profiles.BillingProfile.Get();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(billingProfile);
            
            billingProfile.PurchaseOrderNumber = new Random().Next(9000, 10000).ToString(CultureInfo.InvariantCulture);

            this.Context.ConsoleHelper.StartProgress("Updating partner billing profile");
            var updatedPartnerBillingProfile = partnerOperations.Profiles.BillingProfile.Update(billingProfile);

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(updatedPartnerBillingProfile, "Updated partner billing profile");
        }
    }
}
