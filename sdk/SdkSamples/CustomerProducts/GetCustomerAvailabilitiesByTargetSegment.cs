// -----------------------------------------------------------------------
// <copyright file="GetCustomerAvailabilitiesByTargetSegment.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.CustomerProducts
{
    using System.Globalization;

    /// <summary>
    /// A scenario that retrieves the availabilities of a product's SKU for a customer that target a specific segment.
    /// </summary>
    public class GetCustomerAvailabilitiesByTargetSegment : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCustomerAvailabilitiesByTargetSegment"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetCustomerAvailabilitiesByTargetSegment(IScenarioContext context) : base("Get availabilities for customer by segment", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;
            var customerId = this.ObtainCustomerId("Enter the ID of the corresponding customer");
            var productId = this.ObtainProductId("Enter the ID of the corresponding product");
            var skuId = this.ObtainSkuId("Enter the ID of the corresponding sku");
            string segment = this.Context.ConsoleHelper.ReadNonEmptyString("The segment to filter the availabilities on", "The segment can't be empty");

            this.Context.ConsoleHelper.StartProgress(string.Format(CultureInfo.InvariantCulture, "Getting sku availabilities for product {0} and sku {1} by segment {2} for customer {3}", productId, skuId, segment, customerId));
            var skuAvailabilities = partnerOperations.Customers.ById(customerId).Products.ById(productId).Skus.ById(skuId).Availabilities.ByTargetSegment(segment).Get();
            this.Context.ConsoleHelper.StopProgress();

            this.Context.ConsoleHelper.WriteObject(skuAvailabilities, "Availabilities for customer by segment");
        }
    }
}
