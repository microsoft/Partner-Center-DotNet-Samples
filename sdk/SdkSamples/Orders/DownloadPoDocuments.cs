// -----------------------------------------------------------------------
// <copyright file="UploadPoDocuments.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Orders
{
    using Microsoft.Store.PartnerCenter.Models.Orders;
    using Newtonsoft.Json;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Windows.Forms;

    /// <summary>
    /// A scenario that updates a customer order by uploading PO attachments.
    /// </summary>
    public class DownloadPoDocuments : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UploadPoDocuments"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public DownloadPoDocuments(IScenarioContext context) : base("Download PO documents of a customer order", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;

            string customerId = this.ObtainCustomerId("Enter the ID of the customer whom to retrieve their orders");
            string orderId = this.ObtainOrderID("Enter the ID of order to retrieve");
            string attachmentId = this.ObtainOrderID("Enter the ID of attachment to retrieve");

            this.Context.ConsoleHelper.StartProgress("Retrieving attachment");

            var attachment = partnerOperations.Customers.ById(customerId).Orders.ById(orderId).Attachments.ById(attachmentId).Download(); 
            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(attachment, "Uploaded documents to customer order");
        }
    }
}
