// -----------------------------------------------------------------------
// <copyright file="GetInvoice.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Invoice
{
    using System;

    /// <summary>
    /// Gets a single partner invoice.
    /// </summary>
    public class GetInvoice : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetInvoice"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetInvoice(IScenarioContext context) : base("Get partner's invoice details", context)
        {
        }

        /// <summary>
        /// executes the get invoice scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;
            string invoiceId = this.Context.Configuration.Scenario.DefaultInvoiceId;

            if (string.IsNullOrWhiteSpace(invoiceId))
            {
                // prompt the user the enter the invoice ID
                invoiceId = this.Context.ConsoleHelper.ReadNonEmptyString("Please enter the invoice ID to retrieve ", "The invoice ID can't be empty");
            }
            else
            {
                Console.WriteLine("Found Invoice ID: {0} in configuration.", invoiceId);
            }

            this.Context.ConsoleHelper.StartProgress("Retrieving invoice");

            // Retrieving invoice
            var invoice = partnerOperations.Invoices.ById(invoiceId).Get();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(invoice, "Invoice details");

            // Retrieving invoice line items
            if (invoice.InvoiceDetails != null)
            {
                foreach (var invoiceDetail in invoice.InvoiceDetails)
                {
                    this.Context.ConsoleHelper.WriteObject(invoiceDetail, "Invoice Line Items");
                }
            }
        }
    }
}
