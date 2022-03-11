// -----------------------------------------------------------------------
// <copyright file="CreateOrder.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Orders
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Store.PartnerCenter.Models.Orders;
    using Microsoft.Store.PartnerCenter.Models.Offers;

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
            var partnerOperations = this.Context.UserPartnerOperations;

            string customerId = this.ObtainCustomerId("Enter the ID of the customer making the purchase");
            string offerId = this.ObtainOfferId("Enter the ID of the offer to purchase");
            
            string termDuration = this.Context.ConsoleHelper.ReadNonEmptyString("Enter a term duration [example: P1Y, P1M]", "Term duration is required");
            
            string billingCycleString = this.Context.ConsoleHelper.ReadNonEmptyString("Enter a billing cycle [example: Annual or Monthly]", "Billing cycle is required");
            var billingCycle = (BillingCycleType)Enum.Parse(typeof(BillingCycleType), billingCycleString);
            
            string quantityString = this.Context.ConsoleHelper.ReadNonEmptyString("Enter a quantity", "Quantity is required");
            var quantity = int.Parse(quantity);
            
            string customTermEndDateString = this.Context.ConsoleHelper.ReadOptionalString("Enter a custom term end date or leave blank to keep default");
            DateTime? customTermEndDate = null;
            if (!string.IsNullOrWhiteSpace(customTermEndDate)) {
                customTermEndDate = DateTime.Parse(customTermEndDateString);
            }
            
            var order = new Order()
            {
                ReferenceCustomerId = customerId,
                BillingCycle = billingCycle,
                LineItems = new List<OrderLineItem>()
                {
                    new OrderLineItem()
                    {
                        OfferId = offerId,
                        FriendlyName = "new offer purchase",
                        Quantity = quantity,
                        TermDuration = termDuration,
                        CustomTermEndDate = customTermEndDate
                    }
                }
            };

            this.Context.ConsoleHelper.WriteObject(order, "Order to be placed");
            this.Context.ConsoleHelper.StartProgress("Placing order");

            var createdOrder = partnerOperations.Customers.ById(customerId).Orders.Create(order);

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(createdOrder, "Created order"); 
        }
    }
}
