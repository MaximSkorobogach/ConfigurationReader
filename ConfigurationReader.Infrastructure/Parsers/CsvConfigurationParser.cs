﻿using ConfigurationReader.Infrastructure.Parsers.Abstracts;
using System.Globalization;
using ConfigurationReader.Infrastructure.DTO;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Logging;

namespace ConfigurationReader.Infrastructure.Parsers
{
    public class CsvConfigurationParser : BaseConfigurationParser
    {
        public CsvConfigurationParser(ILogger<CsvConfigurationParser> logger) : base(logger)
        {
        }

        protected override async Task<Configuration?> GetConfigurationRecordAsync(byte[] fileBytes)
        {
            using var memoryStream = new MemoryStream(fileBytes);
            using var reader = new StreamReader(memoryStream);
            using var csvReader = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HasHeaderRecord = true,
            });

            var configurations = new List<Configuration>();
            await foreach (var record in csvReader.GetRecordsAsync<Configuration>())
            {
                configurations.Add(record);
            }

            var configuration = configurations.SingleOrDefault();

            return configuration;
        }
    }
}
