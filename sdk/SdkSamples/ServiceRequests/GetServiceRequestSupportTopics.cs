// -----------------------------------------------------------------------
// <copyright file="GetServiceRequestSupportTopics.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.ServiceRequests
{
    using Store.PartnerCenter.Models;
    using Store.PartnerCenter.Models.ServiceRequests;

    /// <summary>
    /// Gets the list of support topics.
    /// </summary>
    public class GetServiceRequestSupportTopics : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetServiceRequestSupportTopics"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetServiceRequestSupportTopics(IScenarioContext context) : base("Get a list of service request support topics", context)
        {
        }

        /// <summary>
        /// executes the get service request support topics scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;

            this.Context.ConsoleHelper.StartProgress("Getting support topics");

            // Get support Topics
            ResourceCollection<SupportTopic> supportTopicsCollection = partnerOperations.ServiceRequests.SupportTopics.Get();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(supportTopicsCollection);
        }
    }
}