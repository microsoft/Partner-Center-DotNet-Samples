// -----------------------------------------------------------------------
// <copyright file="GetCustomerQualifications.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace Microsoft.Store.PartnerCenter.Samples.Customers
{
    /// <summary>
    /// Gets a customer's qualifications using the asynchronous qualifications APIs.
    /// </summary>
    public class GetCustomerQualifications : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCustomerQualifications"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetCustomerQualifications(IScenarioContext context) : base("Get customer qualifications", context)
        {
        }

        /// <summary>
        /// Executes the get customer qualifications scenario.
        /// </summary>
        protected override void RunScenario()
        {
            string customerIdToRetrieve = this.ObtainCustomerId("Enter the ID of the customer to retrieve qualifications for");

            var partnerOperations = this.Context.UserPartnerOperations;
            this.Context.ConsoleHelper.StartProgress("Retrieving customer qualifications");

            var customerQualifications = partnerOperations.Customers.ById(customerIdToRetrieve).Qualification.GetQualifications();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(customerQualifications, "Customer Qualifications");
        }
    }
}
