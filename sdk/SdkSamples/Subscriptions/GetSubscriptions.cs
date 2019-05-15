// -----------------------------------------------------------------------
// <copyright file="GetSubscriptions.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Subscriptions
{
    /// <summary>
    /// A scenario that retrieves a customer subscription.
    /// </summary>
    public class GetSubscriptions : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetSubscriptions"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetSubscriptions(IScenarioContext context) : base("Get customer Subscriptions", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;

            string customerId = this.ObtainCustomerId("Enter the ID of the customer whom to retrieve their Subscriptions");
        
            this.Context.ConsoleHelper.StartProgress("Retrieving customer Subscriptions");

            var customerSubscription = partnerOperations.Customers.ById(customerId).Subscriptions.Get();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(customerSubscription, "Customer Subscriptions");
        }
    }
}
