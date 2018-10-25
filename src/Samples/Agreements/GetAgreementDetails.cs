// -----------------------------------------------------------------------
// <copyright file="GetAgreementDetails.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Agreements
{
    using Models;
    using Models.Agreements;

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
            IAggregatePartner partnerOperations = Context.UserPartnerOperations;
            Context.ConsoleHelper.StartProgress("Retrieving agreement details");

            ResourceCollection<AgreementMetaData> agreementDetails = partnerOperations.AgreementDetails.Get();

            Context.ConsoleHelper.StopProgress();
            Context.ConsoleHelper.WriteObject(agreementDetails, "Agreement details:");
        }
    }
}