// -----------------------------------------------------------------------
// <copyright file="CreateCustomerUser.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.CustomerUser
{
    using System;
    using Models.Users;

    /// <summary>
    /// Creates a new customer user.
    /// </summary>
    public class CreateCustomerUser : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCustomerUser"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public CreateCustomerUser(IScenarioContext context) : base("Create a new customer user", context)
        {
        }

        /// <summary>
        /// Executes the create customer user scenario.
        /// </summary>
        protected override void RunScenario()
        {
            // get customer Id.
            string selectedCustomerId = this.ObtainCustomerId("Enter the ID of the customer to create customer user");
            var partnerOperations = this.Context.UserPartnerOperations;

            this.Context.ConsoleHelper.StartProgress("Getting customer of selected customer Id");

            // get customer.
            var selectedCustomer = partnerOperations.Customers.ById(selectedCustomerId).Get();
            this.Context.ConsoleHelper.StopProgress();

            var customerUserToCreate = new CustomerUser()
            {
                PasswordProfile = new PasswordProfile() { ForceChangePassword = true, Password = "Password!1" },
                DisplayName = "Kate",
                FirstName = "Kate",
                LastName = "Nichols",
                UsageLocation = "US",
                UserPrincipalName = Guid.NewGuid().ToString("N") + "@" + selectedCustomer.CompanyProfile.Domain
            };
            this.Context.ConsoleHelper.WriteObject(customerUserToCreate, "New customer user Information");

            this.Context.ConsoleHelper.StartProgress("Creating customer user");

            // Create a customer user.
            var createdUser = partnerOperations.Customers.ById(selectedCustomerId).Users.Create(customerUserToCreate);

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.Success("Success!");
            this.Context.ConsoleHelper.WriteObject(createdUser, "Created User Information");
        }
    }
}
