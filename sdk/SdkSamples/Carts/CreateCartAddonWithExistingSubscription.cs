// -----------------------------------------------------------------------
// <copyright file="CreateCartAddonWithExistingSubscription.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Carts
{
    using System.Collections.Generic;
    using System.Linq;
    using Models.Carts;

    /// <summary>
    /// A scenario that creates a new cart with add on items for existing subscription for customer.
    /// </summary>
    public class CreateCartAddonWithExistingSubscription : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCartAddonWithExistingSubscription"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public CreateCartAddonWithExistingSubscription(IScenarioContext context) : base("Create a cart with addon items for existing subscription", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;

            string customerId = this.ObtainCustomerId("Enter the ID of the customer making the purchase");
            string existingSubscriptionId = this.ObtainSubscriptionId(customerId, "Enter existing subscription Id");
            string addonCatalogItemId = this.ObtainCatalogItemId("Enter the addon Item Id");
            var cart = new Cart()
            {
                LineItems = new List<CartLineItem>()
                {
                    new CartLineItem()
                    {
                        Id = 0,
                        CatalogItemId = addonCatalogItemId,
                        ProvisioningContext = new Dictionary<string, string>
                        {
                            {
                                "ParentSubscriptionId",
                                existingSubscriptionId
                            }
                        },
                        Quantity = 1,
                        BillingCycle = Models.Products.BillingCycleType.Monthly
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
