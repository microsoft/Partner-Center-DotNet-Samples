// -----------------------------------------------------------------------
// <copyright file="CreateDeviceBatch.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.DevicesDeployment
{
    using System.Collections.Generic;
    using Microsoft.Store.PartnerCenter.Models.DevicesDeployment;

    /// <summary>
    /// Creates a new device batch with devices.
    /// </summary>
    public class CreateDeviceBatch : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateDeviceBatch"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public CreateDeviceBatch(IScenarioContext context) : base("Create a new Device Batch", context)
        {
        }

        /// <summary>
        /// Executes the create device batch scenario.
        /// </summary>
        protected override void RunScenario()
        {
            string selectedCustomerId = this.ObtainCustomerId("Enter the ID of the customer to create the device batch for");

            List<Device> devicesToBeUploaded = new List<Device>
            {
                new Device
                {
                    HardwareHash = "DummyHash123",
                    ProductKey = "00329-00000-0003-AA606",
                    SerialNumber = "1R9-ZNP67"
                }
            };

            DeviceBatchCreationRequest newDeviceBatch = new DeviceBatchCreationRequest
            {
                BatchId = "SDKTestDeviceBatch",
                Devices = devicesToBeUploaded
            };

            IAggregatePartner partnerOperations = this.Context.UserPartnerOperations;

            this.Context.ConsoleHelper.WriteObject(newDeviceBatch, "New Device Batch");
            this.Context.ConsoleHelper.StartProgress("Creating Device Batch");

            string trackingLocation = partnerOperations.Customers.ById(selectedCustomerId).DeviceBatches.Create(newDeviceBatch);

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(trackingLocation, "Tracking Location to track the status");
            this.Context.ConsoleHelper.Success("Create Device Batch Request submitted successfully!");
        }
    }
}
