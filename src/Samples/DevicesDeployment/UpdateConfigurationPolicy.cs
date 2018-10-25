// -----------------------------------------------------------------------
// <copyright file="UpdateConfigurationPolicy.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.DevicesDeployment
{
    using System.Collections.Generic;
    using Microsoft.Store.PartnerCenter.Models.DevicesDeployment;

    /// <summary>
    /// Updates configuration policy.
    /// </summary>
    public class UpdateConfigurationPolicy : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateConfigurationPolicy"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public UpdateConfigurationPolicy(IScenarioContext context) : base("Update configuration policy", context)
        {
        }

        /// <summary>
        /// Executes the update configuration policy scenario.
        /// </summary>
        protected override void RunScenario()
        {
            string selectedCustomerId = this.ObtainCustomerId("Enter the Customer Id to update the configuration policy for");

            string selectedConfigurationPolicyId = this.ObtainConfigurationPolicyId("Enter the ID of the Configuration Policy to update");

            ConfigurationPolicy configPolicyToBeUpdated = new ConfigurationPolicy()
            {
                Name = "Test Config Policy",
                Id = selectedConfigurationPolicyId,
                PolicySettings = new List<PolicySettingsType>() { PolicySettingsType.OobeUserNotLocalAdmin, PolicySettingsType.RemoveOemPreinstalls }
            };

            var partnerOperations = this.Context.UserPartnerOperations;
           
            this.Context.ConsoleHelper.StartProgress("Updating Configuration Policy");

            ConfigurationPolicy updatedConfigurationPolicy = partnerOperations.Customers.ById(selectedCustomerId).ConfigurationPolicies.ById(selectedConfigurationPolicyId).Patch(configPolicyToBeUpdated);

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(updatedConfigurationPolicy, "Updated Configuration Policy");
        }
    }
}