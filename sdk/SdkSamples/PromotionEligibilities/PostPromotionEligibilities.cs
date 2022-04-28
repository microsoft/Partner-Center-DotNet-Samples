// -----------------------------------------------------------------------
// <copyright file="PostPromotionEligibilities.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.PromotionEligibilities
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Store.PartnerCenter.Models.PromotionEligibilities;
    using Microsoft.Store.PartnerCenter.Models.PromotionEligibilities.Enums;

    /// <summary>
    /// A scenario that posts promotion eligibilities for a customer.
    /// </summary>
    public class PostPromotionEligibilities : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PostPromotionEligibilities"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public PostPromotionEligibilities(IScenarioContext context) : base("Post promotion eligibilities", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;

            string customerId = this.ObtainCustomerId("Enter the ID of the customer to check eligibility for");
            string catalogItemId = this.ObtainCatalogItemId("Enter the catalog Item Id");
            int quantity = int.Parse(this.ObtainQuantity("Enter the Quantity"));
            string termDuration = this.ObtainRenewalTermDuration("Enter the term duration");
            string billingCycle = this.ObtainBillingCycle("Enter the billing cycle");
            var billingCycleType = (BillingCycleType)Enum.Parse(typeof(BillingCycleType), billingCycle, true);
            var promotionId = this.Context.ConsoleHelper.ReadNonEmptyString("Enter the promotionId", "The promotionId can't be empty");

            var promotionEligibilityRequest = new PromotionEligibilitiesRequest()
            {
                Items = new List<PromotionEligibilitiesRequestItem>()
                {
                    new PromotionEligibilitiesRequestItem()
                    {
                        Id = 0,
                        CatalogItemId = catalogItemId,
                        Quantity = quantity,
                        BillingCycle = billingCycleType,
                        TermDuration = termDuration,
                        PromotionId = promotionId
                    }
                }
            };

            this.Context.ConsoleHelper.StartProgress("Posting promotion eligibilities");

            var promotionEligibilities = partnerOperations.Customers.ById(customerId).PromotionEligibilities.Post(promotionEligibilityRequest);

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(promotionEligibilities, "Posted promotion eligibilities");
        }
    }
}
