using System.ComponentModel;
using ConfigurationReader.Infrastructure.Consts;

namespace ConfigurationReader.Infrastructure.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Получить название объекта перечисления
        /// </summary>
        public static string? GetName<TEnum>(this TEnum enumValue) where TEnum : Enum
        {
            return Enum.GetName(typeof(TEnum), enumValue);
        }

        /// <summary>
        /// Получить аттрибут объекта перечисления
        /// </summary>
        private static IEnumerable<TAttribute> GetAttributes<TAttribute, TEnum>(this TEnum value)
            where TAttribute : Attribute
            where TEnum : Enum
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var attributes = (IEnumerable<TAttribute>)fieldInfo.GetCustomAttributes(typeof(TAttribute), false);
           
            return attributes;
        }

        /// <summary>
        /// Получить описание объекта перечисления из аттрибута <see cref="DescriptionAttribute"/>
        /// </summary>
        public static string GetDescription<TEnum>(this TEnum value)
            where TEnum : struct, Enum
        {
            var attributes = value.GetAttributes<DescriptionAttribute, TEnum>().ToArray();

            var description = attributes.FirstOrDefault();

            if (description is null)
                throw new NotSupportedException(string.Format(AllConsts.Errors.CantFindAttribute,
                    nameof(DescriptionAttribute), value));
                                                                                                                         
            return description.Description;
        }
    }
}
