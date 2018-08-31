// -----------------------------------------------------------------------
// <copyright file="GetCustomerUsageSummary.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.RatedUsage
{
    /// <summary>
    /// A scenario that retrieves a single customer usage summary for usage based services.
    /// </summary>
    public class GetCustomerUsageSummary : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCustomerUsageSummary"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetCustomerUsageSummary(IScenarioContext context) : base("Get customer usage summary", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;

            string customerId = this.ObtainCustomerId("Enter the ID of the customer to retrieve his/her usage summary");
            this.Context.ConsoleHelper.StartProgress("Retrieving customer usage summary");

            var customerUsageSummary = partnerOperations.Customers.ById(customerId).UsageSummary.Get();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(customerUsageSummary, "Customer usage summary");
        }
    }
}
