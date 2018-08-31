// -----------------------------------------------------------------------
// <copyright file="UpdatePartnerServiceRequest.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.ServiceRequests
{
    using System;
    using Store.PartnerCenter.Models.ServiceRequests;

    /// <summary>
    /// Updates a partner's service request.
    /// </summary>
    public class UpdatePartnerServiceRequest : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdatePartnerServiceRequest"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public UpdatePartnerServiceRequest(IScenarioContext context) : base("Update a partner's service request", context)
        {
        }

        /// <summary>
        /// executes the update partner service request details scenario.
        /// </summary>
        protected override void RunScenario()
        {
            ServiceRequestNote note = new ServiceRequestNote { Text = "Sample Note" };
            var partnerOperations = this.Context.UserPartnerOperations;
            string serviceRequestIdToUpdate = this.Context.Configuration.Scenario.DefaultServiceRequestId;

            if (string.IsNullOrWhiteSpace(serviceRequestIdToUpdate))
            {
                // prompt the user the enter the service request ID
                serviceRequestIdToUpdate = this.Context.ConsoleHelper.ReadNonEmptyString("Please enter the ID of the service request to update", "The service request ID can't be empty");
            }
            else
            {
                Console.WriteLine("Found service request ID: {0} in configuration.", serviceRequestIdToUpdate);
            }

            this.Context.ConsoleHelper.StartProgress("Retrieving service request to be updated");

            // Retrieve service request
            ServiceRequest serviceRequest = partnerOperations.ServiceRequests.ById(serviceRequestIdToUpdate).Get();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(serviceRequest, "Service Request to be updated");
            this.Context.ConsoleHelper.StartProgress("Updating Service Request");

            // Updating service request
            ServiceRequest updatedServiceRequest = partnerOperations.ServiceRequests.ById(serviceRequestIdToUpdate).Patch(new ServiceRequest
            {
                NewNote = note
            });

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(updatedServiceRequest, "Updated Service Request details");
        }
    }
}
