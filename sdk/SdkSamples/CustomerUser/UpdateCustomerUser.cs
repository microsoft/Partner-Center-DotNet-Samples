// -----------------------------------------------------------------------
// <copyright file="UpdateCustomerUser.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.CustomerUser
{
    using System;
    using Models.Users;

    /// <summary>
    /// A scenario that updates a customer user.
    /// </summary>
    public class UpdateCustomerUser : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCustomerUser"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public UpdateCustomerUser(IScenarioContext context) : base("Update customer user", context)
        {
        }

        /// <summary>
        /// Executes the update customer user scenario.
        /// </summary>
        protected override void RunScenario()
        {
            // get customer user Id.
            string selectedCustomerUserId = this.ObtainCustomerUserId("Enter the ID of the customer user to update details");

            // get customer Id of the entered customer user.
            string selectedCustomerId = this.ObtainCustomerId("Enter the ID of the corresponding customer to update customer user");
            IAggregatePartner partnerOperations = this.Context.UserPartnerOperations;

            this.Context.ConsoleHelper.StartProgress("Getting customer user");

            // get customer user.
            CustomerUser selectedCustomerUser = partnerOperations.Customers.ById(selectedCustomerId).Users.ById(selectedCustomerUserId).Get();
            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(selectedCustomerUser, "Selected customer User");

            this.Context.ConsoleHelper.StartProgress("Getting customer");

            // get customer.
            Models.Customers.Customer selectedCustomer = partnerOperations.Customers.ById(selectedCustomerId).Get();
            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(selectedCustomer, "Selected Customer");

            CustomerUser updatedCustomerUser = new CustomerUser()
            {
                PasswordProfile = new PasswordProfile() { ForceChangePassword = true, Password = "Password!1" },
                DisplayName = "Shubham Bharti",
                FirstName = "Shubham",
                LastName = "Bharti",
                UsageLocation = "US",
                UserPrincipalName = Guid.NewGuid().ToString("N") + "@" + selectedCustomer.CompanyProfile.Domain
            };

            this.Context.ConsoleHelper.StartProgress("Updating the customer user");

            // update customer user information
            CustomerUser updatedCustomerUserInfo = partnerOperations.Customers.ById(selectedCustomerId).Users.ById(selectedCustomerUserId).Patch(updatedCustomerUser);
            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(updatedCustomerUserInfo, "Updated customer user information");
        }
    }
}
