// -----------------------------------------------------------------------
// <copyright file="GetAzureRateCard.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.RateCards
{
    /// <summary>
    /// Gets the Azure rate card.
    /// </summary>
    public class GetAzureRateCard : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetAzureRateCard"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetAzureRateCard(IScenarioContext context) : base("Get Azure rate card", context)
        {
        }

        /// <summary>
        /// executes the get Azure rate card scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;

            this.Context.ConsoleHelper.StartProgress("Retrieving Azure rate card");
            var azureRateCard = partnerOperations.RateCards.Azure.Get();
            this.Context.ConsoleHelper.StopProgress();

            this.Context.ConsoleHelper.WriteObject(azureRateCard, "Azure Rate Card");
        }
    }
}
