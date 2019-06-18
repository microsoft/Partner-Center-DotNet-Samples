// -----------------------------------------------------------------------
// <copyright file="GetCustomerOfferCategories.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Offers
{
    /// <summary>
    /// A scenario that retrieves all the offer categories available for a customer.
    /// </summary>
    public class GetCustomerOfferCategories : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCustomerOfferCategories"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetCustomerOfferCategories(IScenarioContext context) : base("Get customer offer categories", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            string customerIdToRetrieve = this.ObtainCustomerId("Enter the ID of the customer to retrieve offer categories for");
            IAggregatePartner partnerOperations = this.Context.UserPartnerOperations;

            this.Context.ConsoleHelper.StartProgress($"Getting offer catgories for customer { customerIdToRetrieve }");
            Models.ResourceCollection<Models.Offers.OfferCategory> offerCategories = partnerOperations.Customers.ById(customerIdToRetrieve).OfferCategories.Get();
            this.Context.ConsoleHelper.StopProgress();

            this.Context.ConsoleHelper.WriteObject(offerCategories, $"Offer categories for customer { customerIdToRetrieve }");
        }
    }
}
