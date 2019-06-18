// -----------------------------------------------------------------------
// <copyright file="GetInvoiceStatement.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Invoice
{
    using System;

    /// <summary>
    /// Gets the invoice statement for an invoice id.
    /// </summary>
    public class GetInvoiceStatement : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetInvoiceStatement"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetInvoiceStatement(IScenarioContext context) : base("Get Invoice Statement by Id", context)
        {
        }

        /// <summary>
        /// executes the get invoice statement scenario.
        /// </summary>
        protected override void RunScenario()
        {
            IAggregatePartner partnerOperations = this.Context.UserPartnerOperations;

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

            this.Context.ConsoleHelper.StartProgress("Getting Invoice Statement");
            // // Retrieving invoice statement for an invoice id
            System.IO.Stream invoiceStatement = partnerOperations.Invoices.ById(invoiceId).Documents.Statement.Get();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(invoiceId, "Invoice Id");
            this.Context.ConsoleHelper.WriteObject(invoiceStatement.Length, "Invoice Statement Size");
        }
    }
}
