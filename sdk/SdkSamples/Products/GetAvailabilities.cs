// -----------------------------------------------------------------------
// <copyright file="GetAvailabilities.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Products
{
    using System.Globalization;

    /// <summary>
    /// A scenario that retrieves the availabilities of a product's SKU.
    /// </summary>
    public class GetAvailabilities : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetAvailabilities"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetAvailabilities(IScenarioContext context) : base("Get availabilities", context)
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
            string countryCode = this.Context.ConsoleHelper.ReadNonEmptyString("Enter the 2 digit country code of the sku", "The country code can't be empty");

            this.Context.ConsoleHelper.StartProgress(string.Format(CultureInfo.InvariantCulture, "Getting availabilities for product {0} and sku {1} in country {2}", productId, skuId, countryCode));
            var skuAvailabilities = partnerOperations.Products.ByCountry(countryCode).ById(productId).Skus.ById(skuId).Availabilities.Get();
            this.Context.ConsoleHelper.StopProgress();

            this.Context.ConsoleHelper.WriteObject(skuAvailabilities, string.Format(CultureInfo.InvariantCulture, "Availabilities for product {0}", productId));
        }
    }
}
