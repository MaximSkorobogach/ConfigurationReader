using System.ComponentModel;

namespace ConfigurationReader.Infrastructure.Enums;

public enum ConfigurationFileType
{
    [Description(".xml")]
    xml,
    [Description(".csv")]
    csv
}