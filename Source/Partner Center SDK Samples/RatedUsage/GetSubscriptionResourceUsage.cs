// -----------------------------------------------------------------------
// <copyright file="GetSubscriptionResourceUsage.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.RatedUsage
{
    /// <summary>
    /// A scenario that retrieves a single subscription's resource usage records.
    /// </summary>
    public class GetSubscriptionResourceUsage : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetSubscriptionResourceUsage"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetSubscriptionResourceUsage(IScenarioContext context) : base("Get subscription resource usage", context)
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

            this.Context.ConsoleHelper.StartProgress("Retrieving customer orders");

            var usageRecords = partnerOperations.Customers.ById(customerId).Subscriptions.ById(subscriptionId).UsageRecords.Resources.Get();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(usageRecords, "Subscription resource usage records");
        }
    }
}
