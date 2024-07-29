using ConfigurationReader.Infrastructure.DTO;
using ConfigurationReader.Infrastructure.Enums;
using ConfigurationReader.Infrastructure.Exceptions;
using ConfigurationReader.Infrastructure.Resources;

namespace ConfigurationReader.Infrastructure.Extensions
{
    public static class PathExtensions
    {
        /// <summary>
        /// Указанный файл является этим типом конфигурационного файла
        /// </summary>
        /// <param name="fileDto">Экземпляр файла</param>
        /// <param name="configurationFileType">Тип конфигурационного файла</param>
        /// <exception cref="ArgumentNullException">Не передано ДТО</exception>
        /// <exception cref="PathException">В ДТО отсутствует заполненное поле "формат файла"</exception>
        public static bool IsFileOfConfigurationType(this FileDto fileDto,
            ConfigurationFileType configurationFileType)
        {
            ArgumentNullException.ThrowIfNull(fileDto);

            if (string.IsNullOrWhiteSpace(fileDto.FileExtension))
                throw new PathException(ErrorMessages.ExtensionInFileDtoIsNullOrEmpty);

            return String.Equals(fileDto.FileExtension, configurationFileType.GetDescription(),
                StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Получить тип конфигурационного файла по файлу из пути
        /// </summary>
        /// <param name="fileDto">Экземпляр файла</param>
        /// <exception cref="ArgumentNullException">Не передано ДТО</exception>
        public static ConfigurationFileType? GetConfigurationFileType(this FileDto fileDto)
        {
            ArgumentNullException.ThrowIfNull(fileDto);

            var enumValues =
                Enum.GetValues(typeof(ConfigurationFileType))
                    .Cast<ConfigurationFileType>()
                    .ToList();

            var hasConfigurationFileType = 
                enumValues.Any(fileDto.IsFileOfConfigurationType);

            if (!hasConfigurationFileType)
                return null;

            return enumValues.First(fileDto.IsFileOfConfigurationType);
        }

        /// <summary>
        /// Получить путь с учетом формата пути спецификации системы
        /// </summary>
        public static string GetPlatformSpecificPath(this string relativePath)
        {
            return relativePath
                .Replace("/", Path.DirectorySeparatorChar.ToString())
                .Replace("\\", Path.DirectorySeparatorChar.ToString());
        }
    }
}
