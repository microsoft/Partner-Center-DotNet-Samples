// -----------------------------------------------------------------------
// <copyright file="CreateCart.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Carts
{
    using System.Collections.Generic;
    using System.Linq;
    using Models.Carts;

    /// <summary>
    /// A scenario that creates a new cart for a customer.
    /// </summary>
    public class CreateCart : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCart"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public CreateCart(IScenarioContext context) : base("Create a Cart", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;

            string customerId = this.ObtainCustomerId("Enter the ID of the customer making the purchase");
            string catalogItemId = this.ObtainCatalogItemId("Enter the catalog Item Id");
            string productId = catalogItemId.Split(':')[0];
            string skuId = catalogItemId.Split(':')[1];
            string scope = string.Empty;
            string subscriptionId = string.Empty;
            string duration = string.Empty;
            var sku = partnerOperations.Products.ByCountry("US").ById(productId).Skus.ById(skuId).Get();

            if (sku.ProvisioningVariables != null)
            {
                scope = this.ObtainScope("Enter the Scope for the Provisioning status");
                subscriptionId = this.ObtainAzureSubscriptionId("Enter the Subscription Id");
                duration = (string)sku.DynamicAttributes["duration"];
            }

            var cart = new Cart()
            {
                LineItems = new List<CartLineItem>()
                {
                    new CartLineItem()
                    {
                        CatalogItemId = catalogItemId,
                        FriendlyName = "Myofferpurchase",
                        Quantity = 1,
                        BillingCycle = sku.SupportedBillingCycles.ToArray().First(),
                        ProvisioningContext = (sku.ProvisioningVariables == null) ? null : new Dictionary<string, string>
                        {
                            { "subscriptionId", subscriptionId },
                            { "scope", scope },
                            { "duration", duration }
                        }
                    }
                }
            };

            this.Context.ConsoleHelper.WriteObject(cart, "Cart to be created");
            this.Context.ConsoleHelper.StartProgress("Creating cart");

            var createdCart = partnerOperations.Customers.ById(customerId).Carts.Create(cart);

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(createdCart, "Created cart");
        }
    }
}