// <copyright file="GetAllCustomersAgreements.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Agreements
{
    using System;
    using System.Linq;
    using System.IO;
    using Exceptions;
    using Models.Agreements;
    using Models.Customers;
    using Models.Query;

    /// <summary>
    /// Showcases the retrieval of all customers' agreements.
    /// </summary>
    public class GetAllCustomersAgreements : BasePartnerScenario
    {
        /// <summary>
        /// The customer page size.
        /// </summary>
        private readonly int customerPageSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllCustomersAgreements"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        /// <param name="customerPageSize">The number of customers to return per page.</param>
        public GetAllCustomersAgreements(IScenarioContext context, int customerPageSize = 0) : base("Get all Customers' agreements.", context)
        {
            this.customerPageSize = customerPageSize;
        }

        /// <summary>
        /// Executes the get customer agreements scenario.
        /// </summary>
        protected override void RunScenario()
        {
            const string noAgreements = "No agreements found.";
            var partnerOperations = this.Context.UserPartnerOperations;

            var csvFilePath = this.ObtainCustomersAgreementCsvFileName();
            File.WriteAllText(csvFilePath, $"TenantId,Domain,Date,First Name,Last Name,Phone,Email {Environment.NewLine}");

            // query the customers, get the first page if a page size was set, otherwise get all customers
            var customersPage = (this.customerPageSize <= 0) ? partnerOperations.Customers.Get() : partnerOperations.Customers.Query(QueryFactory.Instance.BuildIndexedQuery(this.customerPageSize));

            // create a customer enumerator which will aid us in traversing the customer pages
            var customersEnumerator = partnerOperations.Enumerators.Customers.Create(customersPage);

            var count = 0;
            var startTime = DateTime.UtcNow;

            while (customersEnumerator.HasValue)
            {
                foreach (var customer in customersEnumerator.Current.Items)
                {
                    try
                    {
                        // Fetch customer agreements
                        this.Context.ConsoleHelper.WriteObject($"#{++count} Tenant: {customer?.CompanyProfile?.TenantId ?? customer?.Id}, Domain: {customer?.CompanyProfile?.Domain ?? "Domain not available." }", "Customer");
                        var customerAgreements = partnerOperations.Customers.ById(customer.Id).Agreements.ByAgreementType("*").Get();

                        if (!customerAgreements.Items.Any())
                        {
                            this.Context.ConsoleHelper.WriteObject(noAgreements, "Agreement", 1);
                            File.AppendAllText(csvFilePath, $"{customer?.CompanyProfile?.TenantId ?? customer?.Id} ,{customer?.CompanyProfile?.Domain ?? "Domain not available."},,,,,{Environment.NewLine}");
                        }
                        else
                        {
                            // Fetch the latest agreement, if there are multiple instances of the same type
                            var orderedCustomerAgreements = customerAgreements.Items.OrderByDescending(x => x.DateAgreed).ToArray();

                            this.AppendToFile(csvFilePath, customer, orderedCustomerAgreements, "MicrosoftCloudAgreement");
                            this.AppendToFile(csvFilePath, customer, orderedCustomerAgreements, "MicrosoftCustomerAgreement");
                        }
                    }
                    catch (PartnerException partnerException)
                    {
                        if (partnerException.ServiceErrorPayload.ErrorCode.Equals("600009", StringComparison.InvariantCultureIgnoreCase))
                        {
                            this.Context.ConsoleHelper.WriteObject(noAgreements, "Agreement", 1);
                            File.AppendAllText(csvFilePath, $"{customer?.CompanyProfile?.TenantId ?? customer?.Id},{customer?.CompanyProfile?.Domain ?? "Domain not available."},,,,,{Environment.NewLine}");
                        }
                    }
                }

                // get the next page of customers
                customersEnumerator.Next();
            }

            this.Context.ConsoleHelper.WriteObject($"Total Customers: {count} processed in {DateTime.UtcNow - startTime}.");
        }

        private void AppendToFile(string csvFilePath, Customer customer, Agreement[] orderedCustomerAgreements, string agreementType)
        {
            var customerAgreement = orderedCustomerAgreements.FirstOrDefault(oca => string.Equals(oca.Type, agreementType, StringComparison.OrdinalIgnoreCase));
            if (customerAgreement != null)
            {
                this.Context.ConsoleHelper.WriteObject($"Date: {customerAgreement.DateAgreed}, First Name: {customerAgreement.PrimaryContact.FirstName}, Last Name: {customerAgreement.PrimaryContact.LastName}, Phone: {customerAgreement.PrimaryContact.PhoneNumber}, Email: {customerAgreement.PrimaryContact.Email}", customerAgreement.Type, 1);
                File.AppendAllText(csvFilePath, $"{customer?.CompanyProfile?.TenantId ?? customer?.Id},{customer?.CompanyProfile?.Domain ?? "Domain not available." },{customerAgreement.DateAgreed},{customerAgreement.PrimaryContact.FirstName},{customerAgreement.PrimaryContact.LastName},{customerAgreement.PrimaryContact.PhoneNumber},{customerAgreement.PrimaryContact.Email},{customerAgreement.Type}{Environment.NewLine}");
            }
        }
    }
}
