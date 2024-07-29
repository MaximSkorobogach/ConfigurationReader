namespace ConfigurationReader.Infrastructure.Extensions;

public static class ObjectExtensions
{
    /// <summary>
    /// Все свойства объекта не null
    /// </summary>
    public static bool AllPropertiesIsNotNull(this object objectToCheck)
    {
        ArgumentNullException.ThrowIfNull(objectToCheck);

        return objectToCheck.GetType().GetProperties()
            .All(pi => pi.GetValue(objectToCheck) != null);
    }

    /// <summary>
    /// Все <see cref="string"/> свойства объекта заполнены
    /// </summary>
    public static bool AllStringPropertiesIsNotEmpty(this object objectToCheck)
    {
        ArgumentNullException.ThrowIfNull(objectToCheck);

        return objectToCheck.GetType().GetProperties()
            .Where(x => x.PropertyType == typeof(string))
            .All(pi => !string.IsNullOrWhiteSpace((string)pi.GetValue(objectToCheck)));
    }
}