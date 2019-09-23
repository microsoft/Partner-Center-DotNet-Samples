// -----------------------------------------------------------------------
// <copyright file="GetCustomerUsageSummary.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;

namespace Microsoft.Store.PartnerCenter.Samples.RatedUsage
{
    /// <summary>
    /// A scenario that retrieves all customers usage summaries for usage based services.
    /// </summary>
    public class GetCustomersUsageSummary : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCustomersUsageSummary"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetCustomersUsageSummary(IScenarioContext context) : base("Get all customers usage summaries", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;

            this.Context.ConsoleHelper.StartProgress("Retrieving all customers usage summaries");

            var customersUsageSummary = partnerOperations.Customers.UsageRecords.Get();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(customersUsageSummary, "Customer usage summaries");
        }
    }
}
