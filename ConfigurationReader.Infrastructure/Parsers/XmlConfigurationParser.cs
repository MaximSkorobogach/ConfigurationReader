using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using ConfigurationReader.Infrastructure.DTO;
using ConfigurationReader.Infrastructure.Parsers.Abstracts;
using Microsoft.Extensions.Logging;

namespace ConfigurationReader.Infrastructure.Parsers;

internal class XmlConfigurationParser(ILogger<XmlConfigurationParser> logger) : BaseConfigurationParser(logger)
{
    protected override async Task<Configuration?> GetConfigurationRecordAsync(byte[] fileBytes)
    {
        using var memoryStream = new MemoryStream(fileBytes);
        using var xmlReader = XmlReader.Create(memoryStream, new XmlReaderSettings { Async = true });

        var document = await XDocument.LoadAsync(xmlReader, LoadOptions.None, CancellationToken.None);
        var serializer = new XmlSerializer(typeof(Configuration));

        using var reader = document.CreateReader();
        var configuration = (Configuration?)serializer.Deserialize(reader);

        return configuration;
    }
}