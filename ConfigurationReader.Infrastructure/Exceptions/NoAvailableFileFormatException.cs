namespace ConfigurationReader.Infrastructure.Exceptions;

public class NoAvailableFileFormatException(string message) : Exception(message);