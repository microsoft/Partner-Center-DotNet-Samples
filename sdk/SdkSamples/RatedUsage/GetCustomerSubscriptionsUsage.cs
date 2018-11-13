// -----------------------------------------------------------------------
// <copyright file="GetCustomerSubscriptionsUsage.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.RatedUsage
{
    /// <summary>
    /// A scenario that retrieves the usage records for all the subscriptions owned by a customer.
    /// </summary>
    public class GetCustomerSubscriptionsUsage : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCustomerSubscriptionsUsage"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetCustomerSubscriptionsUsage(IScenarioContext context) : base("Get customer subscriptions usage", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;

            string customerId = this.ObtainCustomerId("Enter the ID of the customer to retrieve his/her subscriptions usage");
            this.Context.ConsoleHelper.StartProgress("Retrieving customer subscriptions usage");

            var customerUsageRecords = partnerOperations.Customers.ById(customerId).Subscriptions.UsageRecords.Get();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(customerUsageRecords, "Customer subscriptions usage records");
        }
    }
}
