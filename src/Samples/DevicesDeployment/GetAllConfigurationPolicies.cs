// -----------------------------------------------------------------------
// <copyright file="GetAllConfigurationPolicies.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.DevicesDeployment
{
    /// <summary>
    /// Gets all configuration policies of a customer.
    /// </summary>
    public class GetAllConfigurationPolicies : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllConfigurationPolicies"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetAllConfigurationPolicies(IScenarioContext context) : base("Get all configuration policies", context)
        {
        }

        /// <summary>
        /// Executes get all configuration policies scenario.
        /// </summary>
        protected override void RunScenario()
        {
            string selectedCustomerId = this.ObtainCustomerId("Enter the ID of the customer to get the configuration policies");

            var partnerOperations = this.Context.UserPartnerOperations;
           
            this.Context.ConsoleHelper.StartProgress("Querying Configuration policies");

            var configPolicies = partnerOperations.Customers.ById(selectedCustomerId).ConfigurationPolicies.Get();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(configPolicies, "Configuration Policies");
        }
    }
}
