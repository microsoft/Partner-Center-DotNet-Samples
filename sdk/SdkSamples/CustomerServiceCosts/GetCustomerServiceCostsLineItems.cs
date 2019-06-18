// -----------------------------------------------------------------------
// <copyright file="GetCustomerServiceCostsLineItems.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.CustomerServiceCosts
{
    using Models.ServiceCosts;

    /// <summary>
    /// Gets Customer Service Costs Line Items.
    /// </summary>
    public class GetCustomerServiceCostsLineItems : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCustomerServiceCostsLineItems"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetCustomerServiceCostsLineItems(IScenarioContext context) : base("Get customer service costs line items", context)
        {
        }

        /// <summary>
        /// Executes the get Customer Service Costs Line Items.
        /// </summary>
        protected override void RunScenario()
        {
            // get a customer Id.
            string selectedCustomerId = this.ObtainCustomerId("Enter the ID of the customer to get service costs line items");

            IAggregatePartner partnerOperations = this.Context.UserPartnerOperations;

            this.Context.ConsoleHelper.StartProgress("Getting customer service costs line items");

            // get the customer's Service Costs Line Items.
            Models.ResourceCollection<ServiceCostLineItem> customerServiceCostsLineItems = partnerOperations.Customers.ById(selectedCustomerId).ServiceCosts.ByBillingPeriod(ServiceCostsBillingPeriod.MostRecent).LineItems.Get();
            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(customerServiceCostsLineItems, "Customer Service Costs Line Items");
        }
    }
}
