// -----------------------------------------------------------------------
// <copyright file="CustomerUserAssignGroup1Licenses.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.CustomerUser
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Models.Licenses;

    /// <summary>
    /// Assign customer user a group1 license.
    /// </summary>
    public class CustomerUserAssignGroup1Licenses : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerUserAssignGroup1Licenses"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public CustomerUserAssignGroup1Licenses(IScenarioContext context) : base("Assign customer user a group1 license", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            // Get customer Id of the entered customer user.
            string selectedCustomerId = this.ObtainCustomerId("Enter the ID of the customer");

            // Get customer user Id.
            string selectedCustomerUserId = this.ObtainCustomerUserId("Enter the ID of the customer user to assign license");

            var partnerOperations = this.Context.UserPartnerOperations;

            this.Context.ConsoleHelper.StartProgress("Getting Subscribed Skus");

            // A list of the groupids
            // Group1 – This group has all products whose license can be managed in the Azure Active Directory (AAD).
            List<LicenseGroupId> groupIds = new List<LicenseGroupId>() { LicenseGroupId.Group1 };

            // Get customer's group1 subscribed skus information.
            var customerGroup1SubscribedSkus = partnerOperations.Customers.ById(selectedCustomerId).SubscribedSkus.Get(groupIds);
            this.Context.ConsoleHelper.StopProgress();

            // Prepare license request.
            LicenseUpdate updateLicense = new LicenseUpdate();
            LicenseAssignment license = new LicenseAssignment();

            // Select the first subscribed sku.
            SubscribedSku sku = customerGroup1SubscribedSkus.Items.First();
        
            // Assigning first subscribed sku as the license
            license.SkuId = sku.ProductSku.Id;
            license.ExcludedPlans = null;

            List<LicenseAssignment> licenseList = new List<LicenseAssignment>();
            licenseList.Add(license);
            updateLicense.LicensesToAssign = licenseList;

            this.Context.ConsoleHelper.StartProgress("Assigning License");

            // Assign licenses to the user.
            var assignLicense = partnerOperations.Customers.ById(selectedCustomerId).Users.ById(selectedCustomerUserId).LicenseUpdates.Create(updateLicense);
            this.Context.ConsoleHelper.StopProgress();

            this.Context.ConsoleHelper.StartProgress("Getting Assigned License");

            // Get customer user assigned licenses information after assigning the license.
            var customerUserAssignedLicenses = partnerOperations.Customers.ById(selectedCustomerId).Users.ById(selectedCustomerUserId).Licenses.Get(groupIds);
            this.Context.ConsoleHelper.StopProgress();

            License userLicense = customerUserAssignedLicenses.Items.First(licenseItem => licenseItem.ProductSku.Id == license.SkuId);
            Console.WriteLine("License was successfully assigned to the user.");
            this.Context.ConsoleHelper.WriteObject(userLicense, "Assigned License");
        }
    }
}
