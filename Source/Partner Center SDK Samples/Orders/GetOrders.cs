// -----------------------------------------------------------------------
// <copyright file="GetOrders.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Orders
{
    /// <summary>
    /// A scenario that retrieves a customer orders.
    /// </summary>
    public class GetOrders : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetOrders"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetOrders(IScenarioContext context) : base("Get customer orders", context)
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
        }
    }
}
