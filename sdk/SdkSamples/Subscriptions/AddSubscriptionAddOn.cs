// -----------------------------------------------------------------------
// <copyright file="AddSubscriptionAddOn.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Subscriptions
{
    using System.Collections.Generic;
    using Store.PartnerCenter.Models.Orders;

    /// <summary>
    /// A scenario that adds a new add on to an existing subscription.
    /// </summary>
    public class AddSubscriptionAddOn : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddSubscriptionAddOn"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public AddSubscriptionAddOn(IScenarioContext context) : base("Add subscription add on", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;

            // obtain the customer ID, the ID of the subscription to amend with the add on offer and the add on offer ID
            var customerId = this.ObtainCustomerId();
            var subscriptionId = this.ObtainSubscriptionId(customerId, "Enter the subscription ID for which to purchase an add on");
            string addOnOfferId = this.Context.ConsoleHelper.ReadNonEmptyString("Enter the ID of the add on offer to purchase", "Offer ID can't be empty");

            var subscriptionOperations = partnerOperations.Customers.ById(customerId).Subscriptions.ById(subscriptionId);

            // get the parent subscription details
            this.Context.ConsoleHelper.StartProgress("Retrieving order information for existing subscription");
            var parentSubscription = subscriptionOperations.Get();
            this.Context.ConsoleHelper.StopProgress();

            // in order to buy an add on subscription for this offer, we need to patch/update the order through which the base offer was purchased
            // by creating an order object with a single line item which represents the add-on offer purchase.
            var orderToUpdate = new Order()
            {
                ReferenceCustomerId = customerId,
                LineItems = new List<OrderLineItem>()
                {
                    new OrderLineItem()
                    {
                        LineItemNumber = 0,
                        OfferId = addOnOfferId,
                        FriendlyName = "Some friendly name",
                        Quantity = 2,
                        ParentSubscriptionId = subscriptionId
                    }
                }
            };

            // update the order to apply the add on purchase
            this.Context.ConsoleHelper.StartProgress("Updating parent subscription order");
            Order updatedOrder = partnerOperations.Customers.ById(customerId).Orders.ById(parentSubscription.OrderId).Patch(orderToUpdate);
            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(updatedOrder, "Updated order");

            // fetch the subscription add ons and display these
            this.Context.ConsoleHelper.StartProgress("Retrieving subscription supported add ons");
            var subscriptionAddOns = subscriptionOperations.AddOns.Get();
            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(subscriptionAddOns, "Subscription add ons");
        }
    }
}