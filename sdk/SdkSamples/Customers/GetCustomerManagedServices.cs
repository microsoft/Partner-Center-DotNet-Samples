// -----------------------------------------------------------------------
// <copyright file="GetCustomerManagedServices.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Customers
{
    /// <summary>
    /// Gets a customer list of managed services.
    /// </summary>
    public class GetCustomerManagedServices : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCustomerManagedServices"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetCustomerManagedServices(IScenarioContext context) : base("Get customer managed services", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            string customerId = this.ObtainCustomerId();

            var partnerOperations = this.Context.UserPartnerOperations;
            this.Context.ConsoleHelper.StartProgress("Getting the customer's managed services");

            var managedServices = partnerOperations.Customers.ById(customerId).ManagedServices.Get();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(managedServices, "Customer managed services");
        }
    }
}
