// -----------------------------------------------------------------------
// <copyright file="UpdateOrder.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Orders
{
    using System.Linq;

    /// <summary>
    /// A scenario that updates a customer order.
    /// </summary>
    public class UpdateOrder : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateOrder"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public UpdateOrder(IScenarioContext context) : base("Update customer order", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;

            string customerId = this.ObtainCustomerId("Enter the ID of the customer whom to retrieve their orders");
            string orderId = this.ObtainOrderID("Enter the ID of order to retrieve");

            this.Context.ConsoleHelper.StartProgress("Retrieving customer order to be updated");
            var customerOrder = partnerOperations.Customers.ById(customerId).Orders.ById(orderId).Get();
            this.Context.ConsoleHelper.StopProgress();

            // increase the quantity of first line item
            customerOrder.LineItems.ToArray()[0].Quantity++;

            this.Context.ConsoleHelper.StartProgress("Updating the customer order");
            var updatedOrder = partnerOperations.Customers.ById(customerId).Orders.ById(customerOrder.Id).Patch(customerOrder);            
            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(updatedOrder, "Updated customer order");
        }
    }
}
