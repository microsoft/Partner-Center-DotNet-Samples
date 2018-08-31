// -----------------------------------------------------------------------
// <copyright file="GetCustomerSkus.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.CustomerProducts
{
    using System.Globalization;

    /// <summary>
    /// A scenario that retrieves all the SKUs related to a product that apply to a customer.
    /// </summary>
    public class GetCustomerSkus : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCustomerSkus"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetCustomerSkus(IScenarioContext context) : base("Get skus for customer", context)
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

            this.Context.ConsoleHelper.StartProgress(string.Format(CultureInfo.InvariantCulture, "Getting skus from product {0} for customer {1}", productId, customerId));
            var skus = partnerOperations.Customers.ById(customerId).Products.ById(productId).Skus.Get();
            this.Context.ConsoleHelper.StopProgress();

            this.Context.ConsoleHelper.WriteObject(skus, "Skus for customer");
        }
    }
}
