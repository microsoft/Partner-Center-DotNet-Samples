// -----------------------------------------------------------------------
// <copyright file="CancelAzureEntitlement.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.AzureEntitlement
{
    using Microsoft.Store.PartnerCenter.Models.Subscriptions;

    /// <summary>
    /// Cancel an Azure entitlement.
    /// </summary>
    public class CancelAzureEntitlement : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CancelAzureEntitlement"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public CancelAzureEntitlement(IScenarioContext context) : base("Cancel an Azure entitlement", context)
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

            var selectedCancellationReasonCode = this.Context.ConsoleHelper.ReadNonEmptyString(
                    "Enter the cancellation reason code (ex: compromise): ",
                    "Cancellation reason code can't be empty");

            if (selectedCancellationReasonCode != "compromise")
            {
                this.Context.ConsoleHelper.Error("Entered cancellation reason code is not supported. Please enter valid cancellation reason code.");
            }

            var azureEntitlementCancellationRequestContent = new AzureEntitlementCancellationRequestContent
            {
                CancellationReason = selectedCancellationReasonCode
            };

            this.Context.ConsoleHelper.StartProgress("Canceling customer's Azure entitlement");
            var azureEntitlement = partnerOperations.Customers.ById(customerId)
                        .Subscriptions.ById(subscriptionId)
                        .AzureEntitlements.ById(azureEntitlementId)
                        .Cancel(azureEntitlementCancellationRequestContent);

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(azureEntitlement, "Customer's Azure entitlement");
        }
    }
}
