using ConfigurationReader.Infrastructure.DTO;
using ConfigurationReader.Infrastructure.Parsers.Abstracts;
using Microsoft.Extensions.Logging;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace ConfigurationReader.Infrastructure.Parsers
{
    public class XmlConfigurationParser : BaseConfigurationParser
    {
        public XmlConfigurationParser(ILogger<XmlConfigurationParser> logger) : base(logger)
        {
        }

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
}