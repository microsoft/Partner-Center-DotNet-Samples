// -----------------------------------------------------------------------
// <copyright file="GetPartnerLicensesDeploymentAnalytics.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Analytics
{
    using Models;
    using Models.Analytics;

    /// <summary>
    /// Gets partner's licenses deployment analytics.
    /// </summary>
    public class GetPartnerLicensesDeploymentAnalytics : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetPartnerLicensesDeploymentAnalytics"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetPartnerLicensesDeploymentAnalytics(IScenarioContext context) : base("Get partner licenses deployment analytics", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            IAggregatePartner partnerOperations = Context.UserPartnerOperations;
            Context.ConsoleHelper.StartProgress("Retrieving partner licenses deployment analytics");

            ResourceCollection<PartnerLicensesDeploymentInsights> partnerLicensesDeploymentAnalytics = partnerOperations.Analytics.Licenses.Deployment.Get();

            Context.ConsoleHelper.StopProgress();
            Context.ConsoleHelper.WriteObject(partnerLicensesDeploymentAnalytics, "Partner licenses deployment analytics");
        }
    }
}