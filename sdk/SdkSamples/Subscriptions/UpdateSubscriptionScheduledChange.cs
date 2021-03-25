// -----------------------------------------------------------------------
// <copyright file="UpdateSubscriptionScheduledChange.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Subscriptions
{
    using Microsoft.Store.PartnerCenter.Models.Offers;
    using Microsoft.Store.PartnerCenter.Models.Subscriptions;
    using System;
    using System.Linq;

    /// <summary>
    /// A scenario that updates a customer subscription scheduled change.
    /// </summary>
    public class UpdateSubscriptionScheduledChange : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpgradeSubscription"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public UpdateSubscriptionScheduledChange(IScenarioContext context) : base("Update customer subscription scheduled change", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;
            string customerId = this.ObtainCustomerId();
            string subscriptionId = this.ObtainSubscriptionId(customerId, "Enter the ID of the subscription to update the scheduled change for");
            var subscriptionOperations = partnerOperations.Customers.ById(customerId).Subscriptions.ById(subscriptionId);

            this.Context.ConsoleHelper.StartProgress("Getting subscription");
            var selectedSubscription = subscriptionOperations.Get();
            this.Context.ConsoleHelper.WriteObject(selectedSubscription, "Existing subscription");
            this.Context.ConsoleHelper.StopProgress();

            this.Context.ConsoleHelper.StartProgress("Retrieving transition eligibilities for scheduled change");
            var transitionEligibilities = subscriptionOperations.TransitionEligibilities.Get("scheduled");
            this.Context.ConsoleHelper.StopProgress();

            if (transitionEligibilities.TotalCount <= 0)
            {
                this.Context.ConsoleHelper.Error("This subscription has no eligible transitions for scheduled change");
            }
            else
            {
                this.Context.ConsoleHelper.WriteObject(transitionEligibilities, "Available transition eligibilities");

                // prompt the user to enter the catalog item ID of the transition eligibilities he wishes to get
                string catalogItemId = this.Context.ConsoleHelper.ReadNonEmptyString("Enter the scheduled change catalog item ID", "Scheduled change catalog item ID can't be empty");
                var selectedEligibility = (from eligibility in transitionEligibilities.Items where eligibility.CatalogItemId == catalogItemId select eligibility).FirstOrDefault();

                if (selectedEligibility == null)
                {
                    this.Context.ConsoleHelper.Error("The entered scheduled change catalog item ID was not found in the list of transition eligibilities");
                }
                else if (!selectedEligibility.Eligibilities.Any(item => item.IsEligible))
                {
                    this.Context.ConsoleHelper.Error("The entered scheduled change catalog item ID is not eligible for the following reasons:");
                    this.Context.ConsoleHelper.WriteObject(selectedEligibility.Eligibilities, indent: 1);
                }
                else
                {
                    var catalog = selectedEligibility.CatalogItemId.Split(':');
                    var changeToProductId = catalog[0];
                    var changeToSkuId = catalog[1];
                    var changeToAvailabilityId = catalog[2];

                    this.Context.ConsoleHelper.StartProgress("Retrieving catalog availability terms");
                    var avaiability = partnerOperations.Customers.ById(customerId).Products.ById(changeToProductId).Skus.ById(changeToSkuId).Availabilities.ById(changeToAvailabilityId).Get();
                    this.Context.ConsoleHelper.StopProgress();
                    this.Context.ConsoleHelper.WriteObject(avaiability.Terms, "Available catalog availability terms");

                    // prompt the user to enter the billing cycle for the scheduled change
                    string billingCycle = this.Context.ConsoleHelper.ReadNonEmptyString("Enter the scheduled change billing cycle", "Scheduled change billing cycle item ID can't be empty");
                    var changeToBillingCycle = (BillingCycleType)Enum.Parse(typeof(BillingCycleType), billingCycle);

                    // prompt the user to enter the term duration for the scheduled change
                    string changeToTermDuration = this.Context.ConsoleHelper.ReadNonEmptyString("Enter the scheduled change term duration", "Scheduled change term duration can't be empty");

                    // prompt the user to enter the term duration for the scheduled change
                    string quantity = this.Context.ConsoleHelper.ReadNonEmptyString("Enter the scheduled change quantity", "Scheduled change term quantity can't be empty");
                    var changeToQuantity = int.Parse(quantity);

                    this.Context.ConsoleHelper.StartProgress("Updating subscription scheduled change");
                    selectedSubscription.ScheduledNextTermInstructions = new ScheduledNextTermInstructions
                    {
                        Product = new ProductTerm
                        {
                            ProductId = changeToProductId,
                            SkuId = changeToSkuId,
                            AvailabilityId = changeToAvailabilityId,
                            BillingCycle = changeToBillingCycle,
                            TermDuration = changeToTermDuration,
                        },
                        Quantity = changeToQuantity,
                    };

                    var updatedSubscription = subscriptionOperations.Patch(selectedSubscription);
                    this.Context.ConsoleHelper.StopProgress();
                    this.Context.ConsoleHelper.WriteObject(updatedSubscription, "Updated subscription scheduled change");
                }
            }
        }
    }
}
