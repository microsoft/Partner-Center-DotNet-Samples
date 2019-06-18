// -----------------------------------------------------------------------
// <copyright file="PlaceOrderForCustomer.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Models.Orders;
    using Models.Relationships;

    /// <summary>
    /// A scenario that creates a new order for a customer with an indirect reseller.
    /// </summary>
    public class PlaceOrderForCustomer : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlaceOrderForCustomer"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public PlaceOrderForCustomer(IScenarioContext context) : base("Create an order for a customer of an indirect reseller", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            IAggregatePartner partnerOperations = this.Context.UserPartnerOperations;

            string customerId = this.ObtainCustomerId("Enter the ID of the customer making the purchase");
            string offerId = this.ObtainOfferId("Enter the ID of the offer to purchase");
            string indirectResellerId = this.ObtainIndirectResellerId("Enter the ID of the indirect reseller: ");

            this.Context.ConsoleHelper.StartProgress("Getting list of indirect resellers");
            Models.ResourceCollection<PartnerRelationship> indirectResellers = partnerOperations.Relationships.Get(PartnerRelationshipType.IsIndirectCloudSolutionProviderOf);
            this.Context.ConsoleHelper.StopProgress();

            PartnerRelationship selectedIndirectReseller = (indirectResellers != null && indirectResellers.Items.Any()) ?
                indirectResellers.Items.FirstOrDefault(reseller => reseller.Id.Equals(indirectResellerId, StringComparison.OrdinalIgnoreCase)) :
                null;

            Order order = new Order()
            {
                ReferenceCustomerId = customerId,
                LineItems = new List<OrderLineItem>()
                {
                    new OrderLineItem()
                    {
                        OfferId = offerId,
                        FriendlyName = "new offer purchase",
                        Quantity = 5,
                        PartnerIdOnRecord = selectedIndirectReseller != null ? selectedIndirectReseller.MpnId : null
                    }
                }
            };

            this.Context.ConsoleHelper.WriteObject(order, "Order to be placed");
            this.Context.ConsoleHelper.StartProgress("Placing order");

            Order createdOrder = partnerOperations.Customers.ById(customerId).Orders.Create(order);

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(createdOrder, "Created order");
        }
    }
}
