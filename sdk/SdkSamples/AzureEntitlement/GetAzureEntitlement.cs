// -----------------------------------------------------------------------
// <copyright file="GetAzureEntitlement.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.AzureEntitlement
{
    /// <summary>
    /// Get an Azure entitlement.
    /// </summary>
    public class GetAzureEntitlement : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetAzureEntitlement"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public GetAzureEntitlement(IScenarioContext context) : base("Get an Azure entitlement", context)
        {
        }

        /// <summary>
        /// Runs the scenario logic. This is delegated to the implementing sub class.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;

            var customerId = this.ObtainCustomerId("Enter the ID of the customer whom to retrieve their Subscription");
            var subscriptionId = this.ObtainSubscriptionId(customerId, "Enter the Azure plan ID to retrieve");
            var azureEntitlementId = this.ObtainAzureEntitlementId(customerId, subscriptionId, "Enter the Azure entitlement ID to retrieve");

            this.Context.ConsoleHelper.StartProgress("Retrieving customer's Azure entitlement");
            var azureEntitlement = partnerOperations.Customers.ById(customerId)
                    .Subscriptions.ById(subscriptionId)
                    .AzureEntitlement.ById(azureEntitlementId)
                    .Get();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(azureEntitlement, "Customer's Azure entitlement");
        }
    }
}
