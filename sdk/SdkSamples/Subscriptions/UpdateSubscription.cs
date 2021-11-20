// -----------------------------------------------------------------------
// <copyright file="UpdateSubscription.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.Store.PartnerCenter.Models.Offers;
using System;

namespace Microsoft.Store.PartnerCenter.Samples.Subscriptions
{
    /// <summary>
    /// A scenario that updates an existing customer subscription.
    /// </summary>
    public class UpdateSubscription : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateSubscription"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public UpdateSubscription(IScenarioContext context) : base("Update existing customer subscription", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;
            var customerId = this.ObtainCustomerId();
            var subscriptionId = this.ObtainSubscriptionId(customerId, "Enter the ID of the subscription to update");

            this.Context.ConsoleHelper.StartProgress("Retrieving customer subscription");
            var existingSubscription = partnerOperations.Customers.ById(customerId).Subscriptions.ById(subscriptionId).Get();
            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(existingSubscription, "Existing subscription");

            string targetQuantity = this.Context.ConsoleHelper.ReadOptionalString("Enter target quantity or leave blank to keep quantity the same");
            if (!string.IsNullOrWhiteSpace(targetQuantity)) {
                existingSubscription.Quantity = int.Parse(targetQuantity);
            }

            string targetTermDuration = this.Context.ConsoleHelper.ReadOptionalString("Enter a new term duration or leave blank to keep the same [example: P1Y, P1M]");

            if (!string.IsNullOrWhiteSpace(targetTermDuration))
            {
                existingSubscription.TermDuration = targetTermDuration;

                string targetBillingCycle = this.Context.ConsoleHelper.ReadOptionalString("Enter a new billing cycle or leave blank to keep the same [example: Annual or Monthly]");

                if (!string.IsNullOrWhiteSpace(targetBillingCycle))
                {
                    existingSubscription.BillingCycle = (BillingCycleType)Enum.Parse(typeof(BillingCycleType), targetBillingCycle, true);
                }
            }

            this.Context.ConsoleHelper.StartProgress("Updating subscription");
            var updatedSubscription = partnerOperations.Customers.ById(customerId).Subscriptions.ById(subscriptionId).Patch(existingSubscription);
            this.Context.ConsoleHelper.StopProgress();

            this.Context.ConsoleHelper.WriteObject(updatedSubscription, "Updated subscription");
        }
    }
}
