// -----------------------------------------------------------------------
// <copyright file="CreateAzureReservationOrder.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Store.PartnerCenter.Models.Offers;
    using Store.PartnerCenter.Models.Orders;

    /// <summary>
    /// A scenario that creates a new Azure RI order for a customer.
    /// </summary>
    public class CreateAzureReservationOrder : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateAzureReservationOrder"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public CreateAzureReservationOrder(IScenarioContext context) : base("Create an Azure Reservation order", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;

            string customerId = this.ObtainCustomerId("Enter the ID of the customer making the purchase");
            string productId = this.ObtainProductId();
            string skuId = this.ObtainSkuId();
            string subscriptionId = this.ObtainAzureSubscriptionId();
            string countryCode = this.Context.ConsoleHelper.ReadNonEmptyString("Enter the 2 digit country code of the sku", "The country code can't be empty");

            var sku = partnerOperations.Products.ByCountry(countryCode).ById(productId).Skus.ById(skuId).Get();
            var availabilities = partnerOperations.Products.ByCountry(countryCode).ById(productId).Skus.ById(skuId).Availabilities.Get();

            if ((sku.DynamicAttributes == null) || string.IsNullOrEmpty(Convert.ToString(sku.DynamicAttributes["duration"])))
            {
                this.Context.ConsoleHelper.Warning("Invalid Azure catalog item ID.");
            }
            else
            {
                if (!availabilities.Items.Any())
                {
                    this.Context.ConsoleHelper.Warning("No availabilities found.");
                }
                else
                {
                    var order = new Order()
                    {
                        ReferenceCustomerId = customerId,
                        BillingCycle = BillingCycleType.OneTime,
                        LineItems = new List<OrderLineItem>()
                        {
                            new OrderLineItem()
                            {
                                OfferId = availabilities.Items.First().CatalogItemId,
                                FriendlyName = "ASampleAzureRI",
                                Quantity = 1,
                                LineItemNumber = 0,
                                ProvisioningContext = new Dictionary<string, string>()
                                {
                                    { "subscriptionId", subscriptionId },
                                    { "scope", "shared" },
                                    { "duration", Convert.ToString(sku.DynamicAttributes["duration"]) }
                                }
                            }
                        }
                    };

                    this.Context.ConsoleHelper.WriteObject(order, "Azure Reservation order to be placed");
                    this.Context.ConsoleHelper.StartProgress("Placing order");

                    var createdOrder = partnerOperations.Customers.ById(customerId).Orders.Create(order);

                    this.Context.ConsoleHelper.StopProgress();
                    this.Context.ConsoleHelper.WriteObject(createdOrder, "Created Azure Reservation order");
                }
            }
        }
    }
}