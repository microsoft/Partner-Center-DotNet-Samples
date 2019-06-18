// -----------------------------------------------------------------------
// <copyright file="GetInvoiceLineItems.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Invoice
{
    using Microsoft.Store.PartnerCenter.Models.Invoices;
    using Microsoft.Store.PartnerCenter.Models.Query;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

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

                    if (invoiceDetail.BillingProvider.ToString().Equals(BillingProvider.Marketplace.ToString()))
                    {
                        var seekBasedResourceCollection = invoiceOperations.By(invoiceDetail.BillingProvider.ToString(), invoiceDetail.InvoiceLineItemType.ToString(), invoice.CurrencyCode, "current", null).Get();

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
                                        seekBasedResourceCollection = invoiceOperations.By(invoiceDetail.BillingProvider.ToString(), invoiceDetail.InvoiceLineItemType.ToString(), invoice.CurrencyCode, "current", null).Seek(seekBasedResourceCollection.ContinuationToken, SeekOperation.Next);
                                    }
                                }
                            }
                        }
                    }
                    else {
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

                    this.Context.ConsoleHelper.StopProgress();
                    Console.Clear();
                }
            }
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
