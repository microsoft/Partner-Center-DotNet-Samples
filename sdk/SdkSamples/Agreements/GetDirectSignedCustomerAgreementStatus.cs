// -----------------------------------------------------------------------
// <copyright file="GetDirectSignedCustomerAgreementStatus.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Agreements
{
    /// <summary>
    /// Gets the status of a customer's direct signing (direct acceptance) of Microsoft Customer Agreement.
    /// </summary>
    public class GetDirectSignedCustomerAgreementStatus : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetDirectSignedCustomerAgreementStatus"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetDirectSignedCustomerAgreementStatus(IScenarioContext context) : base("Get customer's MCA direct signing status.", context)
        {
        }

        /// <summary>
        /// Executes the get customer's MCA direct signing status scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var selectedCustomerId = this.ObtainCustomerId("Enter the ID of the customer to get MCA direct signing status for");

            var partnerOperations = this.Context.UserPartnerOperations;

            this.Context.ConsoleHelper.StartProgress($"Retrieving MCA direct signing status for customer with ID {selectedCustomerId}");

            var signingStatus = partnerOperations.Customers.ById(selectedCustomerId).Agreements.GetDirectSignedCustomerAgreementStatus();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(signingStatus, "MCA Direct Signing Status of Customer");
        }
    }
}
