// -----------------------------------------------------------------------
// <copyright file="GetCustomerAvailabilitiesByReservationScope.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.CustomerProducts
{
    using System.Globalization;

    /// <summary>
    /// A scenario that retrieves the availabilities of a product's SKU for a customer by reservation scope.
    /// </summary>
    public class GetCustomerAvailabilitiesByReservationScope : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCustomerAvailabilities"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetCustomerAvailabilitiesByReservationScope(IScenarioContext context) : base("Get availabilities for customer by reservation scope", context)
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

            this.Context.ConsoleHelper.StartProgress(string.Format(CultureInfo.InvariantCulture, "Getting availabilities for product {0} and sku {1} for customer {2}", productId, skuId, customerId));
            var skuAvailabilities = partnerOperations.Customers.ById(customerId).Products.ById(productId).Skus.ById(skuId).Availabilities.ByReservationScope("AzurePlan").Get();
            this.Context.ConsoleHelper.StopProgress();

            this.Context.ConsoleHelper.WriteObject(skuAvailabilities, "Availabilities for customer by reservation scope");
        }
    }
}
