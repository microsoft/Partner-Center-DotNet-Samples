// -----------------------------------------------------------------------
// <copyright file="GetSkuDownloadOptions.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Products
{
    using System.Globalization;

    /// <summary>
    /// A scenario that retrieves the download options of a product's SKU.
    /// </summary>
    public class GetSkuDownloadOptions : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetSkuDownloadOptions"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetSkuDownloadOptions(IScenarioContext context) : base("Get sku download options", context)
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

            this.Context.ConsoleHelper.StartProgress(string.Format(CultureInfo.InvariantCulture, "Getting sku download options for product {0} and sku {1} in country {2}", productId, skuId, countryCode));
            var sku = partnerOperations.Products.ByCountry(countryCode).ById(productId).Skus.ById(skuId).DownloadOptions.Get();
            this.Context.ConsoleHelper.StopProgress();

            this.Context.ConsoleHelper.WriteObject(sku, string.Format(CultureInfo.InvariantCulture, "Download options for sku {0}", skuId));
        }
    }
}
