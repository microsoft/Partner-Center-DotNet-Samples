// <copyright file="CustomerProvider.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MCARefreshBulkAttestationCLITool.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using CsvHelper;
    using MCARefreshBulkAttestationCLITool.Interfaces;
    using MCARefreshBulkAttestationCLITool.Models;
    using Microsoft.Extensions.Logging;
    using Refit;

    public class CustomerProvider : ICustomerProvider
    {
        private const string FileName = "CustomerAgreementRecords.csv";
        private const int BatchSize = 500;

        private readonly ILogger<CustomerProvider> logger;
        private readonly ICustomerAgreementsClient customerAgreementsClient;
        private readonly IFileProvider fileProvider;

        public CustomerProvider(ICustomerAgreementsClient customerAgreementsClient, IFileProvider fileProvider, ILogger<CustomerProvider> logger)
        {
            this.fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
            this.customerAgreementsClient = customerAgreementsClient ?? throw new ArgumentNullException(nameof(customerAgreementsClient));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> FetchAndSaveCustomerAgreementRecords()
        {
            this.logger.LogInformation("Fetch operation triggered");

            var filePath = this.AcquireChosenFilePath();

            this.logger.LogInformation("Chosen file path: {0}", filePath);

            Console.WriteLine("\nFetching customer agreement records from Partner Center...");

            long totalRecords = 0;

            using (var streamWriter = new StreamWriter(filePath))
            using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
            {
                csvWriter.Context.RegisterClassMap<CustomerAgreementRecordWriterMap>();

                try
                {
                    this.logger.LogInformation("Dispatching fetch call...");

                    var result = await this.customerAgreementsClient.GetCustomerAgreementRecords();
                    totalRecords += result.CustomerAgreementRecords.Count;

                    this.logger.LogInformation("Successfully fetched {0} records.", result.CustomerAgreementRecords.Count);

                    await csvWriter.WriteRecordsAsync(result.CustomerAgreementRecords);
                    
                    while (!string.IsNullOrWhiteSpace(result.ContinuationToken))
                    {
                        Console.Write($"[In Progress] Retrieved {totalRecords}...\r");

                        this.logger.LogInformation("Dispatching fetch continuation call...");

                        result = await this.customerAgreementsClient.GetCustomerAgreementRecords(result.ContinuationToken);
                        totalRecords += result.CustomerAgreementRecords.Count;

                        this.logger.LogInformation("Successfully fetched {0} additional records.", result.CustomerAgreementRecords.Count);

                        await csvWriter.WriteRecordsAsync(result.CustomerAgreementRecords);
                    }
                }
                catch (ApiException ex)
                {
                    this.logger.LogError(ex, "An API exception occurred with status code: {0}, Content: {1}, Correlation ID: {2}, Request ID: {3}", ex.StatusCode, ex.Content, ex.RequestMessage.Headers?.GetValues("MS-CorrelationId").First(), ex.RequestMessage.Headers?.GetValues("MS-RequestId").First());

                    Console.WriteLine($"Failed to dispatch requests for records with status code: {ex.StatusCode}");
                    Console.WriteLine($"Exception message: {ex.Message}");
                    Console.WriteLine("Check the log file for more details.");

                    return false;
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "Exception incurred while trying to fetch customer agreement records");

                    Console.WriteLine($"Failed to fetch customer agreement records due to an unexpected exception. See log files for more details.");

                    return false;
                }
            }

            Console.WriteLine($"[Complete] Fetched {totalRecords} customer agreement records from Partner Center.");
            this.logger.LogInformation("Successfully fetched {0} total records.", totalRecords);

            return true;
        }

        public async Task<bool> UpdateCustomerAgreementRecords(string partnerTenantId)
        {
            List<ReAttestationRequest> transformedRecords = new();

            var filePath = this.AcquireChosenFilePath();

            try
            {
                var records = await this.ReadRecordsFromLocalFile(filePath);
                Console.WriteLine($"Reading {records.Count} records from local file...");

                transformedRecords = records.Where(car => car.PartnerAttestationCompleted).Select<CustomerAgreementRecord, ReAttestationRequest>(
                    car => new ReAttestationRequest
                    {
                        PartnerId = partnerTenantId,
                        CustomerId = car.CustomerTenantId,
                        CustomerDirectAcceptance = false,
                        Agreement = new Agreement
                        {
                            PrimaryContact = new LastAgreementPrimaryContact
                            {
                                FirstName = car.LastAgreementPrimaryContact.FirstName,
                                LastName = car.LastAgreementPrimaryContact.LastName,
                                Email = car.LastAgreementPrimaryContact.Email,
                                PhoneNumber = car.LastAgreementPrimaryContact.PhoneNumber,
                            },
                            DateAgreed = DateTime.UtcNow,
                            TemplateId = Agreement.MicrosoftCustomerAgreementTemplateId,
                        }
                    }).ToList();

                Console.WriteLine($"Processed {transformedRecords.Count} actionable records.");
            }
            catch
            {
                Console.WriteLine($"Failed to read records from local file. Please ensure that the file is not in use by another application and exists at:\n{filePath}");
                return false;
            }

            Console.WriteLine($"Dispatching requests...");

            List<ReAttestationRequest> batch = new();

            try
            {
                for (int i = 1; i < transformedRecords.Count + 1; i++)
                {
                    if (i % BatchSize == 0)
                    {
                        try
                        {
                            await this.customerAgreementsClient.CreateBulkReAttestation(batch);
                        }
                        catch (ApiException ex)
                        {
                            this.logger.LogError(ex, "An API exception occurred with status code: {0}, Content: {1}, Correlation ID: {2}, Request ID: {3}", ex.StatusCode, ex.Content, ex.RequestMessage.Headers?.GetValues("MS-CorrelationId").First(), ex.RequestMessage.Headers?.GetValues("MS-RequestId").First());

                            Console.WriteLine($"Failed to dispatch requests for records with status code: {ex.StatusCode}");
                            Console.WriteLine($"Exception message: {ex.Message}");
                            Console.WriteLine("Check the log file for more details.");

                            throw;
                        }

                        Console.WriteLine($"Processed {i}/{transformedRecords.Count} items...");
                        batch.Clear();
                    }
                    else
                    {
                        batch.Add(transformedRecords[i-1]);
                    }
                }

                if (batch.Any())
                {
                    try
                    {
                        await this.customerAgreementsClient.CreateBulkReAttestation(batch);
                    }
                    catch (ApiException ex)
                    {
                        this.logger.LogError(ex, "An API exception occurred with status code: {0}, Content: {1}, Correlation ID: {2}, Request ID: {3}", ex.StatusCode, ex.Content, ex.RequestMessage.Headers?.GetValues("MS-CorrelationId").First(), ex.RequestMessage.Headers?.GetValues("MS-RequestId").First());

                        Console.WriteLine($"Failed to dispatch requests for records with status code: {ex.StatusCode}");
                        Console.WriteLine($"Exception message: {ex.Message}");
                        Console.WriteLine("Check the log file for more details.");

                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to dispatch requests for records. Please try again later.");
                return false;
            }

            return true;
        }

        private string AcquireChosenFilePath()
        {
            var directoryPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            
            Console.WriteLine($"Enter a desired absolute path to an existing directory to read/write the agreement records.");
            Console.WriteLine($"Default directory [Enter to Accept]: {directoryPath}");

            var input = Console.ReadLine();
            directoryPath = !string.IsNullOrWhiteSpace(input) && Directory.Exists(input) ? input : directoryPath;

            return Path.Combine(directoryPath, FileName);
        }

        private async Task<List<CustomerAgreementRecord>> ReadRecordsFromLocalFile(string filePath)
        {
            Console.WriteLine("Reading records from disk...");

            var records = await this.fileProvider.ReadFromLocalFile(filePath);

            Console.WriteLine("Done reading records from disk.");

            return records.ToList();
        }
    }
}
