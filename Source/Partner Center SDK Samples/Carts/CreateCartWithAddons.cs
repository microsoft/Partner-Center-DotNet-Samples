// -----------------------------------------------------------------------
// <copyright file="CreateCartWithAddons.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Carts
{
    using System.Collections.Generic;
    using System.Linq;
    using Models.Carts;

    /// <summary>
    /// A scenario that creates a new cart with add on items for a customer.
    /// </summary>
    public class CreateCartWithAddons : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCartWithAddons"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public CreateCartWithAddons(IScenarioContext context) : base("Create a Cart with addon items", context)
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
            string addonCatalogItemId = this.ObtainCatalogItemId("Enter the addon Item Id");
            var cart = new Cart()
            {
                LineItems = new List<CartLineItem>()
                {
                    new CartLineItem()
                    {
                        Id = 0,
                        CatalogItemId = catalogItemId,
                        FriendlyName = "Myofferpurchase",
                        Quantity = 3,
                        BillingCycle = Models.Products.BillingCycleType.Monthly,
                        AddonItems = new List<CartLineItem>
                        {
                            new CartLineItem
                            {
                                Id = 1,
                                CatalogItemId = addonCatalogItemId,
                                BillingCycle = Models.Products.BillingCycleType.Monthly,
                                Quantity = 2,
                            }
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
