// -----------------------------------------------------------------------
// <copyright file="GetIndirectResellersOfCustomer.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.IndirectModel
{
    using System.Globalization;

    /// <summary>
    /// A scenario that retrieves the list of indirect resellers associated to the indirect CSP partner of a customer.
    /// </summary>
    public class GetIndirectResellersOfCustomer : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetIndirectResellersOfCustomer"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetIndirectResellersOfCustomer(IScenarioContext context) : base("Get indirect resellers of a customer", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            string customerId = this.ObtainCustomerId("Enter the ID of the customer: ");

            IAggregatePartner partnerOperations = this.Context.UserPartnerOperations;
            this.Context.ConsoleHelper.StartProgress("Getting indirect resellers of a customer");

            Models.ResourceCollection<Models.Relationships.PartnerRelationship> indirectResellers = partnerOperations.Customers[customerId].Relationships.Get();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(
                indirectResellers,
                string.Format(CultureInfo.InvariantCulture, "Indirect Resellers of customer: {0}", customerId));
        }
    }
}
