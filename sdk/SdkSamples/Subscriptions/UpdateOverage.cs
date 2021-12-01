// -----------------------------------------------------------------------
// <copyright file="UpdateOverage.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Subscriptions
{
    using Microsoft.Store.PartnerCenter.Models.Invoices;
    using Microsoft.Store.PartnerCenter.Models.Subscriptions;
    using System;
    using System.Linq;

    /// <summary>
    /// A scenario that updates a overage for subscriptions.
    /// </summary>
    public class UpdateOverage : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateOverage"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public UpdateOverage(IScenarioContext context) : base("Update subscription overage", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;
            string customerId = this.ObtainCustomerId();

            this.Context.ConsoleHelper.StartProgress("Getting subscription overage");
            var overage = partnerOperations.Customers.ById(customerId).Subscriptions.Overage.Get();
            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(overage, "Existing overage");

            this.Context.ConsoleHelper.StartProgress("Getting subscription");
            var subscriptions = partnerOperations.Customers.ById(customerId).Subscriptions.Get();

            if (!subscriptions.Items.Any(sub => sub.ConsumptionType == "overage"))
            {
                this.Context.ConsoleHelper.Error("No overage eligible subscription found for the customer");
                return;
            }
            this.Context.ConsoleHelper.StopProgress();

            string azureEntitlementIdForOverage = null;
            string partnerId = null;
            var modernAzureSubscription = subscriptions.Items.FirstOrDefault(sub => sub.BillingType == BillingType.Usage && !sub.OfferId.StartsWith("MS-AZR"));
            if (modernAzureSubscription == null)
            {
                this.Context.ConsoleHelper.WriteColored("Customer doesn't have modern Azure plan, a new Azure plan will be purchased.", ConsoleColor.Yellow);
            }
            else
            {
                this.Context.ConsoleHelper.WriteColored($"Modern Azure Subscription Id: {modernAzureSubscription.Id}", ConsoleColor.Yellow);

                this.Context.ConsoleHelper.StartProgress("Retrieving Azure plan entitlements");
                var azureEntitlments = partnerOperations.Customers.ById(customerId).Subscriptions.ById(modernAzureSubscription.Id).GetAzurePlanSubscriptionEntitlements();
                this.Context.ConsoleHelper.StopProgress();
                this.Context.ConsoleHelper.WriteObject(azureEntitlments, "Azure plan entitlements");

                var azureEntitlementForOverage = azureEntitlments.Items.FirstOrDefault(e => e.FriendlyName == "Subscription 1");
                azureEntitlementIdForOverage = azureEntitlementForOverage?.Id;
                if (azureEntitlementIdForOverage == null)
                {
                    this.Context.ConsoleHelper.WriteColored("No existing modern Azure plan entitlement found, a new azure entitlement will be created.", ConsoleColor.Yellow);
                }
                else
                {
                    azureEntitlementIdForOverage = this.Context.ConsoleHelper.ReadOptionalString(
                        $"Enter Azure entitlement Id for overage (Leaving empty will consider 'Subscription 1' entitlement if exists or else it will create new)");

                    partnerId = this.Context.ConsoleHelper.ReadOptionalString($"Enter Reseller partnerId (Mpn Id) for overage if a reseller is associated with the azure plan");
                }
            }

            this.Context.ConsoleHelper.StartProgress("Updating overage");
            var overagePayload = new Overage
            {
                AzureEntitlementId = azureEntitlementIdForOverage,
                OverageEnabled = true,
                PartnerId = partnerId,
            };
            var updatedOverage = partnerOperations.Customers.ById(customerId).Subscriptions.Overage.Put(overagePayload);
            this.Context.ConsoleHelper.StopProgress();

            this.Context.ConsoleHelper.StartProgress("Retrieving overage");
            var newOverage = partnerOperations.Customers.ById(customerId).Subscriptions.Overage.Get();
            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(newOverage, "Newly retrieved overage");

            var newPhoneOverage = newOverage.Items.FirstOrDefault(o => o.Type == "PhoneServices");

            if (!string.Equals(updatedOverage.AzureEntitlementId, newPhoneOverage?.AzureEntitlementId))
            {
                this.Context.ConsoleHelper.WriteColored("The Overage update process hasn't completed yet.", ConsoleColor.Yellow);
            }
        }
    }
}
