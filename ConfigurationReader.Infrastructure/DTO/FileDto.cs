namespace ConfigurationReader.Infrastructure.DTO;

public class FileDto
{
    public FileDto(string fileName, string fileExtension, string filePath)
    {
        FileName = fileName;
        FileExtension = fileExtension;
        FilePath = filePath;
    }

    public string FileName { get; }
    public string FileExtension { get; }
    public string FilePath { get; }
}