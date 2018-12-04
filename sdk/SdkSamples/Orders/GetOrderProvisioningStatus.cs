// -----------------------------------------------------------------------
// <copyright file="GetOrderProvisioningStatus.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Orders
{
    /// <summary>
    /// A scenario that retrieves a customer order details.
    /// </summary>
    public class GetOrderProvisioningStatus : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetOrderProvisioningStatus"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetOrderProvisioningStatus(IScenarioContext context) : base("Get customer order provisioning status", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;

            string customerId = this.ObtainCustomerId("Enter the ID of the customer whom to retrieve their order provisioning details");
            string orderId = this.ObtainOrderID("Enter the ID of order to retrieve");

            this.Context.ConsoleHelper.StartProgress("Retrieving customer order provisioning status");
            var customerOrder = partnerOperations.Customers.ById(customerId).Orders.ById(orderId).Get();
            var provisioningStatusList = partnerOperations.Customers.ById(customerId).Orders.ById(customerOrder.Id).ProvisioningStatus.Get();

            this.Context.ConsoleHelper.StopProgress();
            
            foreach (var provisioningStatus in provisioningStatusList.Items)
            {
                foreach (var orderItem in customerOrder.LineItems)
                {
                    if (orderItem.LineItemNumber == provisioningStatus.LineItemNumber)
                    {
                        this.Context.ConsoleHelper.WriteObject(provisioningStatus.Status, "Customer order provisioning status"); 
                    }
                }
            }
        }
    }
}
