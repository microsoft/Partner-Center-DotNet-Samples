// -----------------------------------------------------------------------
// <copyright file="GetEstimatesLinks.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Invoice
{
    using Microsoft.Store.PartnerCenter.Models.Invoices;
    using System;

    /// <summary>
    /// Gets estimate links.
    /// </summary>
    public class GetEstimatesLinks : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetEstimatesLinks"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetEstimatesLinks(IScenarioContext context) : base("Get Estimates links", context)
        {
        }

        /// <summary>
        /// executes the get invoice scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;

            string selectedCurencyCode = this.Context.Configuration.Scenario.DefaultCurrencyCode;
            if (string.IsNullOrWhiteSpace(selectedCurencyCode))
            {
                // prompt the user the enter the currency code
                selectedCurencyCode = this.Context.ConsoleHelper.ReadNonEmptyString("Please enter 3 digit currency code to retrieve the estimates links ", "The currency code can't be empty");
            }
            else
            {
                Console.WriteLine("Found Currency code: {0} in configuration.", selectedCurencyCode);
            }

            // Retrieving estimates links
            this.Context.ConsoleHelper.StartProgress("Retrieving estimates links");
            var estimatesLinks = partnerOperations.Invoices.Estimates.Links.ByCurrency(selectedCurencyCode).Get();

            if (estimatesLinks != null && estimatesLinks.Items != null)
            {
                this.Context.ConsoleHelper.WriteObject(estimatesLinks, "Estimates links");
                foreach (var estimateLink in estimatesLinks.Items)
                {
                    Console.Out.WriteLine("\t--------------------------------------------------------------------------------------------");
                    Console.Out.WriteLine("     \tBilling Provider:             {0}", estimateLink.Title);
                    Console.Out.WriteLine("     \tDescription:                  {0}", estimateLink.Description);
                    Console.Out.WriteLine("     \tPeriod:                       {0}", estimateLink.Period);
                    Console.Out.WriteLine("     \tUri:                          {0}", estimateLink.Link.Uri);
                }
            }
            else
            {
                Console.Out.WriteLine("\tNo estimate links found.");
            }

            this.Context.ConsoleHelper.StopProgress();
        }
    }
}
