// -----------------------------------------------------------------------
// <copyright file="GetPagedPartnerServiceRequests.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.ServiceRequests
{
    using System;
    using System.Globalization;
    using Store.PartnerCenter.Models.Query;

    /// <summary>
    /// Get paged partner service requests.
    /// </summary>
    public class GetPagedPartnerServiceRequests : BasePartnerScenario
    {
        /// <summary>
        /// The service requests page size.
        /// </summary>
        private readonly int serviceRequestPageSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetPagedPartnerServiceRequests"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        /// <param name="serviceRequestPageSize">The number of service requests to return per page.</param>
        public GetPagedPartnerServiceRequests(IScenarioContext context, int serviceRequestPageSize = 0) : base("Get paged partner service requests", context)
        {
            this.serviceRequestPageSize = serviceRequestPageSize;
        }

        /// <summary>
        /// executes the get paged partner service requests scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;
            this.Context.ConsoleHelper.StartProgress("Querying Service Requests");

            // query the service requests, get the first page if a page size was set
            var serviceRequestsPage = partnerOperations.ServiceRequests.Query(QueryFactory.Instance.BuildIndexedQuery(this.serviceRequestPageSize));
            this.Context.ConsoleHelper.StopProgress();

            // create a service requests enumerator which will aid us in traversing the service requests pages
            var serviceRequestsEnumerator = partnerOperations.Enumerators.ServiceRequests.Create(serviceRequestsPage);
            int pageNumber = 1;

            while (serviceRequestsEnumerator.HasValue)
            {
                this.Context.ConsoleHelper.WriteObject(serviceRequestsEnumerator.Current, string.Format(CultureInfo.InvariantCulture, "Service Requests Page: {0}", pageNumber++));
                Console.WriteLine();
                Console.Write("Press any key to retrieve the next service request page");
                Console.ReadKey();

                this.Context.ConsoleHelper.StartProgress("Getting next service request page");

                // get the next page of service requests
                serviceRequestsEnumerator.Next();

                this.Context.ConsoleHelper.StopProgress();
                Console.Clear();
            }
        }
    }
}
