// -----------------------------------------------------------------------
// <copyright file="GetAgreementDetails.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Agreements
{
    /// <summary>
    /// Showcases getting the list of agreement details.
    /// </summary>
    public class GetAgreementDetails : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetAgreementDetails"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetAgreementDetails(IScenarioContext context) : base("Get agreement details.", context)
        {
        }

        /// <summary>
        /// Executes the get agreement details scenario.
        /// </summary>
        protected override void RunScenario()
        {

            this.Context.ConsoleHelper.StartProgress("Retrieving all agreement details");

            // "*" retrieves all agreements.
            this.GetAgreementDetailsOfType("*");

            // Retrieve specific types.
            this.GetAgreementDetailsOfType("MicrosoftCloudAgreement");
            this.GetAgreementDetailsOfType("MicrosoftCustomerAgreement");
        }

        private void GetAgreementDetailsOfType(string agreementType)
        {
            var partnerOperations = this.Context.UserPartnerOperations;

            this.Context.ConsoleHelper.StartProgress(string.Equals(agreementType, "*") ? "Retrieving all agreement details" : $"Retrieving details of agreements of type {agreementType}");

            var agreementDetails = partnerOperations.AgreementDetails.ByAgreementType(agreementType).Get();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(agreementDetails, string.Equals(agreementType, "*") ? "Agreement details:" : $"Agreement details of type {agreementType}:");
        }
    }
}
