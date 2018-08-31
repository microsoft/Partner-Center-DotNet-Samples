// -----------------------------------------------------------------------
// <copyright file="ValidateCustomerAddress.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Customers
{
    /// <summary>
    /// A scenario that showcases country validation rules.
    /// </summary>
    public class ValidateCustomerAddress : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateCustomerAddress"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public ValidateCustomerAddress(IScenarioContext context) : base("Validate customer address.", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;
            string countryCode = this.Context.ConsoleHelper.ReadNonEmptyString("Enter the 2 digit country code to get its validation rules", "The country code can't be empty");

            this.Context.ConsoleHelper.StartProgress("Retrieving country validation rules");
            var countryValidationRules = partnerOperations.CountryValidationRules.ByCountry(countryCode).Get();
            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(countryValidationRules, " Country validation rules");
        }
    }
}
