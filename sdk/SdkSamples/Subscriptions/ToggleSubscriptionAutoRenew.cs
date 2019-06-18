namespace Microsoft.Store.PartnerCenter.Samples.Subscriptions
{
    internal class ToggleSubscriptionAutoRenew : BasePartnerScenario

    {
        public ToggleSubscriptionAutoRenew(IScenarioContext context) : base("Toggle Subcription Autorenew", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            IAggregatePartner partnerOperations = this.Context.UserPartnerOperations;

            string customerId = this.ObtainCustomerId("Enter the ID of the customer whom to retrieve their subscription");
            string subscriptionId = this.ObtainSubscriptionId(customerId);

            this.Context.ConsoleHelper.StartProgress("Retrieving customer subscription");

            this.Context.ConsoleHelper.StartProgress("Retrieving customer subscription");
            Models.Subscriptions.Subscription existingSubscription = partnerOperations.Customers.ById(customerId).Subscriptions.ById(subscriptionId).Get();
            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(existingSubscription, "Existing subscription");

            this.Context.ConsoleHelper.StartProgress("Toggling subscription autorenew");
            existingSubscription.AutoRenewEnabled = !existingSubscription.AutoRenewEnabled;
            Models.Subscriptions.Subscription updatedSubscription = partnerOperations.Customers.ById(customerId).Subscriptions.ById(subscriptionId).Patch(existingSubscription);
            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(updatedSubscription, "Subscription after update");
        }
    }
}

