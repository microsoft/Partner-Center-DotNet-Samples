// -----------------------------------------------------------------------
// <copyright file="ValidateAndCreateNewCommerceMigrationWithAddOn.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.NewCommerceMigrations
{
    using Microsoft.Store.PartnerCenter.Models.NewCommerceMigrations;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A scenario that validates a New-Commerce migration with an add-on and then creates it.
    /// </summary>
    public class ValidateAndCreateNewCommerceMigrationWithAddOn : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateAndCreateNewCommerceMigrationWithAddOn"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public ValidateAndCreateNewCommerceMigrationWithAddOn(IScenarioContext context) : base("Validate and create a New-Commerce migration with add-on", context)
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

            string addOnSubscriptionId = this.ObtainSubscriptionId(customerId, "Enter the ID of the add-on subscription to be migrated to New-Commerce");

            // Get the add-on subscription and display information.
            var addOnSubscription = partnerOperations.Customers.ById(customerId).Subscriptions.ById(addOnSubscriptionId).Get();
            this.Context.ConsoleHelper.WriteObject(addOnSubscription, "Add-on Subscriptions");
            Console.WriteLine();

            string addOnSubscriptionTermDuration = this.ObtainRenewalTermDuration("Enter a term duration for the add-on subscription [example: P1Y, P1M]");
            string addOnSubscriptionBillingCycle = this.ObtainBillingCycle("Enter a billing cycle for the add-on subscription [example: Annual or Monthly]");
            string addOnSubscriptionQuantityString = this.ObtainQuantity("Enter the quantity for the add-on subscription");
            var addOnSubscriptionQuantity = int.Parse(addOnSubscriptionQuantityString);

            var newCommerceMigration = new NewCommerceMigration
            {
                CurrentSubscriptionId = subscriptionId,
                AddOnMigrations = new List<NewCommerceMigration>
                {
                    new NewCommerceMigration
                    {
                        CurrentSubscriptionId = addOnSubscriptionId,
                        TermDuration = addOnSubscriptionTermDuration,
                        BillingCycle = addOnSubscriptionBillingCycle,
                        Quantity = addOnSubscriptionQuantity,
                    }
                },
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