using ConfigurationReader.Infrastructure.Enums;
using ConfigurationReader.Infrastructure.Exceptions;
using ConfigurationReader.Infrastructure.Factories.Interfaces;
using ConfigurationReader.Infrastructure.Parsers.Interfaces;
using ConfigurationReader.Infrastructure.Resources;
using ConfigurationReader.Tests.DI;
using ConfigurationReader.Tests.Services.Interface;

namespace ConfigurationReader.Tests.Tests.ParsersTests;

public class CsvConfigurationParserTests
{
    private readonly IConfigurationParser _parser;
    private readonly ITestService _testService = Resolver.Resolve<ITestService>();

    public CsvConfigurationParserTests()
    {
        var configurationParserFactory = Resolver.Resolve<IConfigurationParserFactory>();
        _parser = configurationParserFactory.CreateParser(ConfigurationFileType.Csv);
    }

    [Fact]
    public async Task GetConfigurationRecord_CorrectCsvConfig_ReturnsConfiguration()
    {
        var testConfigFullPath = _testService.GetConfigFullPath("CorrectCsvConfig.csv");

        var fileBytes = await File.ReadAllBytesAsync(testConfigFullPath);

        var result = await _parser.ParseAsync(fileBytes);

        Assert.NotNull(result);
        Assert.Equal("Конфигурация 2", result.Name);
        Assert.Equal("Описание Конфигурации 2", result.Description);
    }

    [Fact]
    public async Task GetConfigurationRecord_HalfFilledCsvConfig_ThrowsParserAlgorithmException()
    {
        var testConfigFullPath = _testService.GetConfigFullPath("HalfFilledCsvConfig.csv");

        var fileBytes = await File.ReadAllBytesAsync(testConfigFullPath);

        var exception =
            await Assert.ThrowsAsync<ParserAlgorithmException>(
                async () => await _parser.ParseAsync(fileBytes));

        Assert.Equal(string.Format(ErrorMessages.CreatedConfigurationIsNotFilled, _parser.GetType().Name),
            exception.Message);
    }

    [Fact]
    public async Task GetConfigurationRecord_NotFilledCsvConfig_ThrowsParserAlgorithmException()
    {
        var testConfigFullPath = _testService.GetConfigFullPath("NotFilledCsvConfig.csv");

        var fileBytes = await File.ReadAllBytesAsync(testConfigFullPath);

        var exception =
            await Assert.ThrowsAsync<ParserAlgorithmException>(
                async () => await _parser.ParseAsync(fileBytes));

        Assert.Equal(string.Format(ErrorMessages.CreatedConfigurationIsNull, _parser.GetType().Name),
            exception.Message);
    }
}