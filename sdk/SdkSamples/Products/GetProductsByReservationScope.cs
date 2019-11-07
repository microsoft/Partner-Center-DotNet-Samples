// -----------------------------------------------------------------------
// <copyright file="GetProductsByReservationScope.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Products
{
    using System.Globalization;

    /// <summary>
    /// A scenario that retrieves all the products supported in a country, in a catalog view and for a reservation scope.
    /// </summary>
    public class GetProductsByReservationScope : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetProductsByTargetSegment"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetProductsByReservationScope(IScenarioContext context) : base("Get products by reservation scope", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;
            string countryCode = this.Context.ConsoleHelper.ReadNonEmptyString("Enter the 2 digit country code to get its supported products", "The country code can't be empty");
            string targetView = this.Context.ConsoleHelper.ReadNonEmptyString("Enter the target view to get its supported products", "The target view can't be empty");

            this.Context.ConsoleHelper.StartProgress(string.Format(CultureInfo.InvariantCulture, "Getting products in catalog view {0}, in country {1}", targetView, countryCode));
            var products = partnerOperations.Products.ByCountry(countryCode).ByTargetView(targetView).ByReservationScope("AzurePlan").Get();
            this.Context.ConsoleHelper.StopProgress();

            this.Context.ConsoleHelper.WriteObject(products, string.Format(CultureInfo.InvariantCulture, "Products in catalog view {0}", targetView));
        }
    }
}
