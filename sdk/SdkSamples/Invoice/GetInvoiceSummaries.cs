// -----------------------------------------------------------------------
// <copyright file="GetInvoiceSummaries.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Invoice
{
    /// <summary>
    /// Gets a single partner invoice summaries.
    /// </summary>
    public class GetInvoiceSummaries : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetInvoiceSummaries"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetInvoiceSummaries(IScenarioContext context) : base("Get partner's invoice summaries", context)
        {
        }

        /// <summary>
        /// executes the get invoice summaries scenario.
        /// </summary>
        protected override void RunScenario()
        {
            IAggregatePartner partnerOperations = this.Context.UserPartnerOperations;
            this.Context.ConsoleHelper.StartProgress("Retrieving invoice summaries");

            // Retrieving invoice summaries
            Models.ResourceCollection<Models.Invoices.InvoiceSummary> invoiceSummaries = partnerOperations.Invoices.Summaries.Get();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(invoiceSummaries, "Invoice Summaries");

            // Retrieving invoice summaries details
            if (invoiceSummaries.TotalCount > 0)
            {
                foreach (Models.Invoices.InvoiceSummary summary in invoiceSummaries.Items)
                {
                    foreach (Models.Invoices.InvoiceSummaryDetail detail in summary.Details)
                    {
                        this.Context.ConsoleHelper.WriteObject(detail, "Invoice Summaries Details");
                    }
                }
            }
        }
    }
}
