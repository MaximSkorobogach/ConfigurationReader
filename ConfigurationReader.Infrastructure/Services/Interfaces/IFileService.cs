using ConfigurationReader.Infrastructure.DTO;
using ConfigurationReader.Infrastructure.Enums;

namespace ConfigurationReader.Infrastructure.Services.Interfaces;

public interface IFileService
{
    List<FileDto> GetFilesFromDirectoryPath(string directoryPath);
    List<FileDto> GetFilesFromFilesPaths(string[] filePaths);
    FileDto GetFileFromFilePath(string filePath);
}