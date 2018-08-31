// -----------------------------------------------------------------------
// <copyright file="RemoveCustomerUserMemberFromDirectoryRole.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.CustomerDirectoryRoles
{
    using System;

    /// <summary>
    /// Showcases remove customer user from directory role service.
    /// </summary>
    public class RemoveCustomerUserMemberFromDirectoryRole : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveCustomerUserMemberFromDirectoryRole"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public RemoveCustomerUserMemberFromDirectoryRole(IScenarioContext context) : base("Remove customer user from a directory role", context)
        {
        }

        /// <summary>
        /// Executes the remove customer user from a directory role scenario.
        /// </summary>
        protected override void RunScenario()
        {
            // get user member Id.
            string selectedUserMemberId = this.ObtainUserMemberId("Enter the ID of the user member of directory role user to remove");

            // get directory role Id of the entered user member.
            string selectedDirectoryRoleId = this.ObtainDirectoryRoleId("Enter the ID of the directory role to remove customer user");

            // get customer Id of the entered directory roles.
            string selectedCustomerId = this.ObtainCustomerId("Enter the ID of the customer whose customer user to remove from a directory role");
            var partnerOperations = this.Context.UserPartnerOperations;
            
            this.Context.ConsoleHelper.StartProgress("Removing user member from directory role");

            // Remove user member from selected directory role.
            partnerOperations.Customers.ById(selectedCustomerId).DirectoryRoles.ById(selectedDirectoryRoleId).UserMembers.ById(selectedUserMemberId).Delete();
            this.Context.ConsoleHelper.StopProgress();
            Console.WriteLine("The user member with Id: {0} is removed from directory role Id: {1}", selectedUserMemberId, selectedDirectoryRoleId);
        }
    }
}
