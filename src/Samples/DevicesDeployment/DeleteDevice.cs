// -----------------------------------------------------------------------
// <copyright file="DeleteDevice.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.DevicesDeployment
{
    /// <summary>
    /// Deletes a device.
    /// </summary>
    public class DeleteDevice : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteDevice"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public DeleteDevice(IScenarioContext context) : base("Deletes a device", context)
        {
        }

        /// <summary>
        /// Executes the delete device scenario.
        /// </summary>
        protected override void RunScenario()
        {
            string selectedCustomerId = this.ObtainCustomerId("Enter the ID of the customer to delete a device");

            string selectedDeviceBatchId = this.ObtainDeviceBatchId("Enter the ID of the Device batch to retrieve the devices of");

            string selectedDeviceId = this.ObtainDeviceId("Enter the ID of the device to delete");

            var partnerOperations = this.Context.UserPartnerOperations;
           
            this.Context.ConsoleHelper.WriteObject(selectedDeviceId, "Device to be deleted");
            this.Context.ConsoleHelper.StartProgress("Deleting device");

           partnerOperations.Customers.ById(selectedCustomerId).DeviceBatches.ById(selectedDeviceBatchId).Devices.ById(selectedDeviceId).Delete();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.Success("Deleted the device successfully!");
        }
    }
}
