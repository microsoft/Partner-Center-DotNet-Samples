// <copyright file="GetCustomerAgreements.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Agreements
{
    using System;
    using System.Linq;

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
            var selectedCustomerId = this.ObtainCustomerId("Enter the ID of the customer to get agreement for");

            // "*" retrieves all agreements of the customer.
            this.GetAgreementsOfCustomer(selectedCustomerId, "*");

            // Retrieve specific types.
            this.GetAgreementsOfCustomer(selectedCustomerId, "MicrosoftCloudAgreement");
            this.GetAgreementsOfCustomer(selectedCustomerId, "MicrosoftCustomerAgreement");
        }

        private void GetAgreementsOfCustomer(string selectedCustomerId, string agreementType)
        {
            var partnerOperations = this.Context.UserPartnerOperations;

            this.Context.ConsoleHelper.StartProgress(string.Equals(agreementType, "*") ? "Retrieving all agreements of the customer": $"Retrieving agreements of customer of type '{agreementType}'");

            var customerAgreements = partnerOperations.Customers.ById(selectedCustomerId).Agreements.ByAgreementType(agreementType).Get();

            this.Context.ConsoleHelper.StopProgress();

            if (!customerAgreements.Items.Any())
            {
                Console.WriteLine(string.Equals(agreementType, "*") ? "No agreements found for the given customer." : $"No agreements of type '{agreementType}' found for the given customer.");
            }
            else
            {
                this.Context.ConsoleHelper.WriteObject(customerAgreements, string.Equals(agreementType, "*") ? "Customer agreements:" : $"Customer agreements of type '{agreementType}':");
            }
        }
    }
}
