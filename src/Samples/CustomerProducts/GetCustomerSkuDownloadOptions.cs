// -----------------------------------------------------------------------
// <copyright file="GetCustomerSkuDownloadOptions.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.CustomerProducts
{
    using System.Globalization;

    /// <summary>
    /// A scenario that retrieves the download options of a product's SKU for a customer.
    /// </summary>
    public class GetCustomerSkuDownloadOptions : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCustomerSkuDownloadOptions"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetCustomerSkuDownloadOptions(IScenarioContext context) : base("Get sku download options for customer", context)
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

            this.Context.ConsoleHelper.StartProgress(string.Format(CultureInfo.InvariantCulture, "Getting sku download options for sku {0} from product {1} for customer {2}", skuId, productId, customerId));
            var sku = partnerOperations.Customers.ById(customerId).Products.ById(productId).Skus.ById(skuId).DownloadOptions.Get();
            this.Context.ConsoleHelper.StopProgress();

            this.Context.ConsoleHelper.WriteObject(sku, "Sku download options for customer");
        }
    }
}
