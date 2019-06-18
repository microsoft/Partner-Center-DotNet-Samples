// -----------------------------------------------------------------------
// <copyright file="GetOffers.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Offers
{
    using System.Globalization;

    /// <summary>
    /// A scenario that retrieves all the offers supported in a country.
    /// </summary>
    public class GetOffers : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetOffers"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetOffers(IScenarioContext context) : base("Get offers", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            IAggregatePartner partnerOperations = this.Context.UserPartnerOperations;
            int pageSize = this.Context.Configuration.Scenario.DefaultOfferPageSize;
            string countryCode = this.Context.ConsoleHelper.ReadNonEmptyString("Enter the 2 digit country code to get its supported offers", "The country code can't be empty");

            this.Context.ConsoleHelper.StartProgress(string.Format(CultureInfo.InvariantCulture, "Getting first {0} offers for {1}", pageSize, countryCode));
            Models.ResourceCollection<Models.Offers.Offer> offers = partnerOperations.Offers.ByCountry(countryCode).Get(0, pageSize);
            this.Context.ConsoleHelper.StopProgress();

            this.Context.ConsoleHelper.WriteObject(offers, string.Format(CultureInfo.InvariantCulture, "Offers in {0}", countryCode));
        }
    }
}
