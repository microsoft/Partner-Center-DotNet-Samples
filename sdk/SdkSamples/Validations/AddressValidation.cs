// -----------------------------------------------------------------------
// <copyright file="AddressValidation.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Validations
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Models;

    /// <summary>
    /// A scenario that showcases address validation functionality.
    /// </summary>
    public class AddressValidation : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddressValidation"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public AddressValidation(IScenarioContext context) : base("Validate address.", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;

            // Get the address from the console
            var address = new Address()
            {
                AddressLine1 = this.Context.ConsoleHelper.ReadNonEmptyString("Enter the address line 1", "Address line 1 can't be empty"),
                AddressLine2 = this.Context.ConsoleHelper.ReadOptionalString("Enter the address line 2 (optional)"),
                City = this.Context.ConsoleHelper.ReadNonEmptyString("Enter the city", "City can't be empty"),
                State = this.Context.ConsoleHelper.ReadNonEmptyString("Enter the 2 digit state code", "State code can't be empty"),
                Country = this.Context.ConsoleHelper.ReadNonEmptyString("Enter the 2 digit country code", "Country code can't be empty"),
                PostalCode = this.Context.ConsoleHelper.ReadNonEmptyString("Enter the postal/zip code", "Postal/zip code can't be empty")
            };

            this.Context.ConsoleHelper.StartProgress("Validating address");
            var addressValidationResult = partnerOperations.Validations.IsAddressValid(address);
            this.Context.ConsoleHelper.StopProgress();

            Console.WriteLine($"Status: {addressValidationResult.Status}");
            Console.WriteLine($"Original Address:\n{this.DisplayAddress(addressValidationResult.OriginalAddress)}");
            Console.WriteLine($"Validation Message Returned: {addressValidationResult.ValidationMessage ?? "No message returned."}");
            Console.WriteLine($"Suggested Addresses Returned: {addressValidationResult.SuggestedAddresses?.Count ?? "None."}");

            if (addressValidationResult.SuggestedAddresses != null && addressValidationResult.SuggestedAddresses.Any())
            {
                addressValidationResult.SuggestedAddresses.ForEach(a => Console.WriteLine(this.DisplayAddress(a)));
            }
        }

        private string DisplayAddress(Address address)
        {
            StringBuilder sb = new StringBuilder();
            
            foreach (var property in address.GetType().GetProperties())
            {
                sb.AppendLine($"{property.Name}: {property.GetValue(address) ?? "None to Display."}");
            }

            return sb.ToString();
        }
    }
}
