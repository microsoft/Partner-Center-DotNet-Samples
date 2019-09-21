// -----------------------------------------------------------------------
// <copyright file="GetAgreementDocument.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Agreements
{
    using System.Linq;

    /// <summary>
    /// Showcases getting an agreement document.
    /// </summary>
    public class GetAgreementDocument : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetAgreementDocument"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetAgreementDocument(IScenarioContext context) : base("Get agreement document.", context)
        {
        }

        /// <summary>
        /// Executes the get agreement document scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;

            // Get Microsoft Customer Agreement document (for different language/locales and countries).
            var microsoftCustomerAgreementDetails = partnerOperations.AgreementDetails.ByAgreementType("MicrosoftCustomerAgreement").Get().Items.Single();

            var agreementTemplate = partnerOperations.AgreementTemplates.ById(microsoftCustomerAgreementDetails.TemplateId);

            this.Context.ConsoleHelper.StartProgress($"Retrieving agreement document for default language and default country");
            var agreementDocument = agreementTemplate.Document.Get();
            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(agreementDocument, "Agreement document:");

            this.Context.ConsoleHelper.StartProgress($"Retrieving agreement document for language 'de-DE' and default country");
            agreementDocument = agreementTemplate.Document.ByLanguage("de-DE").Get();
            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(agreementDocument, "Agreement document:");

            this.Context.ConsoleHelper.StartProgress($"Retrieving agreement document for language 'de-DE' and country 'CA'");
            agreementDocument = agreementTemplate.Document.ByLanguage("de-DE").ByCountry("CA").Get();
            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(agreementDocument, "Agreement document:");

            this.Context.ConsoleHelper.StartProgress($"Retrieving agreement document for language 'ja-JP' and country 'KO'");
            agreementDocument = agreementTemplate.Document.ByCountry("KO").ByLanguage("ja-JP").Get();
            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(agreementDocument, "Agreement document:");
        }
    }
}
