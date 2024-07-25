using ConfigurationReader.Infrastructure.Parsers.Abstracts;
using System.Formats.Asn1;
using System.Globalization;
using ConfigurationReader.Infrastructure.Consts;
using ConfigurationReader.Infrastructure.DTO;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Logging;
using ConfigurationReader.Infrastructure.Exceptions;

namespace ConfigurationReader.Infrastructure.Parsers;

public class CsvConfigurationParser : BaseConfigurationParser
{
    public CsvConfigurationParser(ILogger<CsvConfigurationParser> logger) : base(logger)
    {
    }

    protected override Configuration GetConfigurationRecord(byte[] fileBytes)
    {
        using var memoryStream = new MemoryStream(fileBytes);
        using var reader = new StreamReader(memoryStream);
        using var csvReader = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";",
            HasHeaderRecord = true,
        });

        var configurations = csvReader.GetRecords<Configuration>();
        var configuration = configurations.FirstOrDefault();

        return configuration;
    }
}