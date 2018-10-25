// -----------------------------------------------------------------------
// <copyright file="GetPagedInvoices.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Invoice
{
    using System;
    using System.Globalization;
    using Store.PartnerCenter.Models.Query;

    /// <summary>
    /// Gets paged invoices for partners.
    /// </summary>
    public class GetPagedInvoices : BasePartnerScenario
    {
        /// <summary>
        /// The Invoice page size.
        /// </summary>
        private readonly int invoicePageSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetPagedInvoices"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        /// <param name="invoicePageSize">The number of invoices to fetch per page.</param>
        public GetPagedInvoices(IScenarioContext context, int invoicePageSize) : base("Get paged invoices", context)
        {
            this.invoicePageSize = invoicePageSize;
        }

        /// <summary>
        /// executes the get paged invoices scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;
            this.Context.ConsoleHelper.StartProgress("Querying invoices");

            // query the invoices, get the first page if a page size was set, otherwise get all invoices
            var invoicesPage = (this.invoicePageSize <= 0) ? partnerOperations.Invoices.Get() : partnerOperations.Invoices.Query(QueryFactory.Instance.BuildIndexedQuery(this.invoicePageSize));
            this.Context.ConsoleHelper.StopProgress();

            // create an invoice enumerator which will aid us in traversing the invoice pages
            var invoicesEnumerator = partnerOperations.Enumerators.Invoices.Create(invoicesPage);
            int pageNumber = 1;

            while (invoicesEnumerator.HasValue)
            {
                // print the current invoice results page
                this.Context.ConsoleHelper.WriteObject(invoicesEnumerator.Current, string.Format(CultureInfo.InvariantCulture, "Invoice Page: {0}", pageNumber++));

                Console.WriteLine();
                Console.Write("Press any key to retrieve the next invoices page");
                Console.ReadKey();

                this.Context.ConsoleHelper.StartProgress("Getting next invoices page");

                // get the next page of invoices
                invoicesEnumerator.Next();

                this.Context.ConsoleHelper.StopProgress();
                Console.Clear();
            }

            this.Context.ConsoleHelper.WriteObject(invoicesPage, "Invoices");
        }
    }
}
