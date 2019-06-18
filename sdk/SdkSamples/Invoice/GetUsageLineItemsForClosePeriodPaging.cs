// -----------------------------------------------------------------------
// <copyright file="GetUsageLineItemsForClosePeriodPaging.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Invoice
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Microsoft.Store.PartnerCenter.Models.Invoices;
    using Microsoft.Store.PartnerCenter.Models.Query;
    using Microsoft.Store.PartnerCenter.RequestContext;

    /// <summary>
    /// Get unbilled recon line items 
    /// </summary>
    public class GetUsageLineItemsForClosePeriodPaging : BasePartnerScenario
    {
        /// <summary>
        /// Unbilled - First Party and Marketplace - Recon Line Items Paging
        /// </summary>
        private readonly int invoicePageSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetBillingLineItemsForOpenPeriodPaging"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        /// <param name="invoicePageSize">invoice Page Size.</param>
        public GetUsageLineItemsForClosePeriodPaging(IScenarioContext context, int invoicePageSize) : base("Billed - Consumption - Reconciliation Line Items Paging", context)
        {
            this.invoicePageSize = invoicePageSize;
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

            var pageMaxSizeReconciliationLineItems = 2000;

            IPartner scopedPartnerOperations = partnerOperations.With(RequestContextFactory.Instance.Create(Guid.NewGuid()));

            this.Context.ConsoleHelper.StartProgress("Getting billed consumption reconciliation line items");
            // Retrieving billed consumption line items
            var seekBasedResourceCollection = scopedPartnerOperations.Invoices.ById(invoiceId).By("marketplace", "usagelineitems", null, null, pageMaxSizeReconciliationLineItems).Get();

            var fetchNext = true;

            ConsoleKeyInfo keyInfo;

            var itemNumber = 1;

            Console.Out.WriteLine("\tRecon line items count: " + seekBasedResourceCollection.Items.Count());

            if (seekBasedResourceCollection.Items.Count() > 0)
            {
                while (fetchNext)
                {
                    seekBasedResourceCollection.Items.Take(2).ToList().ForEach(i =>
                    {
                        Console.Out.WriteLine("\t----------------------------------------------");
                        Console.Out.WriteLine("\tLine Item # {0}", itemNumber);

                        PrintProperties(i);
                        itemNumber++;
                    });

                    Console.Out.WriteLine("\tPress any key to fetch next data. Press the Escape (Esc) key to quit: \n");
                    keyInfo = Console.ReadKey();

                    if (keyInfo.Key == ConsoleKey.Escape)
                    {
                        break;
                    }

                    fetchNext = !string.IsNullOrWhiteSpace(seekBasedResourceCollection.ContinuationToken);

                    if (fetchNext)
                    {
                        if (seekBasedResourceCollection.Links.Next.Headers != null && seekBasedResourceCollection.Links.Next.Headers.Any())
                        {
                            seekBasedResourceCollection = scopedPartnerOperations.Invoices.ById(invoiceId).By("marketplace", "usagelineitems", null, null, pageMaxSizeReconciliationLineItems).Seek(seekBasedResourceCollection.ContinuationToken, SeekOperation.Next);
                        }
                    }
                }
            }

            this.Context.ConsoleHelper.StopProgress();
        }

        /// <summary>
        /// Prints an invoice line item properties.
        /// </summary>
        /// <param name="item">the invoice line item.</param>
        private static void PrintProperties(InvoiceLineItem item)
        {
            Type t = null;

            if (item is DailyRatedUsageLineItem)
            {
                t = typeof(DailyRatedUsageLineItem);
                Console.Out.WriteLine(" ");
                Console.Out.WriteLine("\tMarketplace Daily Rated Usage Line Items: ");
            }
            else if (item is OneTimeInvoiceLineItem)
            {
                t = typeof(OneTimeInvoiceLineItem);
                Console.Out.WriteLine(" ");
                Console.Out.WriteLine("\tFirst Party And Marketplace Recon Line Items: ");
            }

            PropertyInfo[] properties = t.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                Console.Out.WriteLine(string.Format("\t{0,-30}|{1,-50}", property.Name, property.GetValue(item, null).ToString()));
            }
        }
    }
}