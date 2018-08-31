// -----------------------------------------------------------------------
// <copyright file="CreateCustomerAgreement.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Agreements
{
    using System;
    using System.Collections.Generic;
    using Models.Agreements;

    /// <summary>
    /// Showcases creation of a customer agreement.
    /// </summary>
    public class CreateCustomerAgreement : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCustomerAgreement"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public CreateCustomerAgreement(IScenarioContext context) : base("Create a new customer agreement", context)
        {
        }

        /// <summary>
        /// executes the create customer agreement scenario.
        /// </summary>
        protected override void RunScenario()
        {
            string selectedCustomerId = this.ObtainCustomerId("Enter the ID of the customer to create the agreement for");
            string selectedUserId =
                this.ObtainUserMemberId("Enter the user ID of the partner to create customer's agreement");

            string agreementTemplateId = this.Context.Configuration.Scenario.DefaultAgreementTemplateId;

            // Currently, the only supported value is “998b88de-aa99-4388-a42c-1b3517d49490”, which is the unique identifier for the Microsoft Cloud Agreement. 
            if (string.IsNullOrWhiteSpace(agreementTemplateId))
            {
                // The value was not set in the configuration, prompt the user the enter value
                agreementTemplateId = this.Context.ConsoleHelper.ReadNonEmptyString("Enter the agreement template ID", "The agreement template ID can't be empty");
            }
            else
            {
                Console.WriteLine("Found {0}: {1} in configuration.", "Agreement template Id", agreementTemplateId);
            }
    
            var partnerOperations = this.Context.UserPartnerOperations;

            var agreement = new Agreement
            {
                UserId = selectedUserId,
                DateAgreed = DateTime.UtcNow,
                Type = AgreementType.MicrosoftCloudAgreement,
                TemplateId = agreementTemplateId,
                PrimaryContact = new Contact
                {
                    FirstName = "First",
                    LastName = "Last",
                    Email = "first.last@outlook.com",
                    PhoneNumber = "4123456789"
                }
            };

            this.Context.ConsoleHelper.WriteObject(agreement, "New Agreement");
            this.Context.ConsoleHelper.StartProgress("Creating Agreement");

            var newlyCreatedagreement = partnerOperations.Customers.ById(selectedCustomerId).Agreements.Create(agreement);

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.Success("Create new agreement successfully!");
            this.Context.ConsoleHelper.WriteObject(newlyCreatedagreement, "Newly created agreement Information");
        }
    }
}
