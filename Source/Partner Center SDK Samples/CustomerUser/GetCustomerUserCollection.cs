// -----------------------------------------------------------------------
// <copyright file="GetCustomerUserCollection.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.CustomerUser
{
    /// <summary>
    /// Gets customer user collection.
    /// </summary>
    public class GetCustomerUserCollection : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCustomerUserCollection"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetCustomerUserCollection(IScenarioContext context) : base("Get a customer user collection", context)
        {
        }

        /// <summary>
        /// Executes the get customer user collection scenario.
        /// </summary>
        protected override void RunScenario()
        {
            // get customer Id.
            string selectedCustomerId = this.ObtainCustomerId("Enter the ID of the customer to get customer user collection");
            var partnerOperations = this.Context.UserPartnerOperations;

            this.Context.ConsoleHelper.StartProgress("Getting customer users collection");

            // get customer users collection
            var customerUsers = partnerOperations.Customers.ById(selectedCustomerId).Users.Get();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(customerUsers, "Customer Users collection");
        }
    }
}
