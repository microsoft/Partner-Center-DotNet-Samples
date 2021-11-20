// -----------------------------------------------------------------------
// <copyright file="TransitionSubscription.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Subscriptions
{
    using Microsoft.Store.PartnerCenter.Models.Subscriptions;
    using System.Linq;

    /// <summary>
    /// A scenario that transitions a customer subscription to a higher sku.
    /// </summary>
    public class TransitionSubscription : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransitionSubscription"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public TransitionSubscription(IScenarioContext context) : base("Transition customer subscription", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;
            string customerId = this.ObtainCustomerId();
            string subscriptionId = this.ObtainSubscriptionId(customerId, "Enter the ID of the subscription to find transition eligibilities for");
            var subscriptionOperations = partnerOperations.Customers.ById(customerId).Subscriptions.ById(subscriptionId);

            this.Context.ConsoleHelper.StartProgress("Retrieving subscription transition eligibilities");
            var transitionEligibilities = subscriptionOperations.TransitionEligibilities.Get();
            this.Context.ConsoleHelper.StopProgress();

            if (transitionEligibilities.TotalCount <= 0)
            {
                this.Context.ConsoleHelper.Error("This subscription has no transition eligibilities");
            }
            else
            {
                this.Context.ConsoleHelper.WriteObject(transitionEligibilities, "Available transition eligibilities");

                // prompt the user to enter the catalog item ID of the transition eligibilities he wishes to get
                string catalogItemId = this.Context.ConsoleHelper.ReadNonEmptyString("Enter the transition catalog item ID", "Transition catalog item ID can't be empty");
                var selectedEligibility = (from eligibility in transitionEligibilities.Items where eligibility.CatalogItemId == catalogItemId select eligibility).FirstOrDefault();

                if (selectedEligibility == null)
                {
                    this.Context.ConsoleHelper.Error("The entered transition catalog item ID was not found in the list of transition eligibilities");
                }
                else if (!selectedEligibility.Eligibilities.Any(item => item.IsEligible))
                {
                    this.Context.ConsoleHelper.Error("The entered transition eligibility is not eligible for the following reasons:");
                    this.Context.ConsoleHelper.WriteObject(selectedEligibility.Eligibilities, indent: 1);
                }
                else
                {
                    var targetTransition = new Transition()
                    {
                        ToCatalogItemId = selectedEligibility.CatalogItemId,
                        TransitionType = selectedEligibility.Eligibilities.FirstOrDefault(detail => detail.IsEligible == true).TransitionType
                    };

                    string targetQuantity = this.Context.ConsoleHelper.ReadOptionalString("Enter a quantity to transition or leave blank to transition all existing seats");

                    targetTransition.Quantity = !string.IsNullOrWhiteSpace(targetQuantity) ? int.Parse(targetQuantity) : selectedEligibility.Quantity;

                    string updateToSubscriptionsId = this.Context.ConsoleHelper.ReadOptionalString("Would you like to transition into an existing subscription? [y/n]");

                    if (string.Equals(updateToSubscriptionsId, "y", System.StringComparison.OrdinalIgnoreCase))
                    {
                        targetTransition.ToSubscriptionId = this.ObtainSubscriptionId(customerId, "Enter the ID of the subscription to set as target subscription for the transition (Must match same catalogItemId)");
                    }
                    else
                    {
                        string targetTermDuration = this.Context.ConsoleHelper.ReadOptionalString("Enter a new term duration to apply during the transition, leave blank to keep the same [example: P1Y, P1M]");

                        if (!string.IsNullOrWhiteSpace(targetTermDuration))
                        {
                            targetTransition.TermDuration = targetTermDuration;

                            string targetBillingCycle = this.Context.ConsoleHelper.ReadOptionalString("Enter a new billing cycle to apply during the transition, leave blank to keep the same [example: Annual or Monthly]");

                            if (!string.IsNullOrWhiteSpace(targetBillingCycle))
                            {
                                targetTransition.BillingCycle = targetBillingCycle;
                            }
                        }
                    }

                    // the selected transition is eligible, go ahead and perform the transition
                    this.Context.ConsoleHelper.StartProgress("Transtioning subscription");

                    var transitionResult = subscriptionOperations.Transitions.Create(targetTransition);
                    this.Context.ConsoleHelper.StopProgress();
                    this.Context.ConsoleHelper.WriteObject(transitionResult, "Transtion details");
                }
            }
        }
    }
}
