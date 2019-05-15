// -----------------------------------------------------------------------
// <copyright file="GetCustomerLicensesDeploymentAnalytics.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Analytics
{
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
            string customerIdToRetrieve = this.ObtainCustomerId("Enter the ID of the customer to retrieve");

            var partnerOperations = this.Context.UserPartnerOperations;
            this.Context.ConsoleHelper.StartProgress("Retrieving customer licenses deployment analytics");

            var customerLicensesDeploymentAnalytics = partnerOperations.Customers.ById(customerIdToRetrieve).Analytics.Licenses.Deployment.Get();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(customerLicensesDeploymentAnalytics, "Customer licenses deployment analytics");
        }
    }
}
