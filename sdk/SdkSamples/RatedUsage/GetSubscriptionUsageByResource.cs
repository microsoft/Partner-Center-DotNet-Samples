// -----------------------------------------------------------------------
// <copyright file="GetSubscriptionUsageByResource.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.RatedUsage
{
    /// <summary>
    /// A scenario that retrieves a single subscription's resource usage records aggregated by resource.
    /// </summary>
    public class GetSubscriptionUsageByResource : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetSubscriptionUsageByResource"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetSubscriptionUsageByResource(IScenarioContext context) : base("Get subscription resource usage by resource", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;

            string customerId = this.ObtainCustomerId("Enter the ID of the customer who owns the subscription");
            string subscriptionId = this.ObtainSubscriptionId(customerId, "Enter the subscription ID");

            this.Context.ConsoleHelper.StartProgress("Retrieving customer usage records");

            var usageRecords = partnerOperations.Customers.ById(customerId).Subscriptions.ById(subscriptionId).UsageRecords.ByResource.Get();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(usageRecords, "Subscription resource usage records by resource");
        }
    }
}
