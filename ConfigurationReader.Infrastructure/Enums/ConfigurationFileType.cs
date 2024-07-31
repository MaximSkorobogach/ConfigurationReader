using System.ComponentModel;

namespace ConfigurationReader.Infrastructure.Enums;

public enum ConfigurationFileType
{
    [Description(".xml")] Xml,
    [Description(".csv")] Csv
}