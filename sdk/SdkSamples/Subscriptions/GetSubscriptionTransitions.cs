// -----------------------------------------------------------------------
// <copyright file="GetSubscriptionTransitions.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Subscriptions
{
    /// <summary>
    /// A scenario that retrieves a customer subscription.
    /// </summary>
    public class GetSubscriptionTransitions : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetSubscriptionTransitions"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetSubscriptionTransitions(IScenarioContext context) : base("Get customer subscription transitions", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;
            string customerId = this.ObtainCustomerId("Enter the ID of the customer whom to retrieve their Subscriptions");
            string subscriptionId = this.ObtainSubscriptionId(customerId, "Enter the ID of the subscription to find transtions for");
            var subscriptionOperations = partnerOperations.Customers.ById(customerId).Subscriptions.ById(subscriptionId);

            this.Context.ConsoleHelper.StartProgress("Retrieving customer subscription Transitions");
            var transitions = subscriptionOperations.Transitions.Get();
            this.Context.ConsoleHelper.StopProgress();

            this.Context.ConsoleHelper.WriteObject(transitions, "Customer Subscription Transitions");
        }
    }
}
