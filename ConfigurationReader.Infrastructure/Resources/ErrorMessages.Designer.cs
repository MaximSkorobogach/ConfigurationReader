﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ConfigurationReader.Infrastructure.Resources {
    using System;
    
    
    /// <summary>
    ///   Класс ресурса со строгой типизацией для поиска локализованных строк и т.д.
    /// </summary>
    // Этот класс создан автоматически классом StronglyTypedResourceBuilder
    // с помощью такого средства, как ResGen или Visual Studio.
    // Чтобы добавить или удалить член, измените файл .ResX и снова запустите ResGen
    // с параметром /str или перестройте свой проект VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ErrorMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ErrorMessages() {
        }
        
        /// <summary>
        ///   Возвращает кэшированный экземпляр ResourceManager, использованный этим классом.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ConfigurationReader.Infrastructure.Resources.ErrorMessages", typeof(ErrorMessages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Перезаписывает свойство CurrentUICulture текущего потока для всех
        ///   обращений к ресурсу с помощью этого класса ресурса со строгой типизацией.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на При обработке массива файлов возникли следующие ошибки:.
        /// </summary>
        public static string ArrayFilesHaveErrors {
            get {
                return ResourceManager.GetString("ArrayFilesHaveErrors", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Нет реализации для типа {0}.
        /// </summary>
        public static string CantCreateInstanceOfType {
            get {
                return ResourceManager.GetString("CantCreateInstanceOfType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Не найден атрибут {0} для значения {1}.
        /// </summary>
        public static string CantFindAttribute {
            get {
                return ResourceManager.GetString("CantFindAttribute", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Не реализован парсер для {0}.
        /// </summary>
        public static string CantFindParserForThisConfigurationFormat {
            get {
                return ResourceManager.GetString("CantFindParserForThisConfigurationFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Конфигурация из парсера {0} не заполнена полностью.
        /// </summary>
        public static string CreatedConfigurationIsNotFilled {
            get {
                return ResourceManager.GetString("CreatedConfigurationIsNotFilled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Конфигурация из парсера {0} создана пустой.
        /// </summary>
        public static string CreatedConfigurationIsNull {
            get {
                return ResourceManager.GetString("CreatedConfigurationIsNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на В ДТО файла не передан формат файла.
        /// </summary>
        public static string ExtensionInFileDtoIsNullOrEmpty {
            get {
                return ResourceManager.GetString("ExtensionInFileDtoIsNullOrEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Файл не поддерживается для парсинга.
        /// </summary>
        public static string FileFormatNotAvailableForParsing {
            get {
                return ResourceManager.GetString("FileFormatNotAvailableForParsing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Алгоритм парсинга {0} выдал ошибку. {1}.
        /// </summary>
        public static string HasErrorInParsingAlgorithm {
            get {
                return ResourceManager.GetString("HasErrorInParsingAlgorithm", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на При обработке файла по пути {0} возникла ошибка. {1}.
        /// </summary>
        public static string ParsingFileHasError {
            get {
                return ResourceManager.GetString("ParsingFileHasError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Не передан путь.
        /// </summary>
        public static string PathIsNullOrEmpty {
            get {
                return ResourceManager.GetString("PathIsNullOrEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Путь {0} недоступен.
        /// </summary>
        public static string PathNotExists {
            get {
                return ResourceManager.GetString("PathNotExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на В одном из пути возникла ошибка, дальнейшая обработка прервана. {0}.
        /// </summary>
        public static string ProcessingPathsHasErrors {
            get {
                return ResourceManager.GetString("ProcessingPathsHasErrors", resourceCulture);
            }
        }
    }
}
