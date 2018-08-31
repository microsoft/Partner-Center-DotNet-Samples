// -----------------------------------------------------------------------
// <copyright file="UpdateDevicesPolicy.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.DevicesDeployment
{
    using System.Collections.Generic;
    using Microsoft.Store.PartnerCenter.Models.DevicesDeployment;

    /// <summary>
    /// Updates devices with configuration policy.
    /// </summary>
    public class UpdateDevicesPolicy : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateDevicesPolicy"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public UpdateDevicesPolicy(IScenarioContext context) : base("Update configuration policy of devices", context)
        {
        }

        /// <summary>
        /// Executes the update devices scenario.
        /// </summary>
        protected override void RunScenario()
        {
            string selectedCustomerId = this.ObtainCustomerId("Enter the Customer Id to update the devices for");

            string selectedConfigurationPolicyId = this.ObtainConfigurationPolicyId("Enter the ID of the Configuration Policy to update the device with");

            string selectedDeviceId = this.ObtainDeviceId("Enter the Device Id to update");

            List<KeyValuePair<PolicyCategory, string>> policyToBeAdded = new List<KeyValuePair<PolicyCategory, string>>
            {
                new KeyValuePair<PolicyCategory, string>(PolicyCategory.OOBE, selectedConfigurationPolicyId)
            };

            List<Device> devices = new List<Device>
            {
                new Device
                {
                    Id = selectedDeviceId,
                    Policies = policyToBeAdded
                }
            };

            DevicePolicyUpdateRequest devicePolicyUpdateRequest = new DevicePolicyUpdateRequest
            {
                Devices = devices             
            };
           
            var partnerOperations = this.Context.UserPartnerOperations;
           
            this.Context.ConsoleHelper.StartProgress("Updating Devices with Configuration Policy");

            var trackingLocation = partnerOperations.Customers.ById(selectedCustomerId).DevicePolicy.Update(devicePolicyUpdateRequest);

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(trackingLocation, "Tracking Location to track the status");
            this.Context.ConsoleHelper.Success("Update Devices Request submitted successfully!");
        }
    }
}
