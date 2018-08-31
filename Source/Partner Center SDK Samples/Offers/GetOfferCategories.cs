// -----------------------------------------------------------------------
// <copyright file="GetOfferCategories.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Offers
{
    using System.Globalization;

    /// <summary>
    /// A scenario that gets all offer categories.
    /// </summary>
    public class GetOfferCategories : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetOfferCategories"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetOfferCategories(IScenarioContext context) : base("Get offer categories", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;
            string countryCode = this.Context.ConsoleHelper.ReadNonEmptyString("Enter the 2 digit country code to get its supported offer categories", "The country code can't be empty");

            this.Context.ConsoleHelper.StartProgress(string.Format(CultureInfo.InvariantCulture, "Getting offer categories for {0}", countryCode));
            var offerCategories = partnerOperations.OfferCategories.ByCountry(countryCode).Get();
            this.Context.ConsoleHelper.StopProgress();

            this.Context.ConsoleHelper.WriteObject(offerCategories, string.Format(CultureInfo.InvariantCulture, "Offer categories in {0}", countryCode));
        }
    }
}
