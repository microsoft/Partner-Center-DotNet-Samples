// -----------------------------------------------------------------------
// <copyright file="QueryAuditRecords.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Orders
{
    using System;
    using System.Globalization;
    using Models.Query;

    /// <summary>
    /// A scenario that retrieves a partner's audit records.
    /// </summary>
    public class QueryAuditRecords : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryAuditRecords"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public QueryAuditRecords(IScenarioContext context) : base("Query for the partner's audit records.", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            IAggregatePartner partnerOperations = this.Context.UserPartnerOperations;
            const int PageSize = 10;
            DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01);

            this.Context.ConsoleHelper.StartProgress(
                string.Format(CultureInfo.InvariantCulture, "Retrieving the partner's audit records - start date: {0} | page size: {1}", startDate, PageSize));

            Models.SeekBasedResourceCollection<Models.Auditing.AuditRecord> auditRecordsPage = partnerOperations.AuditRecords.Query(startDate.Date, query: QueryFactory.Instance.BuildIndexedQuery(PageSize));
            this.Context.ConsoleHelper.StopProgress();

            // create a customer enumerator which will aid us in traversing the customer pages
            Enumerators.IResourceCollectionEnumerator<Models.SeekBasedResourceCollection<Models.Auditing.AuditRecord>> auditRecordEnumerator = partnerOperations.Enumerators.AuditRecords.Create(auditRecordsPage);

            int pageNumber = 1;

            while (auditRecordEnumerator.HasValue)
            {
                // print the current audit record results page
                this.Context.ConsoleHelper.WriteObject(auditRecordEnumerator.Current, string.Format(CultureInfo.InvariantCulture, "Audit Record Page: {0}", pageNumber++));

                Console.WriteLine();
                Console.Write("Press any key to retrieve the next set of audit records");
                Console.ReadKey();

                this.Context.ConsoleHelper.StartProgress("Getting next audit records page");

                // get the next page of audit records
                auditRecordEnumerator.Next();

                this.Context.ConsoleHelper.StopProgress();
                Console.Clear();
            }
        }
    }
}
