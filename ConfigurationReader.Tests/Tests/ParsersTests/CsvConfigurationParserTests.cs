using System.Text;
using ConfigurationReader.Infrastructure.Consts;
using ConfigurationReader.Infrastructure.Exceptions;
using ConfigurationReader.Infrastructure.Parsers;
using ConfigurationReader.Tests.Service;
using ConfigurationReader.Tests.Service.Interface;
using Microsoft.Extensions.Logging;
using Moq;

namespace ConfigurationReader.Tests.Tests.ParsersTests;

public class CsvConfigurationParserTests
{
    private readonly Mock<ILogger<CsvConfigurationParser>> _loggerMock;
    private readonly CsvConfigurationParser _parser;
    private readonly ITestService _testService = new TestService();

    public CsvConfigurationParserTests()
    {
        _loggerMock = new Mock<ILogger<CsvConfigurationParser>>();
        _parser = new CsvConfigurationParser(_loggerMock.Object);
    }

    [Fact]
    public void GetConfigurationRecord_CorrectCsv_ReturnsConfiguration()
    {
        var testConfigFullPath = _testService.CreateConfigsForTestPath("CorrectCsvConfig.csv");

        var fileBytes = File.ReadAllBytes(testConfigFullPath);

        var result = _parser.Parse(fileBytes);

        Assert.NotNull(result);
        Assert.Equal("Конфигурация 2", result.Name);
        Assert.Equal("Описание Конфигурации 2", result.Description);
    }

    [Fact]
    public void GetConfigurationRecord_NotFulledCsv_ThrowsException()
    {
        var testConfigFullPath = _testService.CreateConfigsForTestPath("NotFulledCsvConfig.csv");

        var fileBytes = File.ReadAllBytes(testConfigFullPath);

        var exception = Assert.Throws<ParserAlgorithmException>(() => _parser.Parse(fileBytes));

        Assert.Equal(string.Format(AllConsts.Errors.CreatedConfigurationIsNotFulled, nameof(CsvConfigurationParser)), exception.Message);
    }

    [Fact]
    public void GetConfigurationRecord_NullCsv_ThrowsException()
    {
        var testConfigFullPath = _testService.CreateConfigsForTestPath("NullCsvConfig.csv");

        var fileBytes = File.ReadAllBytes(testConfigFullPath);

        var exception = Assert.Throws<ParserAlgorithmException>(() => _parser.Parse(fileBytes));

        Assert.Equal(string.Format(AllConsts.Errors.CreatedConfigurationIsNull, nameof(CsvConfigurationParser)), exception.Message);
    }
}