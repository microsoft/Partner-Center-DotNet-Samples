// -----------------------------------------------------------------------
// <copyright file="GetDevicesBatches.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.DevicesDeployment
{
    /// <summary>
    /// Gets devices batches.
    /// </summary>
    public class GetDevicesBatches : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetDevicesBatches"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetDevicesBatches(IScenarioContext context) : base("Get Devices Batches", context)
        {
        }

        /// <summary>
        /// Executes the create device batch scenario.
        /// </summary>
        protected override void RunScenario()
        {
            string selectedCustomerId = this.ObtainCustomerId("Enter the Customer Id to get the devices batches for");

            var partnerOperations = this.Context.UserPartnerOperations;
           
            this.Context.ConsoleHelper.StartProgress("Querying for the devices batches");

            var devicesBatches = partnerOperations.Customers.ById(selectedCustomerId).DeviceBatches.Get();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(devicesBatches, "Device Batches");
        }
    }
}
