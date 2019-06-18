// -----------------------------------------------------------------------
// <copyright file="GetSkusByTargetSegment.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Products
{
    using System.Globalization;

    /// <summary>
    /// A scenario that retrieves all the SKUs related to a product that are supported in a country and that target a specific segment.
    /// </summary>
    public class GetSkusByTargetSegment : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetSkusByTargetSegment"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetSkusByTargetSegment(IScenarioContext context) : base("Get skus by segment", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            IAggregatePartner partnerOperations = this.Context.UserPartnerOperations;
            string productId = this.ObtainProductId("Enter the ID of the corresponding product");
            string countryCode = this.Context.ConsoleHelper.ReadNonEmptyString("Enter the 2 digit country code to get its supported skus", "The country code can't be empty");
            string segment = this.Context.ConsoleHelper.ReadNonEmptyString("The segment to filter the skus on", "The segment can't be empty");

            this.Context.ConsoleHelper.StartProgress(string.Format(CultureInfo.InvariantCulture, "Getting skus for product {0} in country {1} and in segment {2}", productId, countryCode, segment));
            Models.ResourceCollection<Models.Products.Sku> skus = partnerOperations.Products.ByCountry(countryCode).ById(productId).Skus.ByTargetSegment(segment).Get();
            this.Context.ConsoleHelper.StopProgress();

            this.Context.ConsoleHelper.WriteObject(skus, string.Format(CultureInfo.InvariantCulture, "Skus for product {0} in segment {1}", productId, segment));
        }
    }
}
