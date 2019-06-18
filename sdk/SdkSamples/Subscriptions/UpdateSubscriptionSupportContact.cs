// -----------------------------------------------------------------------
// <copyright file="UpdateSubscriptionSupportContact.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Subscriptions
{
    /// <summary>
    /// A scenario that retrieves a customer subscription.
    /// </summary>
    public class UpdateSubscriptionSupportContact : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateSubscriptionSupportContact"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public UpdateSubscriptionSupportContact(IScenarioContext context) : base("Update subscription support contact", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            IAggregatePartner partnerOperations = this.Context.UserPartnerOperations;

            string customerId = this.ObtainCustomerId("Enter the ID of the customer whom to retrieve their Subscription");
            string subscriptionID = this.ObtainSubscriptionId(customerId, "Enter the subscription ID to retrieve");

            this.Context.ConsoleHelper.StartProgress("Retrieving subscription support contact");

            Models.Subscriptions.SupportContact supportContact = partnerOperations.Customers.ById(customerId).Subscriptions.ById(subscriptionID).SupportContact.Get();

            // Here we are the updating the support contact with the same object retrieved above. You can update it with a new object that has valid VAR values.
            Models.Subscriptions.SupportContact updatedSupportContact = partnerOperations.Customers.ById(customerId).Subscriptions.ById(subscriptionID).SupportContact.Update(supportContact);

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(updatedSupportContact, "Subscription support contact");
        }
    }
}
