// -----------------------------------------------------------------------
// <copyright file="TransitionSubscription.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Subscriptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Store.PartnerCenter.Models.PromotionEligibilities;
    using Microsoft.Store.PartnerCenter.Models.Subscriptions;

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
                            else
                            {
                                var sourceSubscription = subscriptionOperations.Get();
                                targetTransition.BillingCycle = sourceSubscription.BillingCycle.ToString();
                            }
                        }
                        else
                        {
                            var sourceSubscription = subscriptionOperations.Get();
                            targetTransition.TermDuration = sourceSubscription.TermDuration;
                            targetTransition.BillingCycle = sourceSubscription.BillingCycle.ToString();
                        }

                        string updatePromoId = this.Context.ConsoleHelper.ReadOptionalString("Would you like to set target promotion id? [y/n]");

                        if (string.Equals(updatePromoId, "y", System.StringComparison.OrdinalIgnoreCase))
                        {
                            string promotionCountry = this.Context.ConsoleHelper.ReadOptionalString("Enter promotion country, leave blank to default to US");

                            if (string.IsNullOrWhiteSpace(promotionCountry))
                            {
                                promotionCountry = "US";
                            }

                            string promotionSegment = this.Context.ConsoleHelper.ReadOptionalString("Enter promotion segment, leave blank to default to Commercial");

                            if (string.IsNullOrWhiteSpace(promotionSegment))
                            {
                                promotionSegment = "Commercial";
                            }

                            // Get available promotions
                            this.Context.ConsoleHelper.StartProgress("Retrieving available promotions");
                            var availablePromotions = partnerOperations.ProductPromotions.ByCountry(promotionCountry).BySegment(promotionSegment).Get();
                            this.Context.ConsoleHelper.StopProgress();

                            var psaArray = targetTransition.ToCatalogItemId.Split(':');

                            var availablePromotionsByTargetTransition = availablePromotions.Items.FirstOrDefault(
                                item => item.RequiredProducts.Any(
                                            eligibleItem =>
                                            string.Equals(eligibleItem.ProductId, psaArray[0], StringComparison.InvariantCultureIgnoreCase) &&
                                            string.Equals(eligibleItem.SkuId, psaArray[1], StringComparison.InvariantCultureIgnoreCase) &&
                                            string.Equals(eligibleItem.Term.Duration, targetTransition.TermDuration, StringComparison.InvariantCultureIgnoreCase) &&
                                            string.Equals(eligibleItem.Term.BillingCycle, targetTransition.BillingCycle, StringComparison.InvariantCultureIgnoreCase)));

                            this.Context.ConsoleHelper.WriteObject(availablePromotionsByTargetTransition, "Available promotion based on target product, term and billing frequency");

                            // prompt the user to enter the promotion ID of the desired promotion
                            string targetPromotionId = this.Context.ConsoleHelper.ReadOptionalString("Enter the promotion ID");

                            if (!string.IsNullOrWhiteSpace(targetPromotionId))
                            {
                                targetTransition.PromotionId = targetPromotionId;

                                Enum.TryParse<Models.PromotionEligibilities.Enums.BillingCycleType>(targetTransition.BillingCycle, true, out var targetBillingCycle);

                                // Build the promotion elibities request.
                                var promotionEligibilitiesRequest = new PromotionEligibilitiesRequest()
                                {
                                    Items = new List<PromotionEligibilitiesRequestItem>()
                                    {
                                        new PromotionEligibilitiesRequestItem()
                                        {
                                            Id = 0,
                                            CatalogItemId = targetTransition.ToCatalogItemId,
                                            TermDuration = targetTransition.TermDuration,
                                            BillingCycle = targetBillingCycle,
                                            Quantity = targetTransition.Quantity,
                                            PromotionId = targetTransition.PromotionId,
                                        },
                                    },
                                };

                                this.Context.ConsoleHelper.StartProgress("Retrieving promotion eligibilities");
                                var promotionEligibilities = partnerOperations.Customers.ById(customerId).PromotionEligibilities.Post(promotionEligibilitiesRequest);
                                this.Context.ConsoleHelper.StopProgress();

                                foreach (var eligibility in promotionEligibilities.Items)
                                {
                                    Console.Out.WriteLine("Eligibility result for CatalogItemId: {0}", eligibility.CatalogItemId);
                                    Console.Out.WriteLine("IsCustomerEligible: {0}", eligibility.Eligibilities.First().IsEligible.ToString());

                                    if (!eligibility.Eligibilities.First().IsEligible)
                                    {
                                        Console.Out.WriteLine("Reasons for ineligibility:");
                                        foreach (var error in eligibility.Eligibilities.First().Errors)
                                        {
                                            Console.Out.WriteLine("Type: {0}", error.Type);
                                            Console.Out.WriteLine("Description: {0}", error.Description);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // the selected transition is eligible, go ahead and perform the transition
                    this.Context.ConsoleHelper.StartProgress("Transitioning subscription");

                    var transitionResult = subscriptionOperations.Transitions.Create(targetTransition);
                    this.Context.ConsoleHelper.StopProgress();
                    this.Context.ConsoleHelper.WriteObject(transitionResult, "Transtion details");
                }
            }
        }
    }
}
