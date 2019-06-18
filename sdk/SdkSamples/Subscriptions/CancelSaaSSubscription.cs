// -----------------------------------------------------------------------
// <copyright file="CancelSaaSSubscription.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Subscriptions
{
    /// <summary>
    /// A scenario that updates an existing customer subscription.
    /// </summary>
    public class CancelSaaSSubscription : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CancelSaaSSubscription"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public CancelSaaSSubscription(IScenarioContext context) : base("Cancel existing customer SaaS subscription", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            IAggregatePartner partnerOperations = this.Context.UserPartnerOperations;
            string customerId = this.ObtainCustomerId();
            string subscriptionId = this.ObtainSubscriptionId(customerId, "Enter the ID of the subscription to cancel");

            this.Context.ConsoleHelper.StartProgress("Retrieving customer subscription");
            Models.Subscriptions.Subscription existingSubscription = partnerOperations.Customers.ById(customerId).Subscriptions.ById(subscriptionId).Get();
            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(existingSubscription, "Existing subscription");

            this.Context.ConsoleHelper.StartProgress("Cancelling subscription");
            existingSubscription.Status = Models.Subscriptions.SubscriptionStatus.Deleted;
            Models.Subscriptions.Subscription updatedSubscription = partnerOperations.Customers.ById(customerId).Subscriptions.ById(subscriptionId).Patch(existingSubscription);
            this.Context.ConsoleHelper.StopProgress();

            this.Context.ConsoleHelper.WriteObject(updatedSubscription, "Cancelled subscription");
        }
    }
}
