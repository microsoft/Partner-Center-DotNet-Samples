// -----------------------------------------------------------------------
// <copyright file="GetSubscriptionUsageRecords.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.RatedUsage
{
    /// <summary>
    /// A scenario that retrieves a customer's subscriptions usage records.
    /// </summary>
    public class GetSubscriptionUsageRecords : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetSubscriptionUsageRecords"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetSubscriptionUsageRecords(IScenarioContext context) : base("Get customer subscriptions usage records", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;

            string customerId = this.ObtainCustomerId("Enter the ID of the customer whom to retrieve their subscriptions usage records");

            this.Context.ConsoleHelper.StartProgress("Retrieving customer subscriptions usage records");

            var customerSubscription = partnerOperations.Customers.ById(customerId).Subscriptions.UsageRecords.Get();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(customerSubscription, "Customer subscriptions usage records");
        }
    }
}
