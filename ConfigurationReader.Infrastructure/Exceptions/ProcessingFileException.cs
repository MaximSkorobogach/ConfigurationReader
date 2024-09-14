namespace ConfigurationReader.Infrastructure.Exceptions;

public class ProcessingFileException : Exception
{
    public ProcessingFileException(string message) : base(message)
    {
    }

    public ProcessingFileException(string message, Exception innerException) : base(message, innerException)
    {
    }
}