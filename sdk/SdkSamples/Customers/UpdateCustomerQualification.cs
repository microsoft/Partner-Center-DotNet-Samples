// -----------------------------------------------------------------------
// <copyright file="UpdateCustomerQualification.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Customers
{
    using Microsoft.Store.PartnerCenter.Models.Customers;

    /// <summary>
    /// Updates a single customer qualification.
    /// </summary>
    public class UpdateCustomerQualification : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCustomerQualification"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public UpdateCustomerQualification(IScenarioContext context) : base("Update customer qualification", context)
        {
        }

        /// <summary>
        /// Executes the update customer qualification scenario.
        /// </summary>
        protected override void RunScenario()
        {
            string customerIdToRetrieve = this.ObtainCustomerId($"Enter the ID of the customer to update qualification to {CustomerQualification.Education}");

            IAggregatePartner partnerOperations = this.Context.UserPartnerOperations;
            this.Context.ConsoleHelper.StartProgress("Updating customer qualification");

            CustomerQualification customerQualification =
                partnerOperations.Customers.ById(customerIdToRetrieve)
                    .Qualification.Update(CustomerQualification.Education);

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(customerQualification, "Customer Qualification");
        }
    }
}
