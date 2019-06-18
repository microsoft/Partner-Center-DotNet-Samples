// -----------------------------------------------------------------------
// <copyright file="GetCustomerAvailability.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.CustomerProducts
{
    using System.Globalization;

    /// <summary>
    /// A scenario that retrieves the availability of a product's SKU for a customer.
    /// </summary>
    public class GetCustomerAvailability : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCustomerAvailability"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetCustomerAvailability(IScenarioContext context) : base("Get availability for customer", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            IAggregatePartner partnerOperations = this.Context.UserPartnerOperations;
            string customerId = this.ObtainCustomerId("Enter the ID of the corresponding customer");
            string productId = this.ObtainProductId("Enter the ID of the corresponding product");
            string skuId = this.ObtainSkuId("Enter the ID of the corresponding sku");
            string availabilityId = this.ObtainAvailabilityId("Enter the ID of the availability");

            this.Context.ConsoleHelper.StartProgress(string.Format(CultureInfo.InvariantCulture, "Getting availability {0} for product {1} and sku {2} for customer {3}", availabilityId, productId, skuId, customerId));
            Models.Products.Availability skuAvailability = partnerOperations.Customers.ById(customerId).Products.ById(productId).Skus.ById(skuId).Availabilities.ById(availabilityId).Get();
            this.Context.ConsoleHelper.StopProgress();

            this.Context.ConsoleHelper.WriteObject(skuAvailability, "Availability for customer");
        }
    }
}
