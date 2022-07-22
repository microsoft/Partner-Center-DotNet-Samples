// -----------------------------------------------------------------------
// <copyright file="DeletePartnerCustomerDap.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Customers
{
    using Models.Customers;

    /// <summary>
    /// Deletes a partner customer Dap.
    /// </summary>
    public class DeletePartnerCustomerDap : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeletePartnerCustomerRelationship"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public DeletePartnerCustomerDap(IScenarioContext context) : base("Delete Partner Customer Dap", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;

            // prompt the user the enter the customer ID
            var customerIdToDeleteRelationshipOf = this.Context.ConsoleHelper.ReadNonEmptyString("Please enter the ID of the customer you want to delete Dap with", "The customer ID can't be empty");

            // Delete the partner customer dap
            this.Context.ConsoleHelper.StartProgress("Deleting partner customer Dap");

            Customer customer = new Customer
            {
                AllowDelegatedAccess = false
            };

            partnerOperations.Customers.ById(customerIdToDeleteRelationshipOf).Patch(customer);

            this.Context.ConsoleHelper.Success("Partner customer Dap successfully deleted");

            this.Context.ConsoleHelper.StopProgress();
        }
    }
}