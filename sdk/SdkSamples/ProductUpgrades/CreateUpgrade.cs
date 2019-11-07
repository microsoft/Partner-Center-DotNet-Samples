// -----------------------------------------------------------------------
// <copyright file="CreateUpgrade.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.ProductUpgrades
{
    using Microsoft.Store.PartnerCenter.Models.ProductUpgrades;
    using System.Globalization;

    /// <summary>
    /// A scenario that upgrades a given product family
    /// </summary>
    public class CreateUpgrade : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateUpgrade"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public CreateUpgrade(IScenarioContext context) : base("Upgrade product", context)
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

            this.Context.ConsoleHelper.StartProgress(string.Format(CultureInfo.InvariantCulture, "Upgrading products for customer {0} for product family {1}", selectedCustomerId, selectedProductFamily));

            var locationHeader = partnerOperations.ProductUpgrades.Create(productUpgradeRequest);
            this.Context.ConsoleHelper.StopProgress();

            this.Context.ConsoleHelper.WriteObject(locationHeader, string.Format(CultureInfo.InvariantCulture, "Upgrade status location header for customer {0}", selectedCustomerId));
        }
    }
}
