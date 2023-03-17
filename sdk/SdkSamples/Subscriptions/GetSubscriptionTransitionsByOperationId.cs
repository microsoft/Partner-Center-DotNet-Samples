// -----------------------------------------------------------------------
// <copyright file="GetSubscriptionTransitionsByOperationId.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Subscriptions
{
    /// <summary>
    /// A scenario that retrieves a customer subscription transitions by operation ID.
    /// </summary>
    public class GetSubscriptionTransitionsByOperationId : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetSubscriptionTransitionsByOperationId"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetSubscriptionTransitionsByOperationId(IScenarioContext context) : base("Get customer subscription transitions by operation ID", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;
            string customerId = this.ObtainCustomerId("Enter the ID of the customer whom to retrieve their Subscriptions");
            string subscriptionId = this.ObtainSubscriptionId(customerId, "Enter the ID of the subscription to find transitions for");
            string operationId = this.Context.ConsoleHelper.ReadNonEmptyString("Enter the operation ID of the transition");


            var subscriptionOperations = partnerOperations.Customers.ById(customerId).Subscriptions.ById(subscriptionId);

            this.Context.ConsoleHelper.StartProgress("Retrieving customer subscription transitions by operation ID");
            var transitions = subscriptionOperations.Transitions.Get(operationId);

            this.Context.ConsoleHelper.StopProgress();

            this.Context.ConsoleHelper.WriteObject(transitions, "Customer subscription transitions by operation ID");
        }
    }
}
