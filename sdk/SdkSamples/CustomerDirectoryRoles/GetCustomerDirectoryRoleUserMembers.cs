// -----------------------------------------------------------------------
// <copyright file="GetCustomerDirectoryRoleUserMembers.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.CustomerDirectoryRoles
{
    /// <summary>
    /// Showcases get customer users by directory role service.
    /// </summary>
    public class GetCustomerDirectoryRoleUserMembers : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCustomerDirectoryRoleUserMembers"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetCustomerDirectoryRoleUserMembers(IScenarioContext context) : base("Get customer user by directory role", context)
        {
        }

        /// <summary>
        /// Executes the get customer users by directory role service scenario.
        /// </summary>
        protected override void RunScenario()
        {
            // get customer Id to get directory role user members.
            string selectedCustomerId = this.ObtainCustomerId("Enter the ID of the customer to get directory role user members");

            // get directory role Id.
            string selectedDirectoryRoleId = this.ObtainDirectoryRoleId("Enter the ID of the directory role");

            IAggregatePartner partnerOperations = this.Context.UserPartnerOperations;

            this.Context.ConsoleHelper.StartProgress("Getting user members by directory roles");

            // Get all user members having the selected directory role.
            Models.SeekBasedResourceCollection<Models.Roles.UserMember> userMembers = partnerOperations.Customers.ById(selectedCustomerId).DirectoryRoles.ById(selectedDirectoryRoleId).UserMembers.Get();
            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(userMembers, "User Members who are having the selected directory role");
        }
    }
}
