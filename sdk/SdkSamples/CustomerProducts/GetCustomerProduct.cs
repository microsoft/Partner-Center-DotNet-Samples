// -----------------------------------------------------------------------
// <copyright file="GetCustomerProduct.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.CustomerProducts
{
    using System.Globalization;

    /// <summary>
    /// A scenario that retrieves product details for a customer.
    /// </summary>
    public class GetCustomerProduct : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCustomerProduct"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetCustomerProduct(IScenarioContext context) : base("Get product for customer", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;
            var customerId = this.ObtainCustomerId("Enter the ID of the corresponding customer");
            var productId = this.ObtainProductId("Enter the ID of the product");

            this.Context.ConsoleHelper.StartProgress(string.Format(CultureInfo.InvariantCulture, "Getting product {0} details for customer {1}", productId, customerId));
            var product = partnerOperations.Customers.ById(customerId).Products.ById(productId).Get();
            this.Context.ConsoleHelper.StopProgress();

            this.Context.ConsoleHelper.WriteObject(product, "Product details for customer");
        }
    }
}
