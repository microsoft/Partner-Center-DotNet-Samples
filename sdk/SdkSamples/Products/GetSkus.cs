// -----------------------------------------------------------------------
// <copyright file="GetSkus.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Products
{
    using System.Globalization;

    /// <summary>
    /// A scenario that retrieves all the SKUs related to a product that are supported in a country.
    /// </summary>
    public class GetSkus : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetSkus"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetSkus(IScenarioContext context) : base("Get skus", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;
            var productId = this.ObtainProductId("Enter the ID of the corresponding product");
            string countryCode = this.Context.ConsoleHelper.ReadNonEmptyString("Enter the 2 digit country code to get its supported skus", "The country code can't be empty");

            this.Context.ConsoleHelper.StartProgress(string.Format(CultureInfo.InvariantCulture, "Getting skus for product {0} in country {1}", productId, countryCode));
            var skus = partnerOperations.Products.ByCountry(countryCode).ById(productId).Skus.Get();
            this.Context.ConsoleHelper.StopProgress();

            this.Context.ConsoleHelper.WriteObject(skus, string.Format(CultureInfo.InvariantCulture, "Skus for product {0}", productId));
        }
    }
}
