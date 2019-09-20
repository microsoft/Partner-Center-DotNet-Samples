// -----------------------------------------------------------------------
// <copyright file="CreateCustomerAgreement.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Agreements
{
    using System;
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

            string agreementTemplateId = this.Context.Configuration.Scenario.DefaultAgreementTemplateId;

            if (string.IsNullOrWhiteSpace(agreementTemplateId))
            {
                // The value was not set in the configuration, prompt the user to enter value
                agreementTemplateId = this.Context.ConsoleHelper.ReadNonEmptyString("Enter the agreement template ID", "The agreement template ID can't be empty");
            }
            else
            {
                Console.WriteLine("Found {0}: {1} in configuration.", "Agreement template Id", agreementTemplateId);
            }
    
            var partnerOperations = this.Context.UserPartnerOperations;

            var agreement = new Agreement
            {
                DateAgreed = DateTime.UtcNow,
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

            var newlyCreatedAgreement = partnerOperations.Customers.ById(selectedCustomerId).Agreements.Create(agreement);

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.Success("Create new agreement successfully!");
            this.Context.ConsoleHelper.WriteObject(newlyCreatedAgreement, "Newly created agreement Information");
        }
    }
}
