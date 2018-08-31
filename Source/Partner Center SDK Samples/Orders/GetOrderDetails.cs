// -----------------------------------------------------------------------
// <copyright file="GetOrderDetails.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Orders
{
    /// <summary>
    /// A scenario that retrieves a customer order details.
    /// </summary>
    public class GetOrderDetails : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetOrderDetails"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetOrderDetails(IScenarioContext context) : base("Get customer order details", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;

            string customerId = this.ObtainCustomerId("Enter the ID of the customer whom to retrieve their order details");
            string orderId = this.ObtainOrderID("Enter the ID of order to retrieve");

            this.Context.ConsoleHelper.StartProgress("Retrieving customer order details");

            var customerOrderDetails = partnerOperations.Customers.ById(customerId).Orders.ById(orderId).Get();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(customerOrderDetails, "Customer order details");
        }
    }
}
