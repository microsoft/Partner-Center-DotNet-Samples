// -----------------------------------------------------------------------
// <copyright file="GetSubscriptionSupportContact.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Subscriptions
{
    /// <summary>
    /// A scenario that retrieves the support contact of a customer's subscription.
    /// </summary>
    public class GetSubscriptionSupportContact : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetSubscriptionSupportContact"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetSubscriptionSupportContact(IScenarioContext context) : base("Get subscription support contact", context)
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

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(supportContact, "Subscription support contact");
        }
    }
}
