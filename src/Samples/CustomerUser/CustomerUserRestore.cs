// -----------------------------------------------------------------------
// <copyright file="CustomerUserRestore.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.CustomerUser
{
    using Models.Users;

    /// <summary>
    /// Showcases customer user restore API.
    /// </summary>
    public class CustomerUserRestore : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerUserRestore"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public CustomerUserRestore(IScenarioContext context) : base("Restore a deleted customer user", context)
        {
        }

        /// <summary>
        /// Executes the restore customer user scenario.
        /// </summary>
        protected override void RunScenario()
        {
            // get customer Id of the entered customer user.
            string selectedCustomerId = this.ObtainCustomerId("Enter the ID of the corresponding customer to restore customer user details");

            // get customer user Id.
            string selectedCustomerUserId = this.ObtainCustomerUserId("Enter the ID of the customer user to restore");

            var partnerOperations = this.Context.UserPartnerOperations;

            var updatedCustomerUser = new CustomerUser()
            {
                State = UserState.Active
            };

            this.Context.ConsoleHelper.StartProgress("Restoring the customer user");

            // restore customer user information using older upn.
            var restoredCustomerUserInfo = partnerOperations.Customers.ById(selectedCustomerId).Users.ById(selectedCustomerUserId).Patch(updatedCustomerUser);
            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(restoredCustomerUserInfo, "Restored customer user.");
        }
    }
}
