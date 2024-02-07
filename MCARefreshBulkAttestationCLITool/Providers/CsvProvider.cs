// <copyright file="CsvProvider.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MCARefreshBulkAttestationCLITool.Providers
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using CsvHelper;
    using CsvHelper.Configuration;
    using MCARefreshBulkAttestationCLITool.Interfaces;
    using MCARefreshBulkAttestationCLITool.Models;
    using Microsoft.Extensions.Logging;

    public class CsvProvider : IFileProvider
    {
        private readonly ILogger<CsvProvider> logger;

        public CsvProvider(ILogger<CsvProvider> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<CustomerAgreementRecord>> ReadFromLocalFile(string fileName)
        {
            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Encoding = Encoding.UTF8,
                Delimiter = ","
            };

            var records = new List<CustomerAgreementRecord>();

            try
            {
                using (var fs = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (var textReader = new StreamReader(fs, Encoding.UTF8))
                    using (var csv = new CsvReader(textReader, configuration))
                    {
                        csv.Context.RegisterClassMap<CustomerAgreementRecordReaderMap>();

                        var data = csv.GetRecordsAsync<CustomerAgreementRecord>();

                        await foreach (var customerAgreementRecord in data)
                        {
                            records.Add(customerAgreementRecord);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Exception incurred while trying to read CSV");
                throw;
            }

            return records;
        }
    }

    internal class CustomerAgreementRecordReaderMap : ClassMap<CustomerAgreementRecord>
    {
        public CustomerAgreementRecordReaderMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
            Map(m => m.LastAgreementDateEpoch).Ignore();
            Map(m => m.CustomerAccountLink).Ignore();
        }
    }

    internal class CustomerAgreementRecordWriterMap : ClassMap<CustomerAgreementRecord>
    {
        public CustomerAgreementRecordWriterMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
            Map(m => m.LastAgreementDateEpoch).Ignore();
        }
    }
}
