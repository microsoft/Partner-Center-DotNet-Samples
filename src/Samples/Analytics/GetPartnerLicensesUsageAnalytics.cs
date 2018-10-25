// -----------------------------------------------------------------------
// <copyright file="GetPartnerLicensesUsageAnalytics.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Analytics
{
    using Models;
    using Models.Analytics;

    /// <summary>
    /// Gets partner's licenses usage analytics.
    /// </summary>
    public class GetPartnerLicensesUsageAnalytics : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetPartnerLicensesUsageAnalytics"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetPartnerLicensesUsageAnalytics(IScenarioContext context) : base("Get partner licenses usage analytics", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            IAggregatePartner partnerOperations = Context.UserPartnerOperations;
            Context.ConsoleHelper.StartProgress("Retrieving partner licenses usage analytics");

            ResourceCollection<PartnerLicensesUsageInsights> partnerLicensesUsageAnalytics = partnerOperations.Analytics.Licenses.Usage.Get();

            Context.ConsoleHelper.StopProgress();
            Context.ConsoleHelper.WriteObject(partnerLicensesUsageAnalytics, "Partner licenses usage analytics");
        }
    }
}