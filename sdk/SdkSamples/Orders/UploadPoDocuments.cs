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

            this.Context.ConsoleHelper.StartProgress("Retrieving customer order to upload the attachments");
            var customerOrder = partnerOperations.Customers.ById(customerId).Orders.ById(orderId).Get();
            this.Context.ConsoleHelper.StopProgress();

            var formData = new MultipartFormDataContent();

            // Create a dummy file to upload
            var fileContent = System.Text.Encoding.UTF8.GetBytes("This is a test file1 for PO content");

            MultipartFormDataContent multiContent = new MultipartFormDataContent();

            var streamContent = new ByteArrayContent(fileContent);
            var fileName = "PO_Customer_ABC.pdf";
            streamContent.Headers.Add("Content-Type", "application/pdf");
            streamContent.Headers.Add("Content-Disposition", "form-data; name=\"POFiles\"; filename=\"" + fileName + "\"");
            multiContent.Add(streamContent, "POFiles", fileName);

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
            var uploadedAttachments = partnerOperations.Customers.ById(customerId).Orders.ById(customerOrder.Id).Attachments.Upload(formData);           
            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(uploadedAttachments, "Uploaded documents to customer order");
        }
    }
}
