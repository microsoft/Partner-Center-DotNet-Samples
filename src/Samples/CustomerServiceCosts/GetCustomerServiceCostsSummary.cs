// -----------------------------------------------------------------------
// <copyright file="GetCustomerServiceCostsSummary.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.CustomerServiceCosts
{
    using Models.ServiceCosts;

    /// <summary>
    /// Gets Customer Service Costs Summary.
    /// </summary>
    public class GetCustomerServiceCostsSummary : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCustomerServiceCostsSummary"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetCustomerServiceCostsSummary(IScenarioContext context) : base("Get customer service costs summary", context)
        {
        }

        /// <summary>
        /// Executes the get Customer Service Costs scenario.
        /// </summary>
        protected override void RunScenario()
        {
            // get a customer Id.
            string selectedCustomerId = this.ObtainCustomerId("Enter the ID of the customer to get service costs summary");

            var partnerOperations = this.Context.UserPartnerOperations;

            this.Context.ConsoleHelper.StartProgress("Getting customer service costs summary");

            // get the customer's Service Costs Summary.
            var customerServiceCostsSummary = partnerOperations.Customers.ById(selectedCustomerId).ServiceCosts.ByBillingPeriod(ServiceCostsBillingPeriod.MostRecent).Summary.Get();
            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(customerServiceCostsSummary, "Customer Service Costs Summary");
        }
    }
}
