// -----------------------------------------------------------------------
// <copyright file="CheckStatus.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.ProductUpgrades
{
    using Microsoft.Store.PartnerCenter.Models.ProductUpgrades;
    using System.Globalization;

    /// <summary>
    /// A scenario that checks a product family upgrade status
    /// </summary>
    public class CheckStatus : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckStatus"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public CheckStatus(IScenarioContext context) : base("Check product upgrade status", context)
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
            string productUpgradeId = this.Context.ConsoleHelper.ReadNonEmptyString("Enter the product upgrade id", "The product upgradeId can't be empty");
            var productUpgradeRequest = new ProductUpgradesRequest
            {
                CustomerId = selectedCustomerId,
                ProductFamily = selectedProductFamily
            };

            this.Context.ConsoleHelper.StartProgress(string.Format(CultureInfo.InvariantCulture, "Checking product upgrade status for customer {0} for product family {1}", selectedCustomerId, selectedProductFamily));

            var status = partnerOperations.ProductUpgrades.ById(productUpgradeId).CheckStatus(productUpgradeRequest);
            this.Context.ConsoleHelper.StopProgress();

            this.Context.ConsoleHelper.WriteObject(status, string.Format(CultureInfo.InvariantCulture, "{0} upgrade status for customer {1}", selectedProductFamily, selectedCustomerId));
        }
    }
}
