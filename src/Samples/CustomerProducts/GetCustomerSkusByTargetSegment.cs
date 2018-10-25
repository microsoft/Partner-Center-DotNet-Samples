// -----------------------------------------------------------------------
// <copyright file="GetCustomerSkusByTargetSegment.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.CustomerProducts
{
    using System.Globalization;

    /// <summary>
    /// A scenario that retrieves all the SKUs related to a product that apply to a customer and that target a specific segment.
    /// </summary>
    public class GetCustomerSkusByTargetSegment : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCustomerSkusByTargetSegment"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetCustomerSkusByTargetSegment(IScenarioContext context) : base("Get skus for customer by segment", context)
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
            string segment = this.Context.ConsoleHelper.ReadNonEmptyString("The segment to filter the skus on", "The segment can't be empty");

            this.Context.ConsoleHelper.StartProgress(string.Format(CultureInfo.InvariantCulture, "Getting skus from product {0} by segment {1} for customer {2}", productId, segment, customerId));
            var skus = partnerOperations.Customers.ById(customerId).Products.ById(productId).Skus.ByTargetSegment(segment).Get();
            this.Context.ConsoleHelper.StopProgress();

            this.Context.ConsoleHelper.WriteObject(skus, string.Format(CultureInfo.InvariantCulture, "Skus for customer {0} by segment {1}", productId, segment));
        }
    }
}
