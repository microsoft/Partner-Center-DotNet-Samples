// -----------------------------------------------------------------------
// <copyright file="UpdateCustomerQualificationWithGCC.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Customers
{
    using System.Collections.Generic;
    using Microsoft.Store.PartnerCenter.Models.Customers;
    using Microsoft.Store.PartnerCenter.Models.ValidationCodes;

    /// <summary>
    /// Updates a single customer qualification to GCC.
    /// </summary>
    public class UpdateCustomerQualificationWithGCC : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCustomerQualificationWithGCC"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public UpdateCustomerQualificationWithGCC(IScenarioContext context) : base("Update customer qualification with GCC", context)
        {
        }

        /// <summary>
        /// Executes the update customer qualification scenario.
        /// </summary>
        protected override void RunScenario()
        {
            string customerIdToRetrieve = this.ObtainCustomerId($"Enter the ID of the customer to update qualification to {CustomerQualification.GovernmentCommunityCloud}");

            IAggregatePartner partnerOperations = this.Context.UserPartnerOperations;

            this.Context.ConsoleHelper.StartProgress("Retrieving validation codes");
            IEnumerable<ValidationCode> validations = partnerOperations.Validations.GetValidationCodes();
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

            this.Context.ConsoleHelper.StartProgress("Updating customer qualification");


            CustomerQualification customerQualification =
                partnerOperations.Customers.ById(customerIdToRetrieve)
                    .Qualification.Update(CustomerQualification.GovernmentCommunityCloud, code);

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(customerQualification, "Customer Qualification");
        }
    }
}
