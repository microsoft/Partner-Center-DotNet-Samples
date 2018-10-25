// -----------------------------------------------------------------------
// <copyright file="GetServiceIncidents.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.ServiceIncidents
{
    using Store.PartnerCenter.Models.Query;
    using Store.PartnerCenter.Models.ServiceIncidents;

    /// <summary>
    /// Gets the list of service incidents for a partner
    /// </summary>
    public class GetServiceIncidents : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetServiceIncidents"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetServiceIncidents(IScenarioContext context) : base("Get Service Incidents", context)
        {
        }

        /// <summary>
        /// executes the get service incidents scenario.
        /// </summary>
        protected override void RunScenario()
        {
            const string SearchTerm = "false";
            var partnerOperations = this.Context.UserPartnerOperations;
            this.Context.ConsoleHelper.StartProgress("Retrieving Service incidents");

            // Query service incidents based on their active status - resolved or not. resolved = false fetches all the active incidents for all subscribed services. 
            var serviceIncidents = partnerOperations.ServiceIncidents.Query(QueryFactory.Instance.BuildIndexedQuery(1, 0, new SimpleFieldFilter(ServiceIncidentSearchField.Resolved.ToString(), FieldFilterOperation.Equals, SearchTerm)));
            this.Context.ConsoleHelper.StopProgress();

            this.Context.ConsoleHelper.WriteObject(serviceIncidents, "Service Incidents");
        }
    }
}
