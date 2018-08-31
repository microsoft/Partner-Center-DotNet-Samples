// -----------------------------------------------------------------------
// <copyright file="GetCustomerQualification.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Customers
{
    /// <summary>
    /// Gets a single customer qualification.
    /// </summary>
    public class GetCustomerQualification : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCustomerQualification"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetCustomerQualification(IScenarioContext context) : base("Get customer qualification", context)
        {
        }

        /// <summary>
        /// Executes the get customer qualification scenario.
        /// </summary>
        protected override void RunScenario()
        {
            string customerIdToRetrieve = this.ObtainCustomerId("Enter the ID of the customer to retrieve qualification for");

            var partnerOperations = this.Context.UserPartnerOperations;
            this.Context.ConsoleHelper.StartProgress("Retrieving customer qualification");

            var customerQualification = partnerOperations.Customers.ById(customerIdToRetrieve).Qualification.Get();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(customerQualification, "Customer Qualification");
        }
    }
}
