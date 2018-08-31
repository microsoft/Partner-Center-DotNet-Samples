// ---------------------------------------------------------------------------
// <copyright file="CreatePartnerServiceRequest.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.ServiceRequests
{
    using System;
    using Store.PartnerCenter.Models;
    using Store.PartnerCenter.Models.ServiceRequests;

    /// <summary>
    /// Creates a new service request.
    /// </summary>
    public class CreatePartnerServiceRequest : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreatePartnerServiceRequest"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public CreatePartnerServiceRequest(IScenarioContext context) : base("Create a new partner service request", context)
        {
        }

        /// <summary>
        /// executes the create partner service request scenario.
        /// </summary>
        protected override void RunScenario()
        {
            string supportTopicId = this.Context.Configuration.Scenario.DefaultSupportTopicId;
            var partnerOperations = this.Context.UserPartnerOperations;

            if (string.IsNullOrEmpty(supportTopicId.ToString()))
            {
                this.Context.ConsoleHelper.StartProgress("Fetching support topics");

                // Get the list of support topics
                ResourceCollection<SupportTopic> supportTopicsCollection = partnerOperations.ServiceRequests.SupportTopics.Get();

                this.Context.ConsoleHelper.StopProgress();
                this.Context.ConsoleHelper.WriteObject(supportTopicsCollection, "Support topics");

                // prompt the user the enter the support topic ID
                supportTopicId = this.Context.ConsoleHelper.ReadNonEmptyString("Please enter the support topic ID ", "The support topic ID can't be empty");
            }
            else
            {
                Console.WriteLine("Found support topic ID: {0} in configuration.", supportTopicId);
            }

            ServiceRequest serviceRequestToCreate = new ServiceRequest()
            {
                Title = "TrialSR",
                Description = "Ignore this SR",
                Severity = ServiceRequestSeverity.Critical,
                SupportTopicId = supportTopicId
            };

            this.Context.ConsoleHelper.StartProgress("Creating Service Request");

            ServiceRequest serviceRequest = partnerOperations.ServiceRequests.Create(serviceRequestToCreate, "en-US");

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(serviceRequest, "Created Service Request");
        }
    }
}