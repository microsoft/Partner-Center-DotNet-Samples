// -----------------------------------------------------------------------
// <copyright file="CreateOrder.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Orders
{
    using System.Collections.Generic;
    using Store.PartnerCenter.Models.Orders;

    /// <summary>
    /// A scenario that creates a new order for a customer.
    /// </summary>
    public class CreateOrder : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrder"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public CreateOrder(IScenarioContext context) : base("Create an order", context)
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

            Order order = new Order()
            {
                ReferenceCustomerId = customerId,
                LineItems = new List<OrderLineItem>()
                {
                    new OrderLineItem()
                    {
                        OfferId = offerId,
                        FriendlyName = "new offer purchase",
                        Quantity = 5
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
