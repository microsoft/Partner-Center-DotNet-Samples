// -----------------------------------------------------------------------
// <copyright file="GetDevices.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.DevicesDeployment
{
    /// <summary>
    /// Gets devices of a device batch.
    /// </summary>
    public class GetDevices : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetDevices"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetDevices(IScenarioContext context) : base("Get Devices", context)
        {
        }

        /// <summary>
        /// Executes the create device batch scenario.
        /// </summary>
        protected override void RunScenario()
        {
            string selectedCustomerId = this.ObtainCustomerId("Enter the Customer Id to get the devices for");

            string selectedDeviceBatchId = this.ObtainDeviceBatchId("Enter the ID of the Device batch to retrieve the devices of");

            var partnerOperations = this.Context.UserPartnerOperations;
           
            this.Context.ConsoleHelper.StartProgress("Querying for the devices");

            var devices = partnerOperations.Customers.ById(selectedCustomerId).DeviceBatches.ById(selectedDeviceBatchId).Devices.Get();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(devices, "Devices");
        }
    }
}
