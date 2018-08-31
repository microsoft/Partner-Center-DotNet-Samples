// -----------------------------------------------------------------------
// <copyright file="GetAvailability.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Products
{
    using System.Globalization;

    /// <summary>
    /// A scenario that retrieves the availability of a product's SKU.
    /// </summary>
    public class GetAvailability : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetAvailability"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetAvailability(IScenarioContext context) : base("Get availability", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;
            var productId = this.ObtainProductId("Enter the ID of the corresponding product");
            var skuId = this.ObtainSkuId("Enter the ID of the corresponding sku");
            var availabilityId = this.ObtainAvailabilityId("Enter the ID of the availability");
            string countryCode = this.Context.ConsoleHelper.ReadNonEmptyString("Enter the 2 digit country code of the availability", "The country code can't be empty");

            this.Context.ConsoleHelper.StartProgress(string.Format(CultureInfo.InvariantCulture, "Getting availability {0} for product {1} and sku {2} in country {3}", availabilityId, productId, skuId, countryCode));
            var availability = partnerOperations.Products.ByCountry(countryCode).ById(productId).Skus.ById(skuId).Availabilities.ById(availabilityId).Get();
            this.Context.ConsoleHelper.StopProgress();

            this.Context.ConsoleHelper.WriteObject(availability, "Availability ");
        }
    }
}
