// -----------------------------------------------------------------------
// <copyright file="GetSubscription.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Subscriptions
{
    /// <summary>
    /// A scenario that retrieves a customer subscription.
    /// </summary>
    public class GetSubscription : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetSubscription"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetSubscription(IScenarioContext context) : base("Get customer Subscription", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            IAggregatePartner partnerOperations = this.Context.UserPartnerOperations;

            string customerId = this.ObtainCustomerId("Enter the ID of the customer whom to retrieve their Subscription");
            string subscriptionID = this.ObtainSubscriptionId(customerId, "Enter the subscription ID to retrieve");

            this.Context.ConsoleHelper.StartProgress("Retrieving customer Subscription");

            Models.Subscriptions.Subscription customerSubscription = partnerOperations.Customers.ById(customerId).Subscriptions.ById(subscriptionID).Get();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(customerSubscription, "Customer Subscription");
        }
    }
}
