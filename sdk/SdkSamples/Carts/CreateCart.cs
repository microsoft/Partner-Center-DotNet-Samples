// -----------------------------------------------------------------------
// <copyright file="CreateCart.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Carts
{
    using System;
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
        /// Provisioning context object.
        /// </summary>
        private Dictionary<string, string> ProvisioningContext;

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;

            string customerId = this.ObtainCustomerId("Enter the ID of the customer making the purchase");
            string catalogItemId = this.ObtainCatalogItemId("Enter the catalog Item Id");
            string countryCode = this.Context.ConsoleHelper.ReadNonEmptyString("Enter the 2 digit country code of the availability", "The country code can't be empty");
            string productId = catalogItemId.Split(':')[0];
            string skuId = catalogItemId.Split(':')[1];
            string availabilityId = catalogItemId.Split(':')[2];
            string scope = string.Empty;
            string subscriptionId = string.Empty;
            string duration = string.Empty;
            string termDuration = string.Empty;
            var sku = partnerOperations.Products.ByCountry(countryCode).ById(productId).Skus.ById(skuId).Get();
            var availability = partnerOperations.Products.ByCountry(countryCode).ById(productId).Skus.ById(skuId).Availabilities.ById(availabilityId).Get();


            if (sku.ProvisioningVariables != null)
            {
                var provisioningContext = new Dictionary<string, string>();
                foreach (var provisioningVariable in sku.ProvisioningVariables)
                {
                    switch (provisioningVariable)
                    {
                        case "Scope":
                            scope = this.ObtainScope("Enter the Scope for the Provisioning status");
                            provisioningContext.Add("scope", scope);
                            break;
                        case "SubscriptionId":
                            subscriptionId = this.ObtainAzureSubscriptionId("Enter the Subscription Id");
                            provisioningContext.Add("subscriptionId", subscriptionId);
                            break;
                        case "Duration":
                            duration = (string)sku.DynamicAttributes["duration"];
                            //if availability Terms duration exists, this does not need to be added. Kept here for backwards compatability
                            provisioningContext.Add("duration", duration);
                            break;
                    }
                }
                ProvisioningContext = provisioningContext;
            }
            else
            {
                ProvisioningContext = null;
            }

            if (sku.IsTrial && availability.Terms.Any(r => r.RenewalOptions.Any(t => t.TermDuration != null)))
            {
                termDuration = this.ObtainTermDuration("Enter the term duration that you want to renew into (P1M or P1Y)");
            }

            var cart = new Cart()
            {
                LineItems = new List<CartLineItem>()
                {
                    new CartLineItem()
                    {
                        CatalogItemId = catalogItemId,
                        FriendlyName = "Myofferpurchase",
                        Quantity = termDuration == String.Empty ? 1 : 10,
                        TermDuration = availability.Terms.First().Duration ?? null ,
                        BillingCycle = sku.SupportedBillingCycles.ToArray().First(),
                        ProvisioningContext = ProvisioningContext,
                        RenewsTo = termDuration == String.Empty ? null : new RenewsTo()
                        {
                            TermDuration = termDuration
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