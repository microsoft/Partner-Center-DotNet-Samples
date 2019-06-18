// -----------------------------------------------------------------------
// <copyright file="GetSku.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Products
{
    using System.Globalization;

    /// <summary>
    /// A scenario that retrieves details of a product's SKU.
    /// </summary>
    public class GetSku : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetSku"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetSku(IScenarioContext context) : base("Get sku", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            IAggregatePartner partnerOperations = this.Context.UserPartnerOperations;
            string productId = this.ObtainProductId("Enter the ID of the corresponding product");
            string skuId = this.ObtainSkuId("Enter the ID of the sku");
            string countryCode = this.Context.ConsoleHelper.ReadNonEmptyString("Enter the 2 digit country code of the sku", "The country code can't be empty");

            this.Context.ConsoleHelper.StartProgress(string.Format(CultureInfo.InvariantCulture, "Getting sku details for sku {0} and product {1} in country {2}", skuId, productId, countryCode));
            Models.Products.Sku sku = partnerOperations.Products.ByCountry(countryCode).ById(productId).Skus.ById(skuId).Get();
            this.Context.ConsoleHelper.StopProgress();

            this.Context.ConsoleHelper.WriteObject(sku, string.Format(CultureInfo.InvariantCulture, "Sku details of {0}", skuId));
        }
    }
}
