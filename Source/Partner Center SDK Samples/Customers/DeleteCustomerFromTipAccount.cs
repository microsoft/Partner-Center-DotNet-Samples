// -----------------------------------------------------------------------
// <copyright file="DeleteCustomerFromTipAccount.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Customers
{
    using System;

    /// <summary>
    /// Deletes a customer from a testing in production account.
    /// </summary>
    public class DeleteCustomerFromTipAccount : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteCustomerFromTipAccount"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public DeleteCustomerFromTipAccount(IScenarioContext context) : base("Delete customer from TIP account", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            string customerIdToDelete = this.Context.Configuration.Scenario.CustomerIdToDelete;

            if (string.IsNullOrWhiteSpace(customerIdToDelete))
            {
                // prompt the user the enter the customer ID
                customerIdToDelete = this.Context.ConsoleHelper.ReadNonEmptyString("Enter the ID of the customer to delete", "The customer ID can't be empty");
            }
            else
            {
                Console.WriteLine("Found customer ID: {0} in configuration.", customerIdToDelete);
            }

            var partnerOperations = this.Context.AppPartnerOperations;

            this.Context.ConsoleHelper.StartProgress("Deleting customer");           
            partnerOperations.Customers.ById(customerIdToDelete).Delete();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.Success("Customer successfully deleted");
        }
    }
}
