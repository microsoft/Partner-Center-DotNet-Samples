// -----------------------------------------------------------------------
// <copyright file="GetAzureSharedRateCard.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.RateCards
{
    /// <summary>
    /// Gets the Azure shared rate card.
    /// </summary>
    public class GetAzureSharedRateCard : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetAzureSharedRateCard"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetAzureSharedRateCard(IScenarioContext context) : base("Get Azure shared rate card", context)
        {
        }

        /// <summary>
        /// executes the get Azure shared rate card scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;

            this.Context.ConsoleHelper.StartProgress("Retrieving Azure shared rate card");
            var azureSharedRateCard = partnerOperations.RateCards.Azure.GetShared();
            this.Context.ConsoleHelper.StopProgress();

            this.Context.ConsoleHelper.WriteObject(azureSharedRateCard, "Azure Shared Rate Card");
        }
    }
}
