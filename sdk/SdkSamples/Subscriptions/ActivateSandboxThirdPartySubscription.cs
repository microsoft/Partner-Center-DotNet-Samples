// -----------------------------------------------------------------------
// <copyright file="ActivateSandboxThirdPartySubscription.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Subscriptions
{
    /// <summary>
    /// Activate Sandbox 3PP subscription.
    /// </summary>
    public class ActivateSandboxThirdPartySubscription : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActivateSandboxThirdPartySubscription"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public ActivateSandboxThirdPartySubscription(IScenarioContext context) : base("Activate Sandbox third party subscription", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;
            var customerId = this.ObtainCustomerId();
            var subscriptionId = this.ObtainSubscriptionId(customerId, "Enter the ID of the subscription to activate");
            var subscriptionOperations = partnerOperations.Customers.ById(customerId).Subscriptions.ById(subscriptionId);

            this.Context.ConsoleHelper.StartProgress("Activating subscription");
            var subscriptionActivationResult = subscriptionOperations.Activate();
            this.Context.ConsoleHelper.StopProgress();

            this.Context.ConsoleHelper.WriteObject(subscriptionActivationResult, "Subscription activation result");
        }
    }
}
