using ConfigurationReader.Infrastructure.DTO;
using ConfigurationReader.Infrastructure.Services.Interfaces;
using ConfigurationReader.Infrastructure.Exceptions;
using ConfigurationReader.Infrastructure.Resources;

namespace ConfigurationReader.Infrastructure.Services;

public class FileService : IFileService
{
    public List<FileDto> GetFilesFromDirectoryPath(string directoryPath)
    {
        ThrowIfPathNotExisting(directoryPath);

        var filesPaths = Directory.GetFiles(directoryPath);

        var files = GetFilesFromFilesPaths(filesPaths);

        return files;
    }

    public List<FileDto> GetFilesFromFilesPaths(string[] filesPaths)
    {
        var files =
            filesPaths
                .Select(GetFileFromFilePath)
                .ToList();

        return files;
    }

    public FileDto GetFileFromFilePath(string filePath)
    {
        ThrowIfPathNotExisting(filePath);

        return CreateFileDtoFromFilePath(filePath);
    }

    private void ThrowIfPathNotExisting(string path)
    {
        if (string.IsNullOrEmpty(path))
            throw new PathException(ErrorMessages.PathIsNullOrEmpty);

        if (!Path.Exists(path))
            throw new PathException(string.Format(ErrorMessages.PathNotExists, path));
    }

    private FileDto CreateFileDtoFromFilePath(string filePath)
    {
        return new FileDto(Path.GetFileName(filePath), Path.GetExtension(filePath), filePath);
    }
}