// -----------------------------------------------------------------------
// <copyright file="GetSelfServePolicies.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.SelfServePolicies
{
    /// <summary>
    /// A scenario that gets a self serve policy for a customer.
    /// </summary>
    public class GetSelfServePolicies : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateSelfServePolicies"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetSelfServePolicies(IScenarioContext context) : base("Gets self serve policies", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            string customerIdAsEntity = this.ObtainCustomerId("Enter the ID of the customer to get policies for");

            var partnerOperations = this.Context.UserPartnerOperations;
            this.Context.ConsoleHelper.StartProgress($"Getting self serve policies for customerId { customerIdAsEntity }");

            // gets the self serve policies
            var SelfServePolicies = partnerOperations.SelfServePolicies.Get(customerIdAsEntity);
            this.Context.ConsoleHelper.StopProgress();

            this.Context.ConsoleHelper.WriteObject(SelfServePolicies, $"Self serve policies for customerId { customerIdAsEntity }");
        }
    }
}