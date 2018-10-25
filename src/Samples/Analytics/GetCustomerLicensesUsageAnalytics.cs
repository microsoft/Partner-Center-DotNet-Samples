// -----------------------------------------------------------------------
// <copyright file="GetCustomerLicensesUsageAnalytics.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Analytics
{
    using Models;
    using Models.Analytics;

    /// <summary>
    /// Gets a single customer's licenses usage analytics.
    /// </summary>
    public class GetCustomerLicensesUsageAnalytics : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCustomerLicensesUsageAnalytics"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetCustomerLicensesUsageAnalytics(IScenarioContext context) : base("Get customer licenses usage analytics", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            string customerIdToRetrieve = ObtainCustomerId("Enter the ID of the customer to retrieve");

            IAggregatePartner partnerOperations = Context.UserPartnerOperations;
            Context.ConsoleHelper.StartProgress("Retrieving customer licenses usage analytics");

            ResourceCollection<CustomerLicensesUsageInsights> customerLicensesDeploymentAnalytics = partnerOperations.Customers.ById(customerIdToRetrieve).Analytics.Licenses.Usage.Get();

            Context.ConsoleHelper.StopProgress();
            Context.ConsoleHelper.WriteObject(customerLicensesDeploymentAnalytics, "Customer licenses usage analytics");
        }
    }
}