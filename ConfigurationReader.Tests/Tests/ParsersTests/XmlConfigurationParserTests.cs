using ConfigurationReader.Infrastructure.Enums;
using ConfigurationReader.Infrastructure.Exceptions;
using ConfigurationReader.Infrastructure.Factories.Interfaces;
using ConfigurationReader.Infrastructure.Parsers.Interfaces;
using ConfigurationReader.Infrastructure.Resources;
using ConfigurationReader.Tests.DI;
using ConfigurationReader.Tests.Services.Interface;

namespace ConfigurationReader.Tests.Tests.ParsersTests;

public class XmlConfigurationParserTests
{
    private readonly IConfigurationParser _parser;
    private readonly ITestService _testService = Resolver.Resolve<ITestService>();

    public XmlConfigurationParserTests()
    {
        var configurationParserFactory = Resolver.Resolve<IConfigurationParserFactory>();
        _parser = configurationParserFactory.CreateParser(ConfigurationFileType.Xml);
    }

    [Fact]
    public async Task GetConfigurationRecord_CorrectXmlConfig_ReturnsConfiguration()
    {
        var testConfigFullPath = _testService.GetConfigFullPath("CorrectXmlConfig.xml");

        var fileBytes = await File.ReadAllBytesAsync(testConfigFullPath);

        var result = await _parser.ParseAsync(fileBytes);

        Assert.NotNull(result);
        Assert.Equal("Конфигурация 1", result.Name);
        Assert.Equal("Описание Конфигурации 1", result.Description);
    }

    [Fact]
    public async Task GetConfigurationRecord_HalfFilledXmlConfig_ThrowsParserAlgorithmException()
    {
        var testConfigFullPath = _testService.GetConfigFullPath("HalfFilledXmlConfig.xml");

        var fileBytes = await File.ReadAllBytesAsync(testConfigFullPath);

        var exception =
            await Assert.ThrowsAsync<ParserAlgorithmException>(
                async () => await _parser.ParseAsync(fileBytes));

        Assert.Equal(string.Format(ErrorMessages.CreatedConfigurationIsNotFilled, _parser.GetType().Name),
            exception.Message);
    }

    [Fact]
    public async Task GetConfigurationRecord_NotFilledXmlConfig_ThrowsParserAlgorithmException()
    {
        var testConfigFullPath = _testService.GetConfigFullPath("NotFilledXmlConfig.xml");

        var fileBytes = await File.ReadAllBytesAsync(testConfigFullPath);

        var exception =
            await Assert.ThrowsAsync<ParserAlgorithmException>(
                async () => await _parser.ParseAsync(fileBytes));

        Assert.Equal(string.Format(ErrorMessages.CreatedConfigurationIsNotFilled, _parser.GetType().Name),
            exception.Message);
    }
}