// -----------------------------------------------------------------------
// <copyright file="CheckEligibility.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.ProductUpgrades
{
    using Microsoft.Store.PartnerCenter.Models.ProductUpgrades;
    using System.Globalization;

    /// <summary>
    /// A scenario that checks a product family upgrade eligibility
    /// </summary>
    public class CheckEligibility : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckEligibility"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public CheckEligibility(IScenarioContext context) : base("Check product upgrade eligibility", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;
            string selectedCustomerId = this.Context.ConsoleHelper.ReadNonEmptyString("Enter the customer id", "The customer id can't be empty");
            string selectedProductFamily = this.Context.ConsoleHelper.ReadNonEmptyString("Enter the product family", "The product family can't be empty");
            var productUpgradeRequest = new ProductUpgradesRequest
            {
                CustomerId = selectedCustomerId,
                ProductFamily = selectedProductFamily
            };

            this.Context.ConsoleHelper.StartProgress(string.Format(CultureInfo.InvariantCulture, "Checking product upgrade eligibility for customer {0} for product family {1}", selectedCustomerId, selectedProductFamily));

            var eligibility = partnerOperations.ProductUpgrades.CheckEligibility(productUpgradeRequest);
            this.Context.ConsoleHelper.StopProgress();

            this.Context.ConsoleHelper.WriteObject(eligibility, string.Format(CultureInfo.InvariantCulture, "{0} upgrade eligibility for customer {1}", selectedProductFamily, selectedCustomerId));
        }
    }
}
