using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Store.PartnerCenter.Samples.Subscriptions
{
    class ToggleSubscriptionAutoRenew : BasePartnerScenario

    {
        public ToggleSubscriptionAutoRenew(IScenarioContext context) : base("Toggle Subcription Autorenew", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;

            string customerId = this.ObtainCustomerId("Enter the ID of the customer whom to retrieve their subscription");
            string subscriptionId = this.ObtainSubscriptionId(customerId);

            this.Context.ConsoleHelper.StartProgress("Retrieving customer subscription");

            this.Context.ConsoleHelper.StartProgress("Retrieving customer subscription");
            var existingSubscription = partnerOperations.Customers.ById(customerId).Subscriptions.ById(subscriptionId).Get();
            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(existingSubscription, "Existing subscription");

            this.Context.ConsoleHelper.StartProgress("Toggling subscription autorenew");
            existingSubscription.AutoRenewEnabled = !existingSubscription.AutoRenewEnabled;
            var updatedSubscription = partnerOperations.Customers.ById(customerId).Subscriptions.ById(subscriptionId).Patch(existingSubscription);
            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(updatedSubscription, "Subscription after update");
        }
    }
}

