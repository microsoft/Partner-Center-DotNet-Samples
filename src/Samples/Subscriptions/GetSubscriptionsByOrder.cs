// -----------------------------------------------------------------------
// <copyright file="GetSubscriptionsByOrder.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Subscriptions
{
    /// <summary>
    /// A scenario that retrieves a customer subscriptions by order.
    /// </summary>
    public class GetSubscriptionsByOrder : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetSubscriptionsByOrder"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetSubscriptionsByOrder(IScenarioContext context) : base("Get customer subscriptions by order", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;

            string customerId = this.ObtainCustomerId("Enter the ID of the customer whom to retrieve their subscriptions by order");
            string orderID = this.ObtainOrderID("Enter the Order ID to retrieve");

            this.Context.ConsoleHelper.StartProgress("Retrieving customer subscriptions by order");

            var customerSubscriptionsByOrder = partnerOperations.Customers.ById(customerId).Subscriptions.ByOrder(orderID).Get();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(customerSubscriptionsByOrder, "Customer Subscriptions By Order");
        }
    }
}
