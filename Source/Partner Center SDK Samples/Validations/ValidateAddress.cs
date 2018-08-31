// -----------------------------------------------------------------------
// <copyright file="ValidateAddress.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Validations
{
    using System;
    using Models;

    /// <summary>
    /// A scenario that showcases address validation functionality.
    /// </summary>
    public class ValidateAddress : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateAddress"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public ValidateAddress(IScenarioContext context) : base("Validate address.", context)
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

            try
            {
                // Validate the address
                var validationResult = partnerOperations.Validations.IsAddressValidAsync(address).Result;
                this.Context.ConsoleHelper.StopProgress();
                Console.WriteLine(validationResult ? "The address is valid." : "Invalid address");
            }
            catch (Exception exception)
            {
                this.Context.ConsoleHelper.StopProgress();
                Console.WriteLine("Address is invalid");
                var innerException = exception.InnerException;
                if (innerException != null)
                {
                    while (innerException != null)
                    {
                        this.Context.ConsoleHelper.WriteObject(innerException.Message);
                        innerException = innerException.InnerException;
                    }
                }
                else if (!string.IsNullOrWhiteSpace(exception.Message))
                {
                    this.Context.ConsoleHelper.WriteObject(exception.Message);
                }
            }
        }
    }
}
