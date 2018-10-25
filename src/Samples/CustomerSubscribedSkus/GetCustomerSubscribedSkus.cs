// -----------------------------------------------------------------------
// <copyright file="GetCustomerSubscribedSkus.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.CustomerSubscribedSkus
{
    using System;
    
    /// <summary>
    /// Gets Customer Subscribed SKUs details.
    /// </summary>
    public class GetCustomerSubscribedSkus : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCustomerSubscribedSkus"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetCustomerSubscribedSkus(IScenarioContext context) : base("Get customer subscribed SKUs", context)
        {
        }

        /// <summary>
        /// Executes the get Customer Subscribed SKUs scenario.
        /// </summary>
        protected override void RunScenario()
        {
            // get customer Id.
            string selectedCustomerId = this.ObtainCustomerId("Enter the ID of the customer to get subscribed skus");

            var partnerOperations = this.Context.UserPartnerOperations;

            this.Context.ConsoleHelper.StartProgress("Getting customer subscribed SKUs");

            // get Customer Subscribed SKUs information.
            var customerSubscribedSkus = partnerOperations.Customers.ById(selectedCustomerId).SubscribedSkus.Get();
            this.Context.ConsoleHelper.StopProgress();

            Console.Out.WriteLine("Customer Subscribed Skus Count: " + customerSubscribedSkus.TotalCount);
            this.Context.ConsoleHelper.WriteObject(customerSubscribedSkus, "Customer Subscribed Sku");
        }
    }
}
