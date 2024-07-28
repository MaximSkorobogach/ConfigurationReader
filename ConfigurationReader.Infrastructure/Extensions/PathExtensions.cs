using System.ComponentModel;
using ConfigurationReader.Infrastructure.Consts;
using ConfigurationReader.Infrastructure.DTO;
using ConfigurationReader.Infrastructure.Enums;
using ConfigurationReader.Infrastructure.Exceptions;

namespace ConfigurationReader.Infrastructure.Extensions
{
    public static class PathExtensions
    {
        /// <summary>
        /// Указанный файл является этим типом конфигурационного файла
        /// </summary>
        /// <param name="fileDto">Экземпляр файла</param>
        /// <param name="configurationFileType">Тип конфигурационного файла</param>
        /// <exception cref="PathException">Ошибки доступа к пути</exception>
        public static bool IsFileOfConfigurationType(this FileDto fileDto,
            ConfigurationFileType configurationFileType)
        {
            if (string.IsNullOrEmpty(fileDto.FilePath))
                throw new PathException(string.Format(AllConsts.Errors.PathIsNullOrEmpty));

            if (!Path.Exists(fileDto.FilePath))
                throw new PathException(string.Format(AllConsts.Errors.PathNotExists, fileDto.FilePath));

            return String.Equals(Path.GetExtension(fileDto.FilePath), configurationFileType.GetDescription(),
                StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Получить тип конфигурационного файла по файлу из пути
        /// </summary>
        /// <param name="fileDto">Экземпляр файла</param>
        public static ConfigurationFileType? GetConfigurationFileTypeFromPath(this FileDto fileDto)
        {
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
    }
}
