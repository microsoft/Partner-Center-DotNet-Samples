// -----------------------------------------------------------------------
// <copyright file="GetCustomerDetails.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Customers
{
    /// <summary>
    /// Gets a single customer details.
    /// </summary>
    public class GetCustomerDetails : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCustomerDetails"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetCustomerDetails(IScenarioContext context) : base("Get a customer details", context)
        {
        }

        /// <summary>
        /// Executes the get customer details scenario.
        /// </summary>
        protected override void RunScenario()
        {
            string customerIdToRetrieve = this.ObtainCustomerId("Enter the ID of the customer to retrieve");

            var partnerOperations = this.Context.UserPartnerOperations;
            this.Context.ConsoleHelper.StartProgress("Retrieving customer");
            
            var customerDetails = partnerOperations.Customers.ById(customerIdToRetrieve).Get();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(customerDetails, "Customer details");
        }
    }
}
