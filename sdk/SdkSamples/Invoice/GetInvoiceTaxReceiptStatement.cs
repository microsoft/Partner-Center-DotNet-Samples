// -----------------------------------------------------------------------
// <copyright file="GetInvoiceTaxReceiptStatement.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Invoice
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    /// <summary>
    /// Gets a single partner invoice.
    /// </summary>
    public class GetInvoiceTaxReceiptStatement : BasePartnerScenario 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetInvoiceTaxReceiptStatement"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetInvoiceTaxReceiptStatement(IScenarioContext context) : base("Get partner's tax receipt statement by Id", context)
        {
        }

        /// <summary>
        /// executes the get invoice tax receipt scenario.
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

            string receiptId = this.Context.Configuration.Scenario.DefaultReceiptId;

            if (string.IsNullOrWhiteSpace(receiptId))
            {
                // prompt the user the enter the receipt ID
                receiptId = this.Context.ConsoleHelper.ReadNonEmptyString("Please enter the receipt ID to retrieve ", "The receipt ID can't be empty");
            }
            else
            {
                Console.WriteLine("Found Receipt ID: {0} in configuration.", receiptId);
            }

            this.Context.ConsoleHelper.StartProgress("Retrieving tax receipt statement");

            // Retrieving invoice
            var taxReceiptStatement = partnerOperations.Invoices.ById(invoiceId).Receipts.ById(receiptId).Documents.Statement.Get();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(receiptId, "Receipt Id");
            this.Context.ConsoleHelper.WriteObject(taxReceiptStatement.Length, "Invoice tax receipt Statement Size");
        }

    }
}
