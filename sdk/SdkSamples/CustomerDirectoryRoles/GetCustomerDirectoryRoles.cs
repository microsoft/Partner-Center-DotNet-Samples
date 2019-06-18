// -----------------------------------------------------------------------
// <copyright file="GetCustomerDirectoryRoles.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.CustomerDirectoryRoles
{
    /// <summary>
    /// Gets customer directory roles details.
    /// </summary>
    public class GetCustomerDirectoryRoles : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCustomerDirectoryRoles"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetCustomerDirectoryRoles(IScenarioContext context) : base("Get customer directory roles", context)
        {
        }

        /// <summary>
        /// Executes the get customer directory roles scenario.
        /// </summary>
        protected override void RunScenario()
        {
            // get customer Id.
            string selectedCustomerId = this.ObtainCustomerId("Enter the ID of the customer to get directory roles");

            IAggregatePartner partnerOperations = this.Context.UserPartnerOperations;

            this.Context.ConsoleHelper.StartProgress("Getting customer directory roles");

            // get directory roles of customer.
            Models.ResourceCollection<Models.Roles.DirectoryRole> directoryRoles = partnerOperations.Customers.ById(selectedCustomerId).DirectoryRoles.Get();
            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(directoryRoles, "Customer Directory Role Details");
        }
    }
}
