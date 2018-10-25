// -----------------------------------------------------------------------
// <copyright file="CreateDevices.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.DevicesDeployment
{
    using System.Collections.Generic;
    using Microsoft.Store.PartnerCenter.Models.DevicesDeployment;

    /// <summary>
    /// Creates new devices under an existing device batch.
    /// </summary>
    public class CreateDevices : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateDevices"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public CreateDevices(IScenarioContext context) : base("Create new Devices", context)
        {
        }

        /// <summary>
        /// Executes the create device batch scenario.
        /// </summary>
        protected override void RunScenario()
        {
            string selectedCustomerId = this.ObtainCustomerId("Enter the ID of the customer to create the devices for");

            string selectedDeviceBatchId = this.ObtainDeviceBatchId("Enter the ID of the Device batch to add the devices to");

            var partnerOperations = this.Context.UserPartnerOperations;

            List<Device> devicesToBeUploaded = new List<Device>
            {
                new Device
                {
                    HardwareHash = "DummyHash1234",
                    ProductKey = "00329-00000-0003-AA606",
                    SerialNumber = "2R9-ZNP67"
                },

                new Device
                {
                    HardwareHash = "DummyHash12345",
                    ProductKey = "00329-00000-0003-AA606",
                    SerialNumber = "2R9-ZNP67"
                }
            };

            this.Context.ConsoleHelper.WriteObject(devicesToBeUploaded, "New Devices");
            this.Context.ConsoleHelper.StartProgress("Creating Devices");

            var trackingLocation = partnerOperations.Customers.ById(selectedCustomerId).DeviceBatches.ById(selectedDeviceBatchId).Devices.Create(devicesToBeUploaded);

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(trackingLocation, "Tracking Location to track the status");
            this.Context.ConsoleHelper.Success("Create Devices Request submitted successfully!");
        }
    }
}
