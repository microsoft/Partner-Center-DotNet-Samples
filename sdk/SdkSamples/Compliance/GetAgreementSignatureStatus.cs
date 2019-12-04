// -----------------------------------------------------------------------
// <copyright file="GetMPNProfile.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Compliance
{
    public class GetAgreementSignatureStatus: BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetAgreementSignatureStatus"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetAgreementSignatureStatus(IScenarioContext context) : base("Get agreement signature status", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;

            this.Context.ConsoleHelper.StartProgress("Retrieving agreement signature status by MPN Id");
            var agreementSignatureStatusByMpnId = partnerOperations.Compliance.AgreementSignatureStatus.Get(mpnId:"Enter MPN Id");
            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(agreementSignatureStatusByMpnId, "Agreement signature status by MPN Id");

            this.Context.ConsoleHelper.StartProgress("Retrieving agreement signature status by Tenant Id");
            var agreementSignatureStatusByTenantId = partnerOperations.Compliance.AgreementSignatureStatus.Get(tenantId: "Enter Tenant Id");
            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(agreementSignatureStatusByTenantId, "Agreement signature status by Tenant Id");
        }
    }
}