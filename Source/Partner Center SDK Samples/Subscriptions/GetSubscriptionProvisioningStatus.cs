// -----------------------------------------------------------------------
// <copyright file="GetSubscriptionProvisioningStatus.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Subscriptions
{
    /// <summary>
    /// A scenario that retrieves a customer subscription's provisioning status.
    /// </summary>
    public class GetSubscriptionProvisioningStatus : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetSubscriptionProvisioningStatus"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetSubscriptionProvisioningStatus(IScenarioContext context) : base("Get subscription provisioning status", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;

            string customerId = this.ObtainCustomerId("Enter the ID of the customer whom to retrieve their Subscription provisioning status");
            string subscriptionID = this.ObtainSubscriptionId(customerId, "Enter the subscription ID to retrieve");

            this.Context.ConsoleHelper.StartProgress("Retrieving subscription provisioning status");

            var provisioningStatus = partnerOperations.Customers.ById(customerId).Subscriptions.ById(subscriptionID).ProvisioningStatus.Get();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(provisioningStatus, "Subscription provisioning status");
        }
    }
}
