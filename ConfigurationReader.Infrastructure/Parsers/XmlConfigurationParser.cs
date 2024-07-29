using System.Xml.Serialization;
using ConfigurationReader.Infrastructure.DTO;
using ConfigurationReader.Infrastructure.Parsers.Abstracts;
using Microsoft.Extensions.Logging;

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
            var serializer = new XmlSerializer(typeof(Configuration));

            var configuration = await Task.Run(() => (Configuration?)serializer.Deserialize(memoryStream));

            return configuration;
        }
    }
}