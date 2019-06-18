// -----------------------------------------------------------------------
// <copyright file="GetOffer.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Offers
{
    using System.Globalization;

    /// <summary>
    /// A scenario that retrieves offer details supported in a country.
    /// </summary>
    public class GetOffer : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetOffer"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetOffer(IScenarioContext context) : base("Get offer details", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            IAggregatePartner partnerOperations = this.Context.UserPartnerOperations;
            string offerId = this.ObtainOfferId("Enter the ID of offer to retrieve");
            string countryCode = this.Context.ConsoleHelper.ReadNonEmptyString("Enter the 2 digit country code to get its supported offers", "The country code can't be empty");

            this.Context.ConsoleHelper.StartProgress(string.Format(CultureInfo.InvariantCulture, "Getting offer details for {0}", countryCode));

            Models.Offers.Offer offer = partnerOperations.Offers.ByCountry(countryCode).ById(offerId).Get();
            Models.ResourceCollection<Models.Offers.Offer> offerAddOns = partnerOperations.Offers.ByCountry(countryCode).ById(offerId).AddOns.Get();

            this.Context.ConsoleHelper.StopProgress();

            this.Context.ConsoleHelper.WriteObject(offer, string.Format(CultureInfo.InvariantCulture, "Offer in {0}", countryCode));
            this.Context.ConsoleHelper.WriteObject(offerAddOns, string.Format(CultureInfo.InvariantCulture, "Offer add-ons in {0}", countryCode));
        }
    }
}
