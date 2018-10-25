// -----------------------------------------------------------------------
// <copyright file="DeleteCustomerUser.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.CustomerUser
{
    /// <summary>
    /// Deletes a customer user.
    /// </summary>
    public class DeleteCustomerUser : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteCustomerUser"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public DeleteCustomerUser(IScenarioContext context) : base("Delete customer user", context)
        {
        }

        /// <summary>
        /// Executes the delete customer user scenario.
        /// </summary>
        protected override void RunScenario()
        {
            // get customer user to delete.
            string customerUserIdToDelete = this.ObtainCustomerUserIdDelete("Enter the ID of the customer user to delete");

            // get customer Id of the entered customer user.
            string selectedCustomerId = this.ObtainCustomerId("Enter the ID of the corresponding customer whose customer user to delete");

            var partnerOperations = this.Context.UserPartnerOperations;

            this.Context.ConsoleHelper.StartProgress("Deleting customer user");

            // delete customer user
            partnerOperations.Customers.ById(selectedCustomerId).Users.ById(customerUserIdToDelete).Delete();
            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.Success("Customer User successfully deleted");
        }
    }
}
