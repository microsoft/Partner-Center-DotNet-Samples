// -----------------------------------------------------------------------
// <copyright file="SearchAuditRecordsByCustomerId.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Orders
{
    using System;
    using System.Globalization;
    using Microsoft.Store.PartnerCenter.Models.Auditing;
    using Models.Query;

    /// <summary>
    /// A scenario that retrieves a partner's audit records and filter by customer company name.
    /// </summary>
    public class SearchAuditRecordsByCustomerId : BasePartnerScenario
    {
        /// <summary>
        /// The search field.
        /// </summary>
        private readonly AuditRecordSearchField auditRecordSearchField;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchAuditRecordsByCustomerId"/> class.
        /// </summary>
        /// <param name="title">The scenario title.</param>
        /// <param name="auditRecordSearchField">The search field.</param>
        /// <param name="context">The scenario context.</param>
        public SearchAuditRecordsByCustomerId(string title, AuditRecordSearchField auditRecordSearchField, IScenarioContext context) : base(title, context)
        {
            this.auditRecordSearchField = auditRecordSearchField;
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            IAggregatePartner partnerOperations = this.Context.UserPartnerOperations;

            string customerId = this.Context.ConsoleHelper.ReadNonEmptyString("Enter a Customer Id to search for", "No Customer Id entered");
            DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01);

            this.Context.ConsoleHelper.StartProgress(
                string.Format(CultureInfo.InvariantCulture, "Retrieving the partner's audit records - start date: {0}", startDate));

            SimpleFieldFilter filter = new SimpleFieldFilter(AuditRecordSearchField.CustomerId.ToString(), FieldFilterOperation.Equals, customerId);

            Models.SeekBasedResourceCollection<AuditRecord> auditRecordsPage = partnerOperations.AuditRecords.Query(startDate.Date, query: QueryFactory.Instance.BuildSimpleQuery(filter));

            this.Context.ConsoleHelper.StopProgress();

            // create a customer enumerator which will aid us in traversing the customer pages
            Enumerators.IResourceCollectionEnumerator<Models.SeekBasedResourceCollection<AuditRecord>> auditRecordEnumerator = partnerOperations.Enumerators.AuditRecords.Create(auditRecordsPage);

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
