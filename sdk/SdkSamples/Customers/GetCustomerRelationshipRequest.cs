// -----------------------------------------------------------------------
// <copyright file="GetCustomerRelationshipRequest.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Customers
{
    /// <summary>
    /// Gets the request which establishes a relationship between the partner and their customers.
    /// </summary>
    public class GetCustomerRelationshipRequest : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCustomerRelationshipRequest"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetCustomerRelationshipRequest(IScenarioContext context) : base("Get customer relationship request", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            IAggregatePartner partnerOperations = this.Context.UserPartnerOperations;

            this.Context.ConsoleHelper.StartProgress("Retrieving customer relationship request");
            Models.RelationshipRequests.CustomerRelationshipRequest customerRelationshipRequest = partnerOperations.Customers.RelationshipRequest.Get();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(customerRelationshipRequest, "Customer relationship request");
        }
    }
}
