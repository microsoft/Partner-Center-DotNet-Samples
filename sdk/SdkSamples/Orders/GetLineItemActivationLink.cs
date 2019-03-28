// -----------------------------------------------------------------------
// <copyright file="GetLineItemActivationLink.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace Microsoft.Store.PartnerCenter.Samples.Orders
{
    /// <summary>
    /// A scenario that retrieves an activation link from a customer order.
    /// </summary>
    public class GetLineItemActivationLink : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetLineItemActivationLink"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetLineItemActivationLink(IScenarioContext context) : base("Get customer order activation link", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;

            string customerId = this.ObtainCustomerId("Enter the ID of the customer whom to retrieve their orders");

            this.Context.ConsoleHelper.StartProgress("Retrieving customer orders");
            var customerOrders = partnerOperations.Customers.ById(customerId).Orders.Get();
            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(customerOrders, "Customer orders");

            string orderId = this.ObtainOrderID();
            this.Context.ConsoleHelper.StartProgress("Retrieving customer order");
            var customerOrder = partnerOperations.Customers.ById(customerId).Orders.ById(orderId).Get();
            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(customerOrder, "Customer order");

            Console.Write("Enter order line item number: ");
            string orderLineItemNumber = Console.ReadLine();
            this.Context.ConsoleHelper.StartProgress("Obtaining order line item activation link");
            var activationLink = partnerOperations.Customers.ById(customerId).Orders.ById(orderId).OrderLineItems.ById(orderLineItemNumber).ActivationLink.Get();
            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(activationLink, "activationLinks");
        }
    }
}
