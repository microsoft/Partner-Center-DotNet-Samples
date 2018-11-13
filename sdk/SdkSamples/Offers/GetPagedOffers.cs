// -----------------------------------------------------------------------
// <copyright file="GetPagedOffers.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Offers
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Gets a partner offers in pages.
    /// </summary>
    public class GetPagedOffers : BasePartnerScenario
    {
        /// <summary>
        /// The offer page size.
        /// </summary>
        private readonly int offersPageSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetPagedOffers"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        /// <param name="offersPageSize">The number of Offers to return per page.</param>
        public GetPagedOffers(IScenarioContext context, int offersPageSize = 0) : base("Get paged offers", context)
        {
            this.offersPageSize = offersPageSize;
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;
            int offset = 0;

            string countryCode = this.Context.ConsoleHelper.ReadNonEmptyString("Enter the 2 digit country code to get its supported offers", "The country code can't be empty");

            this.Context.ConsoleHelper.StartProgress("Querying Offers");

            // query the Offers, get the first page if a page size was set, otherwise get all Offers
            var offersPage = (this.offersPageSize <= 0) ? partnerOperations.Offers.ByCountry(countryCode).Get() : partnerOperations.Offers.ByCountry(countryCode).Get(offset, this.offersPageSize);
            this.Context.ConsoleHelper.StopProgress();

            // create a customer enumerator which will aid us in traversing the customer pages
            var offersEnumerator = partnerOperations.Enumerators.Offers.Create(offersPage);
            int pageNumber = 1;

            while (offersEnumerator.HasValue)
            {
                // print the current customer results page
                this.Context.ConsoleHelper.WriteObject(offersEnumerator.Current, string.Format(CultureInfo.InvariantCulture, "Offers Page: {0}", pageNumber++));

                Console.WriteLine();
                Console.Write("Press any key to retrieve the next offers page");
                Console.ReadKey();

                this.Context.ConsoleHelper.StartProgress("Getting next offers page");

                // get the next page of Offers
                offersEnumerator.Next();

                this.Context.ConsoleHelper.StopProgress();
                Console.Clear();
            }
        }
    }
}
