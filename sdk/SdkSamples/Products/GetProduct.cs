// -----------------------------------------------------------------------
// <copyright file="GetProduct.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Products
{
    using System.Globalization;

    /// <summary>
    /// A scenario that retrieves product details supported in a country.
    /// </summary>
    public class GetProduct : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetProduct"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetProduct(IScenarioContext context) : base("Get product", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;
            var productId = this.ObtainProductId("Enter the ID of the product");
            string countryCode = this.Context.ConsoleHelper.ReadNonEmptyString("Enter the 2 digit country code of the product", "The country code can't be empty");

            this.Context.ConsoleHelper.StartProgress(string.Format(CultureInfo.InvariantCulture, "Getting details for product {0} in country {1}", productId, countryCode));
            var product = partnerOperations.Products.ByCountry(countryCode).ById(productId).Get();
            this.Context.ConsoleHelper.StopProgress();

            this.Context.ConsoleHelper.WriteObject(product, string.Format(CultureInfo.InvariantCulture, "Product details of {0}", productId));
        }
    }
}
