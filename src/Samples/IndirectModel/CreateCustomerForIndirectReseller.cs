// -----------------------------------------------------------------------
// <copyright file="CreateCustomerForIndirectReseller.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Customers
{
    using System;
    using System.Globalization;
    using Models;
    using Models.Customers;

    /// <summary>
    /// Creates a new customer for an indirect reseller.
    /// </summary>
    public class CreateCustomerForIndirectReseller : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCustomerForIndirectReseller"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public CreateCustomerForIndirectReseller(IScenarioContext context) : base("Create a new customer for an indirect reseller", context)
        {
        }

        /// <summary>
        /// executes the create customer scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var indirectResellerId = this.ObtainIndirectResellerId("Enter the ID of the indirect reseller: ");

            var partnerOperations = this.Context.UserPartnerOperations;

            var customerToCreate = new Customer()
            {
                CompanyProfile = new CustomerCompanyProfile()
                {
                    Domain = string.Format(
                        CultureInfo.InvariantCulture,
                        "WingtipToys{0}.{1}",
                        new Random().Next(), 
                        this.Context.Configuration.Scenario.CustomerDomainSuffix)
                },
                BillingProfile = new CustomerBillingProfile()
                {
                    Culture = "EN-US",
                    Email = "Gena@wingtiptoys.com",
                    Language = "En",
                    CompanyName = "Wingtip Toys" + new Random().Next(),
                    DefaultAddress = new Address()
                    {
                        FirstName = "Gena",
                        LastName = "Soto",
                        AddressLine1 = "One Microsoft Way",
                        City = "Redmond",
                        State = "WA",
                        Country = "US",
                        PostalCode = "98052",
                        PhoneNumber = "4255550101"
                    }
                },
                AssociatedPartnerId = indirectResellerId
            };

            this.Context.ConsoleHelper.WriteObject(customerToCreate, "New user Information");
            this.Context.ConsoleHelper.StartProgress("Creating user");

            var newCustomer = partnerOperations.Customers.Create(customerToCreate);

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.Success("Success!");
            this.Context.ConsoleHelper.WriteObject(newCustomer, "Created user Information");
        }
    }
}
