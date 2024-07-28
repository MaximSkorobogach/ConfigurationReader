using System.Xml;
using System.Xml.Serialization;
using ConfigurationReader.Infrastructure.Consts;
using ConfigurationReader.Infrastructure.DTO;
using ConfigurationReader.Infrastructure.Exceptions;
using ConfigurationReader.Infrastructure.Parsers.Abstracts;
using Microsoft.Extensions.Logging;

namespace ConfigurationReader.Infrastructure.Parsers;

public class XmlConfigurationParser : BaseConfigurationParser
{
    public XmlConfigurationParser(ILogger<XmlConfigurationParser> logger) : base(logger)
    {
    }

    protected override Configuration? GetConfigurationRecord(byte[] fileBytes)
    {
        using var memoryStream = new MemoryStream(fileBytes);
        var serializer = new XmlSerializer(typeof(Configuration));
        var configuration = (Configuration)serializer.Deserialize(memoryStream);

        return configuration;
    }
}