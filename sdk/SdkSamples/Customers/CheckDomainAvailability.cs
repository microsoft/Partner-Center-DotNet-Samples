// -----------------------------------------------------------------------
// <copyright file="CheckDomainAvailability.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Customers
{
    /// <summary>
    /// A scenario that checks if a domain is still available for a customer or not.
    /// </summary>
    public class CheckDomainAvailability : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckDomainAvailability"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public CheckDomainAvailability(IScenarioContext context) : base("Check domain availability", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;
            string domainPrefix = this.Context.ConsoleHelper.ReadNonEmptyString("Enter a domain prefix to check its availability", "The entered domain is empty");

            this.Context.ConsoleHelper.StartProgress("Checking");
            bool isDomainAvailable = !partnerOperations.Domains.ByDomain(domainPrefix).Exists();
            this.Context.ConsoleHelper.StopProgress();

            if (isDomainAvailable)
            {
                this.Context.ConsoleHelper.Success("This domain prefix is available!");
            }
            else
            {
                this.Context.ConsoleHelper.Warning("This domain prefix is unavailable.");
            }
        }
    }
}
