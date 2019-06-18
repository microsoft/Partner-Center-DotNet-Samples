// -----------------------------------------------------------------------
// <copyright file="GetCustomerUserDirectoryRoles.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.CustomerUser
{
    /// <summary>
    /// Gets customer user directory roles details.
    /// </summary>
    public class GetCustomerUserDirectoryRoles : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCustomerUserDirectoryRoles"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetCustomerUserDirectoryRoles(IScenarioContext context) : base("Get customer user directory roles", context)
        {
        }

        /// <summary>
        /// Executes the get customer user directory roles scenario.
        /// </summary>
        protected override void RunScenario()
        {
            // get customer user Id.
            string selectedCustomerUserId = this.ObtainCustomerUserId("Enter the ID of the customer user to get directory roles");

            // get customer Id of the entered customer user.
            string selectedCustomerId = this.ObtainCustomerId("Enter the ID of the corresponding customer to get directory roles of customer user");

            IAggregatePartner partnerOperations = this.Context.UserPartnerOperations;
            this.Context.ConsoleHelper.StartProgress("Getting selected customer user");

            // get customer user.
            Models.Users.CustomerUser selectedCustomerUser = partnerOperations.Customers.ById(selectedCustomerId).Users.ById(selectedCustomerUserId).Get();
            this.Context.ConsoleHelper.StopProgress();

            this.Context.ConsoleHelper.WriteObject(selectedCustomerUser, "Selected Customer User");
            this.Context.ConsoleHelper.StartProgress("Getting customer user directory roles");

            // get customer user directory roles.
            Models.ResourceCollection<Models.Roles.DirectoryRole> userMemberships = partnerOperations.Customers.ById(selectedCustomerId).Users.ById(selectedCustomerUserId).DirectoryRoles.Get();
            this.Context.ConsoleHelper.StopProgress();

            this.Context.ConsoleHelper.WriteObject(userMemberships, "Customer User directory roles");
        }
    }
}
