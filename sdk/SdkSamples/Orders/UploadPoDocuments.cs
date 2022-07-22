// -----------------------------------------------------------------------
// <copyright file="UploadPoDocuments.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Orders
{
    using System.IO;
    using System.Net.Http;
    using Microsoft.Store.PartnerCenter.Models.Orders;
    using Newtonsoft.Json;

    /// <summary>
    /// A scenario that updates a customer order by uploading PO attachments.
    /// </summary>
    public class UploadPoDocuments : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UploadPoDocuments"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public UploadPoDocuments(IScenarioContext context) : base("Upload PO documents to customer order", context)
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
            var fileName = this.ObtainFileName("Enter the file path");

            MultipartFormDataContent multiContent = new MultipartFormDataContent();

            FileStream fs = File.OpenRead(fileName);
            var streamContent = new StreamContent(fs);
            streamContent.Headers.Add("Content-Type", "application/pdf");
            streamContent.Headers.Add("Content-Disposition", "form-data; name=\"POFiles\"; filename=\"" + Path.GetFileName(fileName) + "\"");
            multiContent.Add(streamContent, "POFiles", Path.GetFileName(fileName));

            var attachmentMetadata = new AttachmentMetadata()
            {
                Currency = "USD",
                CustomerPrice = "100",
                FxRate = "1",
                IsPartOfTender = false,
            };

            var stringContent = new StringContent(JsonConvert.SerializeObject(attachmentMetadata), System.Text.Encoding.UTF8, "application/json");
            stringContent.Headers.Add("Content-Disposition", "form-data; name=\"metadata\"");
            multiContent.Add(stringContent, "metadata");

            this.Context.ConsoleHelper.StartProgress("Uploading PO documents to the customer order");
            var uploadedAttachments = partnerOperations.Customers.ById(customerId).Orders.ById(orderId).Attachments.Upload(multiContent);           
            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(uploadedAttachments, "Uploaded documents to customer order");
        }

        /// <summary>
        /// Obtain an order ID to work with the configuration if set there or prompts the user to enter it.
        /// </summary>
        /// <param name="promptMessage">An optional custom prompt message</param>
        /// <returns>The order ID</returns>
        protected string ObtainFileName(string promptMessage = default(string))
        {
            return this.Context.ConsoleHelper.ReadNonEmptyString(promptMessage, "");
        }
    }
}
