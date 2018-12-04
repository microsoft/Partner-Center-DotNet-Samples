// -----------------------------------------------------------------------
// <copyright file="GetPartnerServiceRequestDetails.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.ServiceRequests
{
    using System;
    using Store.PartnerCenter.Models.ServiceRequests;

    /// <summary>
    /// Gets a service request details for a partner.
    /// </summary>
    public class GetPartnerServiceRequestDetails : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetPartnerServiceRequestDetails"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetPartnerServiceRequestDetails(IScenarioContext context) : base("Get service request details", context)
        {
        }

        /// <summary>
        /// executes the get service request details scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;
            string serviceRequestIdToRetrieve = this.Context.Configuration.Scenario.DefaultServiceRequestId;

            if (string.IsNullOrWhiteSpace(serviceRequestIdToRetrieve))
            {
                // prompt the user the enter the service request ID
                serviceRequestIdToRetrieve = this.Context.ConsoleHelper.ReadNonEmptyString("Please enter the ID of the service request to retrieve ", "The ID can't be empty");
            }
            else
            {
                Console.WriteLine("Found service request ID: {0} in configuration.", serviceRequestIdToRetrieve);
            }

            this.Context.ConsoleHelper.StartProgress("Retrieving Service Request");
            ServiceRequest serviceRequest = partnerOperations.ServiceRequests.ById(serviceRequestIdToRetrieve).Get();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(serviceRequest, "Service Request details");
        }
    }
}
