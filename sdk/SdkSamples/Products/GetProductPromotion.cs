// -----------------------------------------------------------------------
// <copyright file="GetProductPromotion.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Products
{
    using System.Globalization;

    /// <summary>
    /// A scenario that retrieves product promotion details supported in a country.
    /// </summary>
    public class GetProductPromotion : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetProductPromotion"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetProductPromotion(IScenarioContext context) : base("Get product promotion", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;
            var productPromotionId = this.ObtainProductId("Enter the ID of the product promotion");
            string countryCode = this.Context.ConsoleHelper.ReadNonEmptyString("Enter the 2 digit country code of the product", "The country code can't be empty");

            this.Context.ConsoleHelper.StartProgress(string.Format(CultureInfo.InvariantCulture, "Getting details for product promotion {0} in country {1}", productPromotionId, countryCode));
            var productPromotion = partnerOperations.ProductPromotions.ByCountry(countryCode).ById(productPromotionId).Get();
            this.Context.ConsoleHelper.StopProgress();

            this.Context.ConsoleHelper.WriteObject(productPromotion, string.Format(CultureInfo.InvariantCulture, "Product promotion details of {0}", productPromotionId));
        }
    }
}