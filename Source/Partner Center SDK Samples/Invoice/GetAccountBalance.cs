// -----------------------------------------------------------------------
// <copyright file="GetAccountBalance.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Invoice
{
    /// <summary>
    /// Gets the account balance for a partner.
    /// </summary>
    public class GetAccountBalance : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetAccountBalance"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetAccountBalance(IScenarioContext context) : base("Get account balance", context)
        {
        }

        /// <summary>
        /// executes the get account balance scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;
            this.Context.ConsoleHelper.StartProgress("Getting account balance");

            // Getting the account balance
            var accountBalance = partnerOperations.Invoices.Summary.Get().BalanceAmount;

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(accountBalance, "Account Balance");
        }
    }
}
