// <copyright file="ImportCustomersAgreement.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Agreements
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.IO;
    using Models.Agreements;

    /// <summary>
    /// Import Customers' agreements.
    /// </summary>
    public class ImportCustomersAgreement : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportCustomersAgreement"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public ImportCustomersAgreement(IScenarioContext context) : base("Import all customers' agreements.", context)
        {
        }

        /// <summary>
        /// Executes the import customer agreements scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var startTime = DateTime.UtcNow;
            var errorFilePath = $"{DateTime.UtcNow:yyyyMMddTHHmmss}.txt";
            var count = 0;

            var partnerOperations = this.Context.UserPartnerOperations;

            // Prefetch necessary partner agreement metadata
            var agreementDetails = partnerOperations.AgreementDetails.ByAgreementType("*")?.Get()?.Items.OrderBy(x => x.VersionRank).ToDictionary(ad => ad.AgreementType);
            if (agreementDetails == null || !agreementDetails.Any())
            {
                this.Context.ConsoleHelper.WriteColored("No Agreement metadata available.", ConsoleColor.DarkRed);
                return;
            }

            this.Context.ConsoleHelper.WriteColored($"{Environment.NewLine}Use GetAllCustomersAgreements scenario's output csv file format to import agreements.", ConsoleColor.DarkGray);
            var csvFilePath = this.ObtainCustomersAgreementCsvFileName();
            var customerAgreements = this.ParseCustomerAgreements(csvFilePath, errorFilePath);

            // Perform basic validations to check for duplicate entries (note: there can be multiple entries per customer tenant ID, just not duplicates).
            if (customerAgreements.Where(x => x.Valid).GroupBy(x => x.CustomerTenantId).Any(c => c.Count() > c.Select(ca => ca.Agreement.Type).Distinct(StringComparer.OrdinalIgnoreCase).Count()))
            {
                this.Context.ConsoleHelper.WriteColored("File contains duplicate / contradicting agreement data. Please fix and retry.", ConsoleColor.DarkRed);
                return;
            }

            // Process each line
            foreach (var customerAgreement in customerAgreements)
            {
                this.Context.ConsoleHelper.WriteObject($"Processing #{++count} {customerAgreement.Source}");

                if (!customerAgreement.Valid)
                {
                    File.AppendAllText(errorFilePath,
                        $"{customerAgreement.Source},Insufficient data.{Environment.NewLine}");
                    continue;
                }

                try
                {
                    // Fetch Agreements (of a specific type) for the customer to check if an update is necessary.
                    var customerAgreementOperations = partnerOperations.Customers.ById(customerAgreement.CustomerTenantId).Agreements;
                    var agreements = customerAgreementOperations.ByAgreementType(customerAgreement.Agreement.Type).Get();
                    if (agreements.TotalCount == 0 ||
                        DoesAgreementNeedUpdate(agreements.Items.First(), customerAgreement.Agreement))
                    {
                        var agreementDetail = agreementDetails[customerAgreement.Agreement.Type];

                        // Populate other required agreement details
                        customerAgreement.Agreement.AgreementLink = agreementDetail.AgreementLink;
                        customerAgreement.Agreement.TemplateId = agreementDetail.TemplateId;
                        customerAgreement.Agreement.Type = agreementDetail.AgreementType;

                        // Try to add the agreement
                        customerAgreementOperations.Create(customerAgreement.Agreement);
                        File.AppendAllText(errorFilePath,
                            $"{customerAgreement.Source},Agreement data updated successfully.{Environment.NewLine}");
                    }
                    else
                    {
                        File.AppendAllText(errorFilePath,
                            $"{customerAgreement.Source},Skipped as there is no change in agreement data.{Environment.NewLine}");
                    }
                }
                catch (Exception ex)
                {
                    File.AppendAllText(errorFilePath,
                        $"{customerAgreement.Source},Processing failed with error {ex.Message}{Environment.NewLine}");
                }
            }

            this.Context.ConsoleHelper.WriteObject($"Total Customers: {count} processed in {DateTime.UtcNow - startTime}.");
            this.Context.ConsoleHelper.WriteObject($"{errorFilePath}", "Processed information output file");
        }

        /// <summary>
        /// Compare the existing and to be imported agreement to check if any updates are required.
        /// </summary>
        /// <param name="existingAgreement">Existing agreement</param>
        /// <param name="importAgreement">To be imported agreement</param>
        /// <returns>Whether the agreement needs update?</returns>
        private static bool DoesAgreementNeedUpdate(Agreement existingAgreement, Agreement importAgreement)
        {
            // Check if Agreement data is the same (note: case matters here and that's why the following checks are case sensitive).
            return !string.Equals(existingAgreement.PrimaryContact.FirstName, importAgreement.PrimaryContact.FirstName) ||
                   !string.Equals(existingAgreement.PrimaryContact.LastName, importAgreement.PrimaryContact.LastName) ||
                   !string.Equals(existingAgreement.PrimaryContact.PhoneNumber ?? string.Empty, importAgreement.PrimaryContact.PhoneNumber ?? string.Empty) ||
                   !string.Equals(existingAgreement.PrimaryContact.Email, importAgreement.PrimaryContact.Email);
        }

        /// <summary>
        /// Parse CSV customer agreement file
        /// </summary>
        /// <param name="csvFilePath">CSV Customer agreements file path</param>
        /// <param name="errorFilePath">Error File path</param>
        /// <returns></returns>
        private List<CustomerAgreement> ParseCustomerAgreements(string csvFilePath, string errorFilePath)
        {
            var lines = File.ReadAllLines(csvFilePath).ToList();
            var customerAgreements = new List<CustomerAgreement>();

            switch (lines.Count)
            {
                case 0:
                    this.Context.ConsoleHelper.WriteColored($"{Environment.NewLine}No agreements found.", ConsoleColor.DarkRed);
                    return customerAgreements;
                default:
                    this.Context.ConsoleHelper.WriteObject(lines[0], $"{Environment.NewLine}Header (first line)");
                    File.WriteAllText(errorFilePath, $"{lines[0]}{Environment.NewLine}");
                    break;
            }

            // Construct customer agreement from csv line.
            for (var ptr = 1; ptr < lines.Count; ptr++)
            {
                var parts = lines[ptr].Split(',').Select(s => s.Trim()).ToList();

                bool validLine = !(parts.Count < 8 || string.IsNullOrWhiteSpace(parts[0])
                                    || string.IsNullOrWhiteSpace(parts[1])
                                    || string.IsNullOrWhiteSpace(parts[2])
                                    || string.IsNullOrWhiteSpace(parts[3])
                                    || string.IsNullOrWhiteSpace(parts[4])
                                    || string.IsNullOrWhiteSpace(parts[6])
                                    || string.IsNullOrWhiteSpace(parts[7]));

                if (validLine)
                {
                    customerAgreements.Add(new CustomerAgreement
                    {
                        Source = lines[ptr],
                        Valid = true,
                        CustomerTenantId = parts[0],
                        CustomerDomainName = parts[1],
                        Agreement = new Agreement
                        {
                            DateAgreed = DateTime.Parse(parts[2]),
                            PrimaryContact = new Contact
                            {
                                FirstName = parts[3],
                                LastName = parts[4],
                                PhoneNumber = parts[5],
                                Email = parts[6]
                            },
                            Type = parts[7]
                        }
                        ,
                    });
                }
                else
                {
                    customerAgreements.Add(new CustomerAgreement
                    {
                        Source = lines[ptr],
                        Valid = false,
                    });
                }
            }

            return customerAgreements;
        }

        /// <summary>
        /// Customer Agreement class to store csv agreement data
        /// </summary>
        public class CustomerAgreement
        {
            /// <summary>
            /// Gets or sets agreement data
            /// </summary>
            public Agreement Agreement { get; set; }

            /// <summary>
            /// Gets or sets source line from CSV file (unformatted)
            /// </summary>
            public string Source { get; set; }

            /// <summary>
            /// Gets or sets Customer Tenant Id (First column from csv line)
            /// </summary>
            public string CustomerTenantId { get; set; }

            /// <summary>
            /// Gets or sets Customer Domain Name (Second column from csv line)
            /// </summary>
            public string CustomerDomainName { get; set; }

            /// <summary>
            /// Gets or sets whether the customer agreement data has all required information?
            /// </summary>
            public bool Valid { get; set; }
        }
    }
}
