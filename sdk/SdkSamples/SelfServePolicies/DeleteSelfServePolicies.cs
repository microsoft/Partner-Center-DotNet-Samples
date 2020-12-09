// -----------------------------------------------------------------------
// <copyright file="DeleteSelfServePolicies.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------


namespace Microsoft.Store.PartnerCenter.Samples.SelfServePolicies
{
    /// <summary>
    /// A scenario that deletes a self serve policy for a customer.
    /// </summary>
    public class DeleteSelfServePolicies : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateSelfServePolicies"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public DeleteSelfServePolicies(IScenarioContext context) : base("Deletes self serve policy", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            string policyId = this.ObtainCustomerId("Enter the self serve policy ID to delete");

            var partnerOperations = this.Context.UserPartnerOperations;

            // deletes the self serve policies
            this.Context.ConsoleHelper.StartProgress($"Deleting the self serve policy id: { policyId }");
            partnerOperations.SelfServePolicies.ById(policyId).Delete();
            this.Context.ConsoleHelper.StopProgress();
        }
    }
}
