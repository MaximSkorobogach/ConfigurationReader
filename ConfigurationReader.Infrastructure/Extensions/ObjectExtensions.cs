using ConfigurationReader.Infrastructure.Consts;

namespace ConfigurationReader.Infrastructure.Extensions;

public static class ObjectExtensions
{
    /// <summary>
    /// Все свойства объекта не null
    /// </summary>
    public static bool AllPropertiesIsNotNull(this object objectToCheck)
    {
        if (objectToCheck is null)
            throw new NullReferenceException(AllConsts.Errors.ObjectIsNull);

        return objectToCheck.GetType().GetProperties()
            .All(pi => pi.GetValue(objectToCheck) != null);
    }

    /// <summary>
    /// Все string свойства объекта не null, пустые, пробелы
    /// </summary>
    public static bool AllStringPropertiesIsNotEmpty(this object objectToCheck)
    {
        if (objectToCheck is null)
            throw new NullReferenceException(AllConsts.Errors.ObjectIsNull);

        return objectToCheck.GetType().GetProperties()
            .Where(x => x.PropertyType == typeof(string))
            .All(pi => !string.IsNullOrWhiteSpace((string)pi.GetValue(objectToCheck)));
    }
}