// -----------------------------------------------------------------------
// <copyright file="GetValidationStatus.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Customers
{
    using Microsoft.Store.PartnerCenter.Models.ValidationStatus.Enums;

    /// <summary>
    /// A scenario that showcases retrieving a customer's validation status.
    /// </summary>
    public class GetValidationStatus : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetValidationStatus"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetValidationStatus(IScenarioContext context) : base("Get customer validation status.", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            string customerIdToRetrieve = this.ObtainCustomerId("Enter the ID of the customer to retrieve the validation status for:");

            var partnerOperations = this.Context.UserPartnerOperations;
            this.Context.ConsoleHelper.StartProgress($"Retrieving customer's validation status for type: {ValidationType.Account}");

            var validationTypeToFetch = ValidationType.Account;

            var customerValidationStatus = partnerOperations.Customers.ById(customerIdToRetrieve).ValidationStatus.GetValidationStatus(validationTypeToFetch);

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(customerValidationStatus, "Customer Validation Status");
        }
    }
}
