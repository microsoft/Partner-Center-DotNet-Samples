// -----------------------------------------------------------------------
// <copyright file="AddUserMemberToDirectoryRole.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.CustomerDirectoryRoles
{
    using System;
    using Models.Roles;

    /// <summary>
    /// Adds user member to a directory role.
    /// </summary>
    public class AddUserMemberToDirectoryRole : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddUserMemberToDirectoryRole"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public AddUserMemberToDirectoryRole(IScenarioContext context) : base("Add user member to a directory role", context)
        {
        }

        /// <summary>
        /// Executes the add user member to a directory role scenario.
        /// </summary>
        protected override void RunScenario()
        {
            // get customer user Id.
            string selectedCustomerUserId = this.ObtainCustomerUserId("Enter the ID of the customer user to get details for creating user member");

            // get customer Id of the entered customer user.
            string selectedCustomerId = this.ObtainCustomerId("Enter the ID of the customer whose customer user details should be used for user member");

            // get directory role Id.
            string selectedDirectoryRoleId = this.ObtainDirectoryRoleId("Enter the ID of the directory role");

            var partnerOperations = this.Context.UserPartnerOperations;

            this.Context.ConsoleHelper.StartProgress("Getting Customer User Details");

            // getting customer user details
            var selectedCustomer = partnerOperations.Customers.ById(selectedCustomerId).Users.ById(selectedCustomerUserId).Get();
            this.Context.ConsoleHelper.StopProgress();
            UserMember userMemberToAdd = new UserMember()
            {
                UserPrincipalName = selectedCustomer.UserPrincipalName,
                DisplayName = selectedCustomer.DisplayName,
                Id = selectedCustomer.Id
            };
            this.Context.ConsoleHelper.StartProgress("Adding user member to directory roles");

            // Add this customer user to the selected directory role.
            var userMemberAdded = partnerOperations.Customers.ById(selectedCustomerId).DirectoryRoles.ById(selectedDirectoryRoleId).UserMembers.Create(userMemberToAdd);
            this.Context.ConsoleHelper.StopProgress();
            Console.WriteLine("Below Customer user was added to directory role with id: {0}", selectedDirectoryRoleId);
            this.Context.ConsoleHelper.WriteObject(userMemberAdded, "Added Customer User Member to Directory Role Details");
        }
    }
}
