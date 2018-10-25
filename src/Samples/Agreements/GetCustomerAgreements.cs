// <copyright file="GetCustomerAgreements.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Agreements
{
    using System;
    using System.Linq;
    using Models;
    using Models.Agreements;

    /// <summary>
    /// Showcases the retrieval of customer agreements.
    /// </summary>
    public class GetCustomerAgreements : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCustomerAgreements"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetCustomerAgreements(IScenarioContext context) : base("Get agreements for a customer.", context)
        {
        }

        /// <summary>
        /// Executes the get customer agreements scenario.
        /// </summary>
        protected override void RunScenario()
        {
            IAggregatePartner partnerOperations = Context.UserPartnerOperations;
            string selectedCustomerId = ObtainCustomerId("Enter the ID of the customer to get agreement for");

            Context.ConsoleHelper.StartProgress("Retrieving agreements of the customer");

            ResourceCollection<Agreement> customerAgreements = partnerOperations.Customers.ById(selectedCustomerId)
                .Agreements.Get();

            Context.ConsoleHelper.StopProgress();

            if (!customerAgreements.Items.Any())
            {
                Console.WriteLine("No agreements found for the given customer.");
            }
            else
            {
                Context.ConsoleHelper.WriteObject(customerAgreements, "Customer agreements:");
            }
        }
    }
}