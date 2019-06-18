// -----------------------------------------------------------------------
// <copyright file="UpgradeSubscription.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Subscriptions
{
    using System.Linq;

    /// <summary>
    /// A scenario that upgrades a customer subscription to a higher skew.
    /// </summary>
    public class UpgradeSubscription : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpgradeSubscription"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public UpgradeSubscription(IScenarioContext context) : base("Upgrade customer subscription", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            IAggregatePartner partnerOperations = this.Context.UserPartnerOperations;
            string customerId = this.ObtainCustomerId();
            string subscriptionId = this.ObtainSubscriptionId(customerId, "Enter the ID of the subscription to find upgrades for");
            PartnerCenter.Subscriptions.ISubscription subscriptionOperations = partnerOperations.Customers.ById(customerId).Subscriptions.ById(subscriptionId);

            this.Context.ConsoleHelper.StartProgress("Retrieving subscription upgrades");
            Models.ResourceCollection<Models.Subscriptions.Upgrade> upgrades = subscriptionOperations.Upgrades.Get();
            this.Context.ConsoleHelper.StopProgress();

            if (upgrades.TotalCount <= 0)
            {
                this.Context.ConsoleHelper.Error("This subscription has no upgrades");
            }
            else
            {
                this.Context.ConsoleHelper.WriteObject(upgrades, "Available upgrades");

                // prompt the user to enter the offer ID of the upgrade he wishes to get
                string upgradeOfferId = this.Context.ConsoleHelper.ReadNonEmptyString("Enter the upgrade offer ID", "Upgrade offer ID can't be empty");
                Models.Subscriptions.Upgrade selectedUpgrade = (from upgrade in upgrades.Items where upgrade.TargetOffer.Id == upgradeOfferId select upgrade).FirstOrDefault();

                if (selectedUpgrade == null)
                {
                    this.Context.ConsoleHelper.Error("The entered upgrade offer ID was not found in the list of upgrades");
                }
                else if (!selectedUpgrade.IsEligible)
                {
                    this.Context.ConsoleHelper.Error("The entered upgrade is not eligible for the following reasons:");
                    this.Context.ConsoleHelper.WriteObject(selectedUpgrade.UpgradeErrors, indent: 1);
                }
                else
                {
                    // the selected upgrade is eligible, go ahead and perform the upgrade
                    this.Context.ConsoleHelper.StartProgress("Upgrading subscription");
                    Models.Subscriptions.UpgradeResult updgradeResult = subscriptionOperations.Upgrades.Create(selectedUpgrade);
                    this.Context.ConsoleHelper.StopProgress();
                    this.Context.ConsoleHelper.WriteObject(updgradeResult, "Upgrade details");
                }
            }
        }
    }
}
