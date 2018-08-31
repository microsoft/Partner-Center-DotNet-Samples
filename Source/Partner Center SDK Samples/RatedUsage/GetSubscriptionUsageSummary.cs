// -----------------------------------------------------------------------
// <copyright file="GetSubscriptionUsageSummary.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.RatedUsage
{
    /// <summary>
    /// A scenario that retrieves the customer's subscription usage summary.
    /// </summary>
    public class GetSubscriptionUsageSummary : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetSubscriptionUsageSummary"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetSubscriptionUsageSummary(IScenarioContext context) : base("Get customer Subscription Usage Summary", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;

            string customerId = this.ObtainCustomerId("Enter the ID of the customer who owns the subscription to retrieve it's usage summary");
            string subscriptionID = this.ObtainSubscriptionId(customerId, "Enter the ID of the subscription to retrieve it's usage summary");

            this.Context.ConsoleHelper.StartProgress("Retrieving customer Subscription Usage Summary");

            var customerSubscription = partnerOperations.Customers.ById(customerId).Subscriptions.ById(subscriptionID).UsageSummary.Get();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(customerSubscription, "Customer Subscription Usage Summary");
        }
    }
}
