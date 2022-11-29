// -----------------------------------------------------------------------
// <copyright file="ValidateAndCreateNewCommerceMigration.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.NewCommerceMigrations
{
    using Microsoft.Store.PartnerCenter.Models.NewCommerceMigrations;

    /// <summary>
    /// A scenario that validates a New-Commerce migration and then creates it.
    /// </summary>
    public class ValidateAndCreateNewCommerceMigration : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateAndCreateNewCommerceMigration"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public ValidateAndCreateNewCommerceMigration(IScenarioContext context) : base("Validate and create a New-Commerce migration", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;

            string customerId = this.ObtainCustomerId("Enter the ID of the customer making the purchase");
            string subscriptionId = this.ObtainSubscriptionId(customerId, "Enter the ID of the subscription to be migrated to New-Commerce");
            string termDuration = this.ObtainRenewalTermDuration("Enter a term duration for the subscription [example: P1Y, P1M]");
            string billingCycle = this.ObtainBillingCycle("Enter a billing cycle for the subscription [example: Annual or Monthly]");
            string quantityString = this.ObtainQuantity("Enter the quantity for the subscription");
            var quantity = int.Parse(quantityString);
            var customTermEndDate = this.ObtainCustomTermEndDate("Enter the custom term end date for the subscription or leave blank to keep default");

            var newCommerceMigration = new NewCommerceMigration()
            {
                CurrentSubscriptionId = subscriptionId,
                TermDuration = termDuration,
                BillingCycle = billingCycle,
                Quantity = quantity,
                CustomTermEndDate = customTermEndDate,
            };

            var newCommerceMigrationOperations = partnerOperations.Customers.ById(customerId).NewCommerceMigrations;

            this.Context.ConsoleHelper.StartProgress("Validating New-Commerce migration");
            var newCommerceEligibility = newCommerceMigrationOperations.Validate(newCommerceMigration);

            this.Context.ConsoleHelper.WriteObject(newCommerceEligibility, "New-Commerce eligibility for the specified subscription");

            this.Context.ConsoleHelper.StopProgress();

            if (newCommerceEligibility.IsEligible)
            {
                this.Context.ConsoleHelper.StartProgress("Creating New-Commerce migration");
                newCommerceMigration = newCommerceMigrationOperations.Create(newCommerceMigration);
                this.Context.ConsoleHelper.WriteObject(newCommerceEligibility, "New-Commerce migration created for the specified subscription");
                newCommerceMigration = newCommerceMigrationOperations.ById(newCommerceMigration.Id).Get();
                this.Context.ConsoleHelper.WriteObject(newCommerceMigration, "Final New-Commerce migration");
                this.Context.ConsoleHelper.StopProgress();
            }
            else
            {
                this.Context.ConsoleHelper.Warning("The specified subscription is not eligibile for migrating to New-Commerce.");
            }
        }
    }
}