// -----------------------------------------------------------------------
// <copyright file="GetCustomerLicensesDeploymentAnalytics.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Analytics
{
    using Models;
    using Models.Analytics;

    /// <summary>
    /// Gets a single customer's licenses deployment analytics.
    /// </summary>
    public class GetCustomerLicensesDeploymentAnalytics : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCustomerLicensesDeploymentAnalytics"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetCustomerLicensesDeploymentAnalytics(IScenarioContext context) : base("Get customer licenses deployment analytics", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            string customerIdToRetrieve = ObtainCustomerId("Enter the ID of the customer to retrieve");

            IAggregatePartner partnerOperations = Context.UserPartnerOperations;
            Context.ConsoleHelper.StartProgress("Retrieving customer licenses deployment analytics");

            ResourceCollection<CustomerLicensesDeploymentInsights> customerLicensesDeploymentAnalytics = partnerOperations.Customers.ById(customerIdToRetrieve).Analytics.Licenses.Deployment.Get();

            Context.ConsoleHelper.StopProgress();
            Context.ConsoleHelper.WriteObject(customerLicensesDeploymentAnalytics, "Customer licenses deployment analytics");
        }
    }
}