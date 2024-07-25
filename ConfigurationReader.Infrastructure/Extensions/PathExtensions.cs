using System.ComponentModel;
using ConfigurationReader.Infrastructure.Consts;
using ConfigurationReader.Infrastructure.Enums;
using ConfigurationReader.Infrastructure.Exceptions;

namespace ConfigurationReader.Infrastructure.Extensions
{
    public static class PathExtensions
    {
        /// <summary>
        /// Указанный файл является этим типом конфигурационного файла
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        /// <param name="configurationFileType">Тип конфигурационного файла</param>
        /// <exception cref="Exception">К указанному пути нет доступа</exception>
        public static bool IsExtensionForConfigurationFileType(this string filePath,
            ConfigurationFileType configurationFileType)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new PathException(string.Format(AllConsts.Errors.PathIsNullOrEmpty));

            if (!Path.Exists(filePath))
                throw new PathException(string.Format(AllConsts.Errors.PathNotExists, filePath));

            return String.Equals(Path.GetExtension(filePath), configurationFileType.GetDescription(),
                StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Получить тип конфигурационного файла по файлу из пути
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        public static ConfigurationFileType? GetConfigurationFileTypeFromPath(this string filePath)
        {
            var enumValues =
                Enum.GetValues(typeof(ConfigurationFileType))
                    .Cast<ConfigurationFileType>()
                    .ToList();

            var hasConfigurationFileType = 
                enumValues.Any(filePath.IsExtensionForConfigurationFileType);

            if (!hasConfigurationFileType)
                return null;

            return enumValues.First(filePath.IsExtensionForConfigurationFileType);
        }
    }
}
