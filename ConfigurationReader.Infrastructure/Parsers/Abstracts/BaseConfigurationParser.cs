using ConfigurationReader.Infrastructure.Consts;
using ConfigurationReader.Infrastructure.DTO;
using ConfigurationReader.Infrastructure.Exceptions;
using ConfigurationReader.Infrastructure.Extensions;
using ConfigurationReader.Infrastructure.Parsers.Interfaces;
using Microsoft.Extensions.Logging;

namespace ConfigurationReader.Infrastructure.Parsers.Abstracts;

public abstract class BaseConfigurationParser : IConfigurationParser
{
    private readonly ILogger<BaseConfigurationParser> _logger;

    public BaseConfigurationParser(ILogger<BaseConfigurationParser> logger)
    {
        _logger = logger;
    }

    public virtual Configuration Parse(byte[] fileBytes)
    {
        Configuration configuration;
        _logger.LogTrace(string.Format(AllConsts.Tracing.ParsingStarted, GetType().Name));

        try
        {
            configuration = GetConfigurationRecord(fileBytes);
        }
        catch (Exception e)
        {
            throw new ParserAlgorithmException(
                string.Format(AllConsts.Errors.HasErrorInParsingAlgorithm, GetType().Name, e.Message));
        }

        ValidateConfiguration(configuration);

        _logger.LogTrace(string.Format(AllConsts.Tracing.ParsingFinished, GetType().Name));

        return configuration;
    }

    protected virtual void ValidateConfiguration(Configuration configuration)
    {
        if (configuration is null)
            throw new ParserAlgorithmException(string.Format(AllConsts.Errors.CreatedConfigurationIsNull, GetType().Name));

        if (!configuration.AllStringPropertiesIsNotEmpty() || !configuration.AllPropertiesIsNotNull())
            throw new ParserAlgorithmException(string.Format(AllConsts.Errors.CreatedConfigurationIsNotFilled, GetType().Name));
    } 

    protected abstract Configuration? GetConfigurationRecord(byte[] fileBytes);
}