// -----------------------------------------------------------------------
// <copyright file="CreateConfigurationPolicy.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.DevicesDeployment
{
    using System.Collections.Generic;
    using Microsoft.Store.PartnerCenter.Models.DevicesDeployment;

    /// <summary>
    /// Creates a new configuration policy for devices.
    /// </summary>
    public class CreateConfigurationPolicy : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateConfigurationPolicy"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public CreateConfigurationPolicy(IScenarioContext context) : base("Create a new configuration policy", context)
        {
        }

        /// <summary>
        /// Executes the create configuration policy scenario.
        /// </summary>
        protected override void RunScenario()
        {
            string selectedCustomerId = this.ObtainCustomerId("Enter the ID of the customer to create the configuration policy for");

            var configurationPolicyToCreate = new ConfigurationPolicy
            {
                Name = "Test Config Policy",
                Description = "This configuration policy is created by the SDK samples",
                PolicySettings = new List<PolicySettingsType>() { PolicySettingsType.OobeUserNotLocalAdmin, PolicySettingsType.SkipEula }
            };
            var partnerOperations = this.Context.UserPartnerOperations;

            this.Context.ConsoleHelper.WriteObject(configurationPolicyToCreate, "New configuration policy Information");
            this.Context.ConsoleHelper.StartProgress("Creating Configuration Policy");

            var createdConfigurationPolicy = partnerOperations.Customers.ById(selectedCustomerId).ConfigurationPolicies.Create(configurationPolicyToCreate);

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.Success("Success!");
            this.Context.ConsoleHelper.WriteObject(createdConfigurationPolicy, "Created configuration policy Information");
        }
    }
}
