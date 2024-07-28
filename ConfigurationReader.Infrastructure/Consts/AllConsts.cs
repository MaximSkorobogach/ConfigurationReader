using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationReader.Infrastructure.Consts
{
    public class AllConsts
    {
        public class Tests
        {
            public const string TestConfigurationsFileDirectory = "ConfigsForTest";
        }

        public class Errors
        {
            public const string PathIsNullOrEmpty = "Не передан путь";
            public const string PathNotExists = "Путь {0} недоступен";
            public const string ProcessingPathsHasErrors = "В одном из пути возникла ошибка, дальнейшая обработка прервана. {0}";
            public const string ParsingFileHasError = "При обработке файла по пути {0} возникла ошибка. {1}";
            public const string FileFormatNotAvailableForParsing = "Файл не поддерживается для парсинга";
            public const string CantCreateInstanceOfType = "Нет реализации для типа {0}";
            public const string CantFindParserForThisConfigurationFormat = "Не реализован парсер для {0}";
            public const string HasErrorInParsingAlgorithm = "Алгоритм парсинга {0} выдал ошибку. {1}";
            public const string CreatedConfigurationIsNull = "Конфигурация из парсера {0} создана пустой";
            public const string CreatedConfigurationIsNotFilled = "Конфигурация из парсера {0} не заполнена полностью";
            public const string ObjectIsNull = "Объект для проверки пуст";
            public const string CantFindAttribute = "Не найден атрибут {0} для значения {1}";
        }

        public class Tracing
        {
            public const string MethodStarted = "Запущен метод {0}, параметры: {1}";
            public const string MethodFinished = "Метод {0} завершен, время обработки: {1}";
            public const string ParsingStarted = "Парсинг через алгоритм {0} запущен";
            public const string ParsingFinished = "Парсинг через алгоритм {0} завершен, время обработки: {1}";
        }
    }
}
