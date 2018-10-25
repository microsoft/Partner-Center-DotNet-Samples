// -----------------------------------------------------------------------
// <copyright file="CheckInventory.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Products
{
    using System.Collections.Generic;
    using System.Globalization;
    using Models.Products;

    /// <summary>
    /// A scenario that retrieves inventory validation results for the provided country.
    /// </summary>
    public class CheckInventory : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckInventory"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public CheckInventory(IScenarioContext context) : base("Check inventory", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;
            var productId = this.ObtainProductId("Enter the ID of the product to check inventory for");
            var customerId = this.ObtainCustomerId("Enter a customer ID");
            var subscriptionId = this.ObtainSubscriptionId(customerId, "Enter a subscription ID");
            string countryCode = this.Context.ConsoleHelper.ReadNonEmptyString("Enter the 2 digit country code", "The country code can't be empty");
            
            var inventoryCheckRequest = new InventoryCheckRequest()
            {
                TargetItems = new InventoryItem[] { new InventoryItem { ProductId = productId } },
                InventoryContext = new Dictionary<string, string>()
                {
                    { "customerId", customerId },
                    { "azureSubscriptionId", subscriptionId }
                }
            };

            this.Context.ConsoleHelper.StartProgress(string.Format(CultureInfo.InvariantCulture, "Checking inventory for product {0} in country {1}", productId, countryCode));
            var inventoryResults = partnerOperations.Extensions.Product.ByCountry(countryCode).CheckInventory(inventoryCheckRequest);
            this.Context.ConsoleHelper.StopProgress();

            this.Context.ConsoleHelper.WriteObject(inventoryResults, string.Format(CultureInfo.InvariantCulture, "Inventory check for product {0}", productId));
        }
    }
}
