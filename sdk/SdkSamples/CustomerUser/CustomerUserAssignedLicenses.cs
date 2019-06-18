// -----------------------------------------------------------------------
// <copyright file="CustomerUserAssignedLicenses.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.CustomerUser
{
    /// <summary>
    /// Gets customer user assigned licenses.
    /// </summary>
    public class CustomerUserAssignedLicenses : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerUserAssignedLicenses"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public CustomerUserAssignedLicenses(IScenarioContext context) : base("Get customer user assigned licenses", context)
        {
        }

        /// <summary>
        /// Executes the get customer user assigned licenses scenario.
        /// </summary>
        protected override void RunScenario()
        {
            // get customer user Id.
            string selectedCustomerUserId = this.ObtainCustomerUserId("Enter the ID of the customer user to get assigned licenses");

            // get customer Id of the entered customer user.
            string selectedCustomerId = this.ObtainCustomerId("Enter the ID of the customer");

            IAggregatePartner partnerOperations = this.Context.UserPartnerOperations;

            this.Context.ConsoleHelper.StartProgress("Getting customer user assigned licenses");

            // get customer user assigned licenses information.
            Models.ResourceCollection<Models.Licenses.License> customerUserAssignedLicenses = partnerOperations.Customers.ById(selectedCustomerId).Users.ById(selectedCustomerUserId).Licenses.Get();
            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(customerUserAssignedLicenses, "Customer User Assigned Licenses");
        }
    }
}
