// -----------------------------------------------------------------------
// <copyright file="UpdateCustomerBillingProfile.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Customers
{
    /// <summary>
    /// A scenario that updates a customer's billing profile.
    /// </summary>
    public class UpdateCustomerBillingProfile : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCustomerBillingProfile"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public UpdateCustomerBillingProfile(IScenarioContext context) : base("Update customer billing profile", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            string customerId = this.ObtainCustomerId();

            IAggregatePartner partnerOperations = this.Context.UserPartnerOperations;
            this.Context.ConsoleHelper.StartProgress("Getting customer billing profile");

            Models.Customers.CustomerBillingProfile billingProfile = partnerOperations.Customers.ById(customerId).Profiles.Billing.Get();

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(billingProfile, "Customer billing profile");

            this.Context.ConsoleHelper.StartProgress("Updating the customer billing profile");

            // append some A's to some of the customer's billing profile properties
            billingProfile.FirstName = billingProfile.FirstName + "A";
            billingProfile.LastName = billingProfile.LastName + "A";
            billingProfile.CompanyName = billingProfile.CompanyName + "A";

            // update the billing profile
            Models.Customers.CustomerBillingProfile updatedBillingProfile = partnerOperations.Customers.ById(customerId).Profiles.Billing.Update(billingProfile);

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(updatedBillingProfile, "Updated customer billing profile");
        }
    }
}
