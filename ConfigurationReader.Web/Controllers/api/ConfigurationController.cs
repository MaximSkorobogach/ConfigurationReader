using System.Diagnostics;
using ConfigurationReader.Infrastructure.Consts;
using ConfigurationReader.Infrastructure.DTO;
using ConfigurationReader.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ConfigurationReader.Web.Controllers.api
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfigurationController : ControllerBase
    {
        private readonly IConfigurationService _configurationService;
        private readonly ILogger<ConfigurationController> _logger;

        public ConfigurationController(IConfigurationService configurationService, ILogger<ConfigurationController> logger)
        {
            _configurationService = configurationService;
            _logger = logger;
        }

        /// <summary>
        /// Получить все конфигурации с пути
        /// </summary>
        /// <param name="directoryPath">Путь до файлов</param>
        [HttpGet]
        [Route("GetConfigurationsFromDirectoryPath")]
        [ProducesResponseType(typeof(List<Configuration>), 200)]
        public IActionResult GetConfigurationsFromDirectoryPath(string directoryPath)
        {
            _logger.LogInformation(string.Format(AllConsts.Tracing.MethodStarted,
                nameof(GetConfigurationsFromDirectoryPath), directoryPath));
            var stopWatch = new Stopwatch();

            try
            {
                stopWatch.Start();
                var configurations = _configurationService.GetConfigurationsFromDirectoryPath(directoryPath);
                stopWatch.Stop();

                _logger.LogInformation(string.Format(AllConsts.Tracing.MethodFinished,
                    nameof(GetConfigurationsFromDirectoryPath), stopWatch.Elapsed));

                return Ok(configurations);
            }
            catch (Exception e)
            {
                var message = string.Format(AllConsts.Errors.ProcessingPathsHasErrors, e.Message);

                _logger.LogError(message);

                return BadRequest(message);
            }
        }

        /// <summary>
        /// Получить конфигурацию с путей файлов
        /// </summary>
        /// <param name = "filePaths" > Пути к файлам</param>
        [HttpGet]
        [Route("GetConfigurationFromFilesPaths")]
        [ProducesResponseType(typeof(List<Configuration>), 200)]
        public IActionResult GetConfigurationFromFilesPaths(string[] filesPaths)
        {
            _logger.LogInformation(string.Format(AllConsts.Tracing.MethodStarted,
                nameof(GetConfigurationFromFilesPaths), string.Join("; ", filesPaths)));
            var stopWatch = new Stopwatch();

            try
            {
                stopWatch.Start();
                var configurations = _configurationService.GetConfigurationFromFilesPaths(filesPaths);
                stopWatch.Stop();

                _logger.LogInformation(string.Format(AllConsts.Tracing.MethodFinished,
                    nameof(GetConfigurationsFromDirectoryPath), stopWatch.Elapsed));

                return Ok(configurations);
            }
            catch (Exception e)
            {
                var message = string.Format(AllConsts.Errors.ProcessingPathsHasErrors, e.Message);

                _logger.LogError(message);

                return BadRequest(message);
            }
        }

        /// <summary>
        /// Получить конфигурацию с пути файла
        /// </summary>
        /// <param name="filePath">Путь до файла</param>
        [HttpGet]
        [Route("GetConfigurationFromFilePath")]
        [ProducesResponseType(typeof(Configuration), 200)]
        public IActionResult GetConfigurationFromFilePath(string filePath)
        {
            _logger.LogInformation(string.Format(AllConsts.Tracing.MethodStarted,
                nameof(GetConfigurationFromFilesPaths), filePath));
            var stopWatch = new Stopwatch();

            try
            {
                stopWatch.Start();
                var configuration = _configurationService.GetConfigurationFromFilePath(filePath);
                stopWatch.Stop();

                _logger.LogInformation(string.Format(AllConsts.Tracing.MethodFinished,
                    nameof(GetConfigurationsFromDirectoryPath), stopWatch.Elapsed));

                return Ok(configuration);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e.Message);

                return BadRequest(e.Message);
            }
        }
    }
}
