// -----------------------------------------------------------------------
// <copyright file="CreateCustomerQualification.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------


namespace Microsoft.Store.PartnerCenter.Samples.Customers
{
    using Microsoft.Store.PartnerCenter.Models.Customers;

    /// <summary>
    /// Creates a qualification for a customer.
    /// </summary>
    public class CreateCustomerQualification : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCustomerQualification"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public CreateCustomerQualification(IScenarioContext context) : base("Create customer qualification", context)
        {
        }

        /// <summary>
        /// Executes the create customer qualification scenario.
        /// </summary>
        protected override void RunScenario()
        {
            string customerIdToRetrieve = this.ObtainCustomerId($"Enter the ID of the customer to create qualification for {CustomerQualification.Education}");

            var partnerOperations = this.Context.UserPartnerOperations;
            this.Context.ConsoleHelper.StartProgress("Creating customer qualification");

            /* The qualification value can be set to any allowed qualification, for example:
             * (1) "Education" <- this has to be paired with an education segment, and an optional website
             * (2) "GovernmentCommunityCloud" <- this has to be paired with a validation code, see sample in "CreateCustomerQualificationWithGCC.cs"
             * (3) "StateOwnedEntity"
             */ 
            
            var customerQualificationRequest = new Models.Customers.V2.CustomerQualificationRequest
            {
                Qualification = "Education",
                EducationSegment = "HigherEducation", // can also be "K12"
                Website = "example.edu"
            };

            var createCustomerQualification = partnerOperations.Customers.ById(customerIdToRetrieve).Qualification.CreateQualifications(customerQualificationRequest);

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(createCustomerQualification, "Customer Qualification");
        }
    }
}
