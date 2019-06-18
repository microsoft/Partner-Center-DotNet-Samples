// -----------------------------------------------------------------------
// <copyright file="CustomerUserAssignGroup2Licenses.cs" company="Microsoft">
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
    /// Assign customer user a Group2 license
    /// </summary>
    public class CustomerUserAssignGroup2Licenses : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerUserAssignGroup2Licenses"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public CustomerUserAssignGroup2Licenses(IScenarioContext context) : base("Assign customer user a Group2 license", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            // A sample License Group2 Id - Minecraft product id.
            string minecraftProductSkuId = "984df360-9a74-4647-8cf8-696749f6247a";

            // Subscribed Sku for minecraft;
            SubscribedSku minecraftSubscribedSku = null;

            // Get customer Id of the entered customer user.
            string selectedCustomerId = this.ObtainCustomerId("Enter the ID of the customer");

            // Get customer user Id.
            string selectedCustomerUserId = this.ObtainCustomerUserId("Enter the ID of the customer user to assign license");

            IAggregatePartner partnerOperations = this.Context.UserPartnerOperations;

            this.Context.ConsoleHelper.StartProgress("Getting Subscribed Skus");

            // Group2 – This group contains products that cant be managed in Azure Active Directory
            List<LicenseGroupId> groupIds = new List<LicenseGroupId>() { LicenseGroupId.Group2 };

            // Get customer's subscribed skus information.
            Models.ResourceCollection<SubscribedSku> customerSubscribedSkus = partnerOperations.Customers.ById(selectedCustomerId).SubscribedSkus.Get(groupIds);

            // Check if a minecraft exists  for a given user
            foreach (SubscribedSku customerSubscribedSku in customerSubscribedSkus.Items)
            {
                if (customerSubscribedSku.ProductSku.Id.ToString() == minecraftProductSkuId)
                {
                    minecraftSubscribedSku = customerSubscribedSku;
                }
            }

            if (minecraftSubscribedSku == null)
            {
                Console.WriteLine("Customer user doesnt have subscribed sku");
                this.Context.ConsoleHelper.StopProgress();
                return;
            }

            this.Context.ConsoleHelper.StopProgress();

            // Prepare license request.
            LicenseUpdate updateLicense = new LicenseUpdate();

            // Select the license
            SubscribedSku sku = minecraftSubscribedSku;
            LicenseAssignment license = new LicenseAssignment
            {

                // Assigning subscribed sku as the license
                SkuId = sku.ProductSku.Id,
                ExcludedPlans = null
            };

            List<LicenseAssignment> licenseList = new List<LicenseAssignment>
            {
                license
            };
            updateLicense.LicensesToAssign = licenseList;

            this.Context.ConsoleHelper.StartProgress("Assigning License");

            // Assign licenses to the user.
            LicenseUpdate assignLicense = partnerOperations.Customers.ById(selectedCustomerId).Users.ById(selectedCustomerUserId).LicenseUpdates.Create(updateLicense);
            this.Context.ConsoleHelper.StopProgress();

            this.Context.ConsoleHelper.StartProgress("Getting Assigned License");

            // Get customer user assigned licenses information after assigning the license.
            Models.ResourceCollection<License> customerUserAssignedLicenses = partnerOperations.Customers.ById(selectedCustomerId).Users.ById(selectedCustomerUserId).Licenses.Get(groupIds);
            this.Context.ConsoleHelper.StopProgress();

            Console.WriteLine("License was successfully assigned to the user.");
            License userLicense = customerUserAssignedLicenses.Items.First(licenseItem => licenseItem.ProductSku.Id == license.SkuId);
            this.Context.ConsoleHelper.WriteObject(userLicense, "Assigned License");
        }
    }
}
