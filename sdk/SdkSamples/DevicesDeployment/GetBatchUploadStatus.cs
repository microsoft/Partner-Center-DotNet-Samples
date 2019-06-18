// -----------------------------------------------------------------------
// <copyright file="GetBatchUploadStatus.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.DevicesDeployment
{
    /// <summary>
    /// Gets batch upload status.
    /// </summary>
    public class GetBatchUploadStatus : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetBatchUploadStatus"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetBatchUploadStatus(IScenarioContext context) : base("Get Batch Upload Status", context)
        {
        }

        /// <summary>
        /// Executes the create device batch scenario.
        /// </summary>
        protected override void RunScenario()
        {
            string selectedCustomerId = this.ObtainCustomerId("Enter the Customer Id to get the status for");

            string selectedTrackingId = this.ObtainBatchUploadStatusTrackingId("Enter the batch upload status tracking Id to get the status of");

            IAggregatePartner partnerOperations = this.Context.UserPartnerOperations;

            this.Context.ConsoleHelper.StartProgress("Querying the status");

            Models.DevicesDeployment.BatchUploadDetails status = partnerOperations.Customers.ById(selectedCustomerId).BatchUploadStatus.ById(selectedTrackingId).Get();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(status, "Tracking Status");
        }
    }
}
