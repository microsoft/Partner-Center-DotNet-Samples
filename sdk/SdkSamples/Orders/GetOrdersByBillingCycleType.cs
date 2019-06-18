// -----------------------------------------------------------------------
// <copyright file="GetOrdersByBillingCycleType.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Orders
{
    using System;
    using Models.Offers;

    /// <summary>
    /// A scenario that retrieves all customer orders by billing cycle type.
    /// </summary> 
    public class GetOrdersByBillingCycleType : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetOrdersByBillingCycleType"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetOrdersByBillingCycleType(IScenarioContext context) : base("Get All Orders by billing cycle type", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            IAggregatePartner partnerOperations = this.Context.UserPartnerOperations;

            string customerId = this.ObtainCustomerId("Enter the ID of the customer whom to retrieve their order by billing cycle type");
            string billingCycle = this.ObtainBillingCycleType("Enter the billing cycle type");

            BillingCycleType billingCyle = (BillingCycleType)Enum.Parse(typeof(BillingCycleType), billingCycle, true);

            this.Context.ConsoleHelper.StartProgress("Retrieving customer orders");

            Models.ResourceCollection<Models.Orders.Order> customerOrders = partnerOperations.Customers.ById(customerId).Orders.ByBillingCycleType(billingCyle).Get();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(customerOrders, "Retrieved Customer orders");
        }
    }
}
