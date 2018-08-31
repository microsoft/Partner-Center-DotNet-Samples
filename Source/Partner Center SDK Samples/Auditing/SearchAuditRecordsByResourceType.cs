// -----------------------------------------------------------------------
// <copyright file="SearchAuditRecordsByResourceType.cs" company="Microsoft">
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
    public class SearchAuditRecordsByResourceType : BasePartnerScenario
    {
        /// <summary>
        /// The search field.
        /// </summary>
        private readonly AuditRecordSearchField auditRecordSearchField;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchAuditRecordsByResourceType"/> class.
        /// </summary>
        /// <param name="title">The scenario title.</param>
        /// <param name="auditRecordSearchField">The search field.</param>
        /// <param name="context">The scenario context.</param>
        public SearchAuditRecordsByResourceType(string title, AuditRecordSearchField auditRecordSearchField, IScenarioContext context) : base(title, context)
        {
            this.auditRecordSearchField = auditRecordSearchField;
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;

            string[] resourceTypes = Enum.GetNames(typeof(ResourceType));

            int resourceTypesMaxIndex;

            for (resourceTypesMaxIndex = 0; resourceTypesMaxIndex < resourceTypes.Length; resourceTypesMaxIndex++)
            {
                Console.WriteLine(resourceTypesMaxIndex.ToString() + ": " + resourceTypes[resourceTypesMaxIndex]);
            }

            // decrement max index by 1 to account for zero-based start
            resourceTypesMaxIndex--;

            Console.WriteLine();

            while (true)
            {
                string resourceType = this.Context.ConsoleHelper.ReadNonEmptyString(
                    "Enter the number corresponding to the Resource Type to search for",
                    string.Format(CultureInfo.InvariantCulture, "Please enter a number between 0 and {0}", resourceTypesMaxIndex));

                int resourceTypeInt = -1;

                if (int.TryParse(resourceType, out resourceTypeInt))
                {
                    if (resourceTypeInt < 0 || resourceTypeInt > resourceTypesMaxIndex)
                    {
                        this.Context.ConsoleHelper.Error(
                            string.Format(CultureInfo.InvariantCulture, "The number must be between 0 and {0}", resourceTypesMaxIndex));

                        break;
                    }
                }
                else
                {
                    this.Context.ConsoleHelper.Error(
                        string.Format(CultureInfo.InvariantCulture, "The number must be a valid integer between 0 and {0}", resourceTypesMaxIndex));

                    break;
                }

                var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01);

                this.Context.ConsoleHelper.StartProgress(
                    string.Format(CultureInfo.InvariantCulture, "Retrieving the partner's audit records - start date: {0}", startDate));

                string searchField = Enum.GetName(typeof(ResourceType), resourceTypeInt);
                var filter = new SimpleFieldFilter(AuditRecordSearchField.ResourceType.ToString(), FieldFilterOperation.Equals, searchField);

                var auditRecordsPage = partnerOperations.AuditRecords.Query(startDate.Date, query: QueryFactory.Instance.BuildSimpleQuery(filter));

                this.Context.ConsoleHelper.StopProgress();

                // create a customer enumerator which will aid us in traversing the customer pages
                var auditRecordEnumerator = partnerOperations.Enumerators.AuditRecords.Create(auditRecordsPage);

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

                break;
            }
        }
    }
}
