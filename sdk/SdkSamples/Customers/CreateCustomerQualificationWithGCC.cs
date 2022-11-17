// -----------------------------------------------------------------------
// <copyright file="CreateCustomerQualificationWithGCC.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Customers
{
    using Microsoft.Store.PartnerCenter.Models.Customers;
    using Microsoft.Store.PartnerCenter.Models.ValidationCodes;

    /// <summary>
    /// Creates a GCC qualification for a customer.
    /// </summary>
    public class CreateCustomerQualificationWithGCC : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCustomerQualificationWithGCC"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public CreateCustomerQualificationWithGCC(IScenarioContext context) : base("Create customer qualification with GCC", context)
        {
        }

        /// <summary>
        /// Executes the create customer qualification with GCC scenario.
        /// </summary>
        protected override void RunScenario()
        {
            string customerIdToRetrieve = this.ObtainCustomerId($"Enter the ID of the customer to create qualification for {CustomerQualification.GovernmentCommunityCloud}");

            var partnerOperations = this.Context.UserPartnerOperations;

            this.Context.ConsoleHelper.StartProgress("Retrieving validation codes");
            var validations = partnerOperations.Validations.GetValidationCodes();
            this.Context.ConsoleHelper.StopProgress();

            this.Context.ConsoleHelper.Success("Success!");

            this.Context.ConsoleHelper.WriteObject(validations, "Validations");

            string validationCodeToRetrieve = this.ObtainQuantity("Enter validation code to use by ValidationId");

            ValidationCode code = null;

            foreach (ValidationCode c in validations)
            {
                if (c.ValidationId == validationCodeToRetrieve)
                {
                    code = c;
                    break;
                }
            }

            if (code == null)
            {
                this.Context.ConsoleHelper.Error("Code not found");
            }

            this.Context.ConsoleHelper.StartProgress("Creating customer qualification with GCC");

            var customerQualificationRequest = new Models.Customers.V2.CustomerQualificationRequest 
            {
                Qualification = "GovernmentCommunityCloud",
                ValidationCode = code.ValidationId,
            };

            var createCustomerQualification = partnerOperations.Customers.ById(customerIdToRetrieve).Qualification.CreateQualifications(customerQualificationRequest);

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(createCustomerQualification, "Customer Qualification with GCC");
        }
    }
}
