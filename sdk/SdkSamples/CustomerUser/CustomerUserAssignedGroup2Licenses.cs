// -------------------------------------------------------------------------------------
// <copyright file="CustomerUserAssignedGroup2Licenses.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.CustomerUser
{
    using System.Collections.Generic;
    using Models.Licenses;

    /// <summary>
    /// Get customer user assigned group2 licenses
    /// </summary>
    public class CustomerUserAssignedGroup2Licenses : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerUserAssignedGroup2Licenses"/> class
        /// </summary>
        /// <param name="context">The scenario context</param>
        public CustomerUserAssignedGroup2Licenses(IScenarioContext context) : base("Get customer user assinged group2 licenses", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            // Get the customer Id of the entered customer user
            string selectedCustomerId = this.ObtainCustomerId("Enter the ID of the customer");

            // Get the customer user Id
            string selectedCustomerUserId = this.ObtainCustomerUserId("Enter the ID of the customer user to get assigned licenses");

            IAggregatePartner partnerOperations = this.Context.UserPartnerOperations;
            this.Context.ConsoleHelper.StartProgress("Getting customer user assigned licenses");

            // Get the customer user assigned group2 licenses information 
            // Group2 – This group contains products that cant be managed in Azure Active Directory
            List<LicenseGroupId> groupIds = new List<LicenseGroupId>() { LicenseGroupId.Group2 };
            Models.ResourceCollection<License> customerUserAssignedGroup2Licenses = partnerOperations.Customers.ById(selectedCustomerId).Users.ById(selectedCustomerUserId).Licenses.Get(groupIds);
            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(customerUserAssignedGroup2Licenses, "Customer User Assigned Group2 Licenses");
        }
    }
}