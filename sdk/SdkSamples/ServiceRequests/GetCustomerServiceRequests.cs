// -----------------------------------------------------------------------
// <copyright file="GetCustomerServiceRequests.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.ServiceRequests
{
    using System;
    using System.Linq;
    using Store.PartnerCenter.Models;
    using Store.PartnerCenter.Models.ServiceRequests;

    /// <summary>
    /// Gets customer service requests.
    /// </summary>
    public class GetCustomerServiceRequests : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCustomerServiceRequests"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetCustomerServiceRequests(IScenarioContext context) : base("Get customer service requests", context)
        {
        }

        /// <summary>
        /// executes the get customer service requests scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;
            string customerIdToRetrieve = this.Context.Configuration.Scenario.DefaultCustomerId;

            if (string.IsNullOrWhiteSpace(customerIdToRetrieve))
            {
                // prompt the user the enter the customer ID
                customerIdToRetrieve = this.Context.ConsoleHelper.ReadNonEmptyString("Please enter the ID of the customer to retrieve: ", "The customer ID can't be empty");
            }
            else
            {
                Console.WriteLine("Found customer ID: {0} in configuration.", customerIdToRetrieve);
            }

            this.Context.ConsoleHelper.StartProgress("Retrieving Customer's Service Requests");

            ResourceCollection<ServiceRequest> serviceRequests =
                partnerOperations.Customers.ById(customerIdToRetrieve)
                    .ServiceRequests.Get();

            this.Context.ConsoleHelper.StopProgress();

            if (serviceRequests.Items.Count() == 0)
            {
                Console.WriteLine("No Service requests found for the given customer.");
            }
            else
            {
                this.Context.ConsoleHelper.WriteObject(serviceRequests, "Service Request results.");
            }
        }
    }
}
