using System.Text;
using ConfigurationReader.Infrastructure.Consts;
using ConfigurationReader.Infrastructure.Exceptions;
using ConfigurationReader.Infrastructure.Parsers;
using ConfigurationReader.Tests.Service;
using ConfigurationReader.Tests.Service.Interface;
using Microsoft.Extensions.Logging;
using Moq;

namespace ConfigurationReader.Tests.Tests.ParsersTests;

public class XmlConfigurationParserTests
{
    private readonly Mock<ILogger<XmlConfigurationParser>> _loggerMock;
    private readonly XmlConfigurationParser _parser;
    private readonly ITestService _testService = new TestService();

    public XmlConfigurationParserTests()
    {
        _loggerMock = new Mock<ILogger<XmlConfigurationParser>>();
        _parser = new XmlConfigurationParser(_loggerMock.Object);
    }

    [Fact]
    public void GetConfigurationRecord_CorrectXml_ReturnsConfiguration()
    {
        var testConfigFullPath = _testService.CreateConfigsForTestPath("CorrectXmlConfig.xml");

        var fileBytes = File.ReadAllBytes(testConfigFullPath);

        var result = _parser.Parse(fileBytes);

        Assert.NotNull(result);
        Assert.Equal("Конфигурация 1", result.Name);
        Assert.Equal("Описание Конфигурации 1", result.Description);
    }

    [Fact]
    public void GetConfigurationRecord_NotFulledXml_ThrowsException()
    {
        var testConfigFullPath = _testService.CreateConfigsForTestPath("NotFulledXmlConfig.xml");

        var fileBytes = File.ReadAllBytes(testConfigFullPath);

        var exception = Assert.Throws<ParserAlgorithmException>(() => _parser.Parse(fileBytes));

        Assert.Equal(string.Format(AllConsts.Errors.CreatedConfigurationIsNotFulled, nameof(XmlConfigurationParser)), exception.Message);
    }

    [Fact]
    public void GetConfigurationRecord_NullXml_ThrowsException()
    {
        var testConfigFullPath = _testService.CreateConfigsForTestPath("NullXmlConfig.xml");

        var fileBytes = File.ReadAllBytes(testConfigFullPath);

        var exception = Assert.Throws<ParserAlgorithmException>(() => _parser.Parse(fileBytes));

        Assert.Equal(string.Format(AllConsts.Errors.CreatedConfigurationIsNotFulled, nameof(XmlConfigurationParser)), exception.Message);
    }
}