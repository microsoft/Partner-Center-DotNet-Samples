// -----------------------------------------------------------------------
// <copyright file="GetSubscriptionsByMpnId.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.IndirectPartners
{
    using System;
    using System.Globalization;

    /// <summary>
    /// A scenario that gets a customer's subscriptions which belong to a partner MPN ID.
    /// </summary>
    public class GetSubscriptionsByMpnId : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetSubscriptionsByMpnId"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetSubscriptionsByMpnId(IScenarioContext context) : base("Get customer subscriptions by partner MPN ID", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            string customerId = this.ObtainCustomerId();
            string partnerMpnId = this.ObtainMpnId();

            var partnerOperations = this.Context.UserPartnerOperations;
            this.Context.ConsoleHelper.StartProgress("Getting subscriptions");

            var customerSubscriptionsByMpnId = partnerOperations.Customers.ById(customerId).Subscriptions.ByPartner(partnerMpnId).Get();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(
                customerSubscriptionsByMpnId,
                string.Format(CultureInfo.InvariantCulture, "Customer subscriptions by MPN ID: {0}", partnerMpnId));
        }
    }
}
