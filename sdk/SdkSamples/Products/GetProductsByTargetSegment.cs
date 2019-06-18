// -----------------------------------------------------------------------
// <copyright file="GetProductsByTargetSegment.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Products
{
    using System.Globalization;

    /// <summary>
    /// A scenario that retrieves all the products supported in a country, in a catalog view and in a segment.
    /// </summary>
    public class GetProductsByTargetSegment : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetProductsByTargetSegment"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetProductsByTargetSegment(IScenarioContext context) : base("Get products by segment", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            IAggregatePartner partnerOperations = this.Context.UserPartnerOperations;
            string countryCode = this.Context.ConsoleHelper.ReadNonEmptyString("Enter the 2 digit country code to get its supported products", "The country code can't be empty");
            string targetView = this.Context.ConsoleHelper.ReadNonEmptyString("Enter the target view to get its supported products", "The target view can't be empty");
            string segment = this.Context.ConsoleHelper.ReadNonEmptyString("The segment to filter the products on", "The segment can't be empty");

            this.Context.ConsoleHelper.StartProgress(string.Format(CultureInfo.InvariantCulture, "Getting products in catalog view {0}, in country {1} and in segment {2}", targetView, countryCode, segment));
            Models.ResourceCollection<Models.Products.Product> products = partnerOperations.Products.ByCountry(countryCode).ByTargetView(targetView).ByTargetSegment(segment).Get();
            this.Context.ConsoleHelper.StopProgress();

            this.Context.ConsoleHelper.WriteObject(products, string.Format(CultureInfo.InvariantCulture, "Products in catalog view {0} by segment {1}", targetView, segment));
        }
    }
}
