// -----------------------------------------------------------------------
// <copyright file="GetInvoiceLineItems.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Invoice
{
    using System;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// Gets an invoice's line items.
    /// </summary>
    public class GetInvoiceLineItems : BasePartnerScenario
    {
        /// <summary>
        /// The invoice page size.
        /// </summary>
        private readonly int invoicePageSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetInvoiceLineItems"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        /// <param name="invoicePageSize">The number of invoice line items to return per page.</param>
        public GetInvoiceLineItems(IScenarioContext context, int invoicePageSize = 0) : base("Get an invoice's line items", context)
        {
            this.invoicePageSize = invoicePageSize;
        }

        /// <summary>
        /// executes the get invoice line items scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;
            string invoiceId = this.Context.Configuration.Scenario.DefaultInvoiceId;

            if (string.IsNullOrWhiteSpace(invoiceId))
            {
                // prompt the user the enter the invoice ID
                invoiceId = this.Context.ConsoleHelper.ReadNonEmptyString("Please enter the ID of the invoice to retrieve ", "The ID can't be empty");
            }
            else
            {
                Console.WriteLine("Found Invoice ID: {0} in configuration.", invoiceId);
            }

            this.Context.ConsoleHelper.StartProgress("Retrieving Invoice Line Items");

            // Retrieve the invoice 
            var invoiceOperations = partnerOperations.Invoices.ById(invoiceId);
            var invoice = invoiceOperations.Get();

            this.Context.ConsoleHelper.StopProgress();

            if ((invoice.InvoiceDetails == null) || (invoice.InvoiceDetails.Count() <= 0))
            {
                this.Context.ConsoleHelper.Warning(string.Format(CultureInfo.InvariantCulture, "Invoice {0} does not have any invoice line items", invoice.Id));
            }
            else
            {
                foreach (var invoiceDetail in invoice.InvoiceDetails)
                {
                    this.Context.ConsoleHelper.StartProgress(string.Format("Getting invoice line item for product {0} and line item type {1}", invoiceDetail.BillingProvider, invoiceDetail.InvoiceLineItemType));

                    // Get the invoice line items
                    var invoiceLineItemsCollection = (this.invoicePageSize <= 0) ? invoiceOperations.By(invoiceDetail.BillingProvider, invoiceDetail.InvoiceLineItemType).Get() : invoiceOperations.By(invoiceDetail.BillingProvider, invoiceDetail.InvoiceLineItemType).Get(this.invoicePageSize, 0);

                    var invoiceLineItemEnumerator = partnerOperations.Enumerators.InvoiceLineItems.Create(invoiceLineItemsCollection);
                    this.Context.ConsoleHelper.StopProgress();
                    int pageNumber = 1;

                    while (invoiceLineItemEnumerator.HasValue)
                    {
                        this.Context.ConsoleHelper.WriteObject(invoiceLineItemEnumerator.Current, string.Format(CultureInfo.InvariantCulture, "Invoice Line Item Page: {0}", pageNumber++));
                        Console.WriteLine();
                        Console.Write("Press any key to retrieve the next invoice line items page");
                        Console.ReadKey();

                        this.Context.ConsoleHelper.StartProgress("Getting next invoice line items page");

                        // Get the next list of invoice line items
                        invoiceLineItemEnumerator.Next();

                        this.Context.ConsoleHelper.StopProgress();
                        Console.Clear();
                    }
                }
            }
        }
    }
}
