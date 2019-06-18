// -----------------------------------------------------------------------
// <copyright file="GetUsageLineItemsForOpenPeriodPaging.cs" company="Microsoft">
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
    public class GetUsageLineItemsForOpenPeriodPaging : BasePartnerScenario
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
        public GetUsageLineItemsForOpenPeriodPaging(IScenarioContext context, int invoicePageSize) : base("Unbilled - Consumption - Reconciliation Line Items Paging", context)
        {
            this.invoicePageSize = invoicePageSize;
        }

        /// <summary>
        /// executes the get invoice scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;
            string curencyCode = this.Context.Configuration.Scenario.DefaultCurrencyCode;
            if (string.IsNullOrWhiteSpace(curencyCode))
            {
                // prompt the user the enter the currency code
                curencyCode = this.Context.ConsoleHelper.ReadNonEmptyString("Please enter 3 digit currency code to retrieve the unbilled consumption reconciliation line items ", "The currency code can't be empty");
            }
            else
            {
                Console.WriteLine("Found Currency code: {0} in configuration.", curencyCode);
            }

            var pageMaxSizeReconciliationLineItems = 2000;
            var period = "current";

            IPartner scopedPartnerOperations = partnerOperations.With(RequestContextFactory.Instance.Create(Guid.NewGuid()));

            this.Context.ConsoleHelper.StartProgress("Getting unbilled consumption reconciliation line items");
            // Retrieving unbilled consumption line items

            var seekBasedResourceCollection = scopedPartnerOperations.Invoices.ById("unbilled").By("marketplace", "usagelineitems", curencyCode, period, pageMaxSizeReconciliationLineItems).Get();

            var fetchNext = true;

            ConsoleKeyInfo keyInfo;

            var itemNumber = 1;

            Console.Out.WriteLine("\tRecon line items count: " + seekBasedResourceCollection.Items.Count());
            Console.Out.WriteLine("\tPeriod: " + period);

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
                            seekBasedResourceCollection = scopedPartnerOperations.Invoices.ById("unbilled").By("marketplace", "usagelineitems", curencyCode, period, pageMaxSizeReconciliationLineItems).Seek(seekBasedResourceCollection.ContinuationToken, SeekOperation.Next);
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