// -----------------------------------------------------------------------
// <copyright file="ConvertTrialSubscription.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Subscriptions
{
    using System.Linq;

    /// <summary>
    /// A scenario that converts a trial subscription to paid subscription.
    /// </summary>
    public class ConvertTrialSubscription : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertTrialSubscription"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public ConvertTrialSubscription(IScenarioContext context) : base("Convert customer trial subscription", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;
            string customerId = this.ObtainCustomerId();
            string subscriptionId = this.ObtainSubscriptionId(customerId, "Enter the ID of the trial subscription to find conversions for");
            var subscriptionOperations = partnerOperations.Customers.ById(customerId).Subscriptions.ById(subscriptionId);

            this.Context.ConsoleHelper.StartProgress("Retrieving subscription conversions");
            var conversions = subscriptionOperations.Conversions.Get();
            this.Context.ConsoleHelper.StopProgress();

            if (conversions.TotalCount <= 0)
            {
                this.Context.ConsoleHelper.Error("This subscription has no conversions");
            }
            else
            {
                // Default to the first conversion.
                var selectedConversion = conversions.Items.ToList()[0];
                this.Context.ConsoleHelper.WriteObject(conversions, "Available conversions");              
                this.Context.ConsoleHelper.StartProgress("Converting trial subscription");
                var convertResult = subscriptionOperations.Conversions.Create(selectedConversion);
                this.Context.ConsoleHelper.StopProgress();
                this.Context.ConsoleHelper.WriteObject(convertResult, "Conversion details");
            }
        }
    }
}
