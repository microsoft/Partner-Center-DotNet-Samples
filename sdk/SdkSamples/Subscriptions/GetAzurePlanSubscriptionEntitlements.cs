namespace Microsoft.Store.PartnerCenter.Samples.Subscriptions
{
    using System.Globalization;

    /// <summary>
    /// A scenario that gets the entitlements under an Azure Plan
    /// </summary>
    public class GetAzurePlanSubscriptionEntitlements : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetAzurePlanSubscriptionEntitlements"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetAzurePlanSubscriptionEntitlements(IScenarioContext context) : base("Get Azure Plan entitlements", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;

            // obtain the customer ID, the ID of the subscription to amend with the add on offer and the add on offer ID
            var customerId = this.ObtainCustomerId();
            var subscriptionId = this.ObtainSubscriptionId(customerId, "Enter the subscription ID of the Azure Plan");

            this.Context.ConsoleHelper.StartProgress(string.Format(CultureInfo.InvariantCulture, "Getting Azure Plan entitlements for customer {0}", customerId));
            var azurePlanEntitlements = partnerOperations.Customers.ById(customerId).Subscriptions.ById(subscriptionId).GetAzurePlanSubscriptionEntitlements();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(azurePlanEntitlements, string.Format(CultureInfo.InvariantCulture, "Azure Plan entitlements for customer {0}", customerId));
        }
    }
}