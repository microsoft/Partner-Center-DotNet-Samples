// -----------------------------------------------------------------------
// <copyright file="DeleteConfigurationPolicy.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.DevicesDeployment
{
    /// <summary>
    /// Deletes a configuration policy.
    /// </summary>
    public class DeleteConfigurationPolicy : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteConfigurationPolicy"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public DeleteConfigurationPolicy(IScenarioContext context) : base("Delete a configuration policy", context)
        {
        }

        /// <summary>
        /// Executes the create device batch scenario.
        /// </summary>
        protected override void RunScenario()
        {
            string selectedCustomerId = this.ObtainCustomerId("Enter the ID of the customer to delete a configuration policy");

            string selectedPolicyId = this.ObtainConfigurationPolicyId("Enter the ID of the configuration policy to delete");

            IAggregatePartner partnerOperations = this.Context.UserPartnerOperations;

            this.Context.ConsoleHelper.WriteObject(selectedPolicyId, "Configuration policy to be deleted");
            this.Context.ConsoleHelper.StartProgress("Deleting Configuration policy");

            partnerOperations.Customers.ById(selectedCustomerId).ConfigurationPolicies.ById(selectedPolicyId).Delete();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.Success("Delete Configuration policy request submitted successfully!");
        }
    }
}
