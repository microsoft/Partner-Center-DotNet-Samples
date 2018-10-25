// -----------------------------------------------------------------------
// <copyright file="GetIndirectResellers.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.IndirectModel
{
    using Models.Relationships;

    /// <summary>
    /// A scenario that retrieves the list of indirect resellers associated to the indirect CSP partner
    /// </summary>
    public class GetIndirectResellers : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetIndirectResellers"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetIndirectResellers(IScenarioContext context) : base("Get indirect resellers", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;
            this.Context.ConsoleHelper.StartProgress("Getting indirect resellers");

            var indirectResellers = partnerOperations.Relationships.Get(PartnerRelationshipType.IsIndirectCloudSolutionProviderOf);

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(indirectResellers, "Indirect Resellers");
        }
    }
}
