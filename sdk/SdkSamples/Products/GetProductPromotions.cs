// -----------------------------------------------------------------------
// <copyright file="GetProductPromotions.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Products
{
    using System.Globalization;

    /// <summary>
    /// A scenario that retrieves all the product promotions supported in a country and in a segment.
    /// </summary>
    public class GetProductPromotions : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetProductPromotions"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetProductPromotions(IScenarioContext context) : base("Get product promotions", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;
            string countryCode = this.Context.ConsoleHelper.ReadNonEmptyString("Enter the 2 digit country code to get its supported products", "The country code can't be empty");
            string segment = this.Context.ConsoleHelper.ReadNonEmptyString("Enter the segment to get its supported product promotions", "The segment can't be empty");

            this.Context.ConsoleHelper.StartProgress(string.Format(CultureInfo.InvariantCulture, "Getting product promotions in segment {0} in country {1}", segment, countryCode));
            var productPromotions = partnerOperations.ProductPromotions.ByCountry(countryCode).BySegment(segment).Get();
            this.Context.ConsoleHelper.StopProgress();

            this.Context.ConsoleHelper.WriteObject(productPromotions, string.Format(CultureInfo.InvariantCulture, "Product promotions in segment {0}", segment));
        }
    }
}