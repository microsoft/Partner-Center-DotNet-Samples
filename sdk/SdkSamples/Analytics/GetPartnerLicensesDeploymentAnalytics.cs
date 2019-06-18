// -----------------------------------------------------------------------
// <copyright file="GetPartnerLicensesDeploymentAnalytics.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Analytics
{
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
            IAggregatePartner partnerOperations = this.Context.UserPartnerOperations;
            this.Context.ConsoleHelper.StartProgress("Retrieving partner licenses deployment analytics");

            Models.ResourceCollection<Models.Analytics.PartnerLicensesDeploymentInsights> partnerLicensesDeploymentAnalytics = partnerOperations.Analytics.Licenses.Deployment.Get();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(partnerLicensesDeploymentAnalytics, "Partner licenses deployment analytics");
        }
    }
}
