// -----------------------------------------------------------------------
// <copyright file="GetCustomerOffers.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Offers
{
    /// <summary>
    /// A scenario that retrieves all the offers available for a customer.
    /// </summary>
    public class GetCustomerOffers : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCustomerOffers"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetCustomerOffers(IScenarioContext context) : base("Get customer offers", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            string customerIdToRetrieve = this.ObtainCustomerId("Enter the ID of the customer to retrieve offers for");
            var pageSize = this.Context.Configuration.Scenario.DefaultOfferPageSize;
            var partnerOperations = this.Context.UserPartnerOperations;
            
            this.Context.ConsoleHelper.StartProgress($"Getting first { pageSize } offers for customer { customerIdToRetrieve }");
            var offers = partnerOperations.Customers.ById(customerIdToRetrieve).Offers.Get(0, pageSize);
            this.Context.ConsoleHelper.StopProgress();

            this.Context.ConsoleHelper.WriteObject(offers, $"First { pageSize } offers for customer { customerIdToRetrieve }");
        }
    }
}
