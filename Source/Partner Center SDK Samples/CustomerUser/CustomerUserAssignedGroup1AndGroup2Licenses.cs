// -----------------------------------------------------------------------
// <copyright file="CustomerUserAssignedGroup1AndGroup2Licenses.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.CustomerUser
{
    using System.Collections.Generic;
    using Models.Licenses;

    /// <summary>
    /// Get customer user assigned group1 and group2 licenses
    /// </summary>
    public class CustomerUserAssignedGroup1AndGroup2Licenses : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerUserAssignedGroup1AndGroup2Licenses"/> class
        /// </summary>
        /// <param name="context">The scenario context</param>
        public CustomerUserAssignedGroup1AndGroup2Licenses(IScenarioContext context) : base("Get customer user assigned group1 and group2 licenses", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            // Get the customer user Id
            string selectedCustomerUserId = this.ObtainCustomerUserId("Enter the ID of the customer user to get assigned licenses");

            // Get the customer Id of the entered customer user
            string selectedCustomerId = this.ObtainCustomerId("Enter the ID of the customer");

            var partnerOperations = this.Context.UserPartnerOperations;
            this.Context.ConsoleHelper.StartProgress("Getting customer user assigned licenses");

            // Get the customer user assigned group1 and group2 licenses information
            //  Group1 – This group has all products whose license can be managed in the Azure Active Directory (AAD).
            //  Group2 – This group contains products that cant be managed in Azure Active Directory
            List<LicenseGroupId> groupIds = new List<LicenseGroupId>() { LicenseGroupId.Group1, LicenseGroupId.Group2 };
            var customerUserAssignedGroup1Group2Licenses = partnerOperations.Customers.ById(selectedCustomerId).Users.ById(selectedCustomerUserId).Licenses.Get(groupIds);
            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(customerUserAssignedGroup1Group2Licenses, "Customer User Assigned Group1 and Group2 Licenses");
        }
    }
}