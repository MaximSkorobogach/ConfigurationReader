using ConfigurationReader.Infrastructure.Consts;
using ConfigurationReader.Infrastructure.DTO;
using ConfigurationReader.Infrastructure.Exceptions;
using ConfigurationReader.Infrastructure.Extensions;
using ConfigurationReader.Infrastructure.Services;
using ConfigurationReader.Infrastructure.Services.Interfaces;
using ConfigurationReader.Tests.Services;
using ConfigurationReader.Tests.Services.Interface;

namespace ConfigurationReader.Tests.Tests.ServicesTests;

public class FileServiceTests
{
    private readonly IFileService _fileService = new FileService();
    private readonly ITestService _testService = new TestService();

    #region GetFilesFromDirectoryPath

    [Fact]
    public void GetAllFilesFromDirectoryPath_ExistDirectoryPath_ReturnsFiles()
    {
        string directoryPath = "TestDirectory";
        string[] filePaths =
        {
            "TestDirectory\\file1.txt".GetPlatformSpecificPath(), 
            "TestDirectory\\file2.txt".GetPlatformSpecificPath()
        };
        var expectedFiles = new List<FileDto>
        {
            new FileDto(Path.GetFileName(filePaths[0]), Path.GetExtension(filePaths[0]), filePaths[0]),
            new FileDto(Path.GetFileName(filePaths[1]), Path.GetExtension(filePaths[1]), filePaths[1])
        };

        _testService.SetupTestDirectory(directoryPath, filePaths);

        var result = _fileService.GetFilesFromDirectoryPath(directoryPath);

        _testService.AssertFileDtos(expectedFiles, result);

        _testService.CleanupTestDirectory(directoryPath);
    }

    [Fact]
    public void GetAllFilesFromDirectoryPath_NotExistDirectoryPath_ThrowsPathException()
    {
        string directoryPath = "NonExistingDirectory".GetPlatformSpecificPath();

        var exception = Assert.Throws<PathException>(() => _fileService.GetFilesFromDirectoryPath(directoryPath));
        Assert.Equal(string.Format(AllConsts.Errors.PathNotExists, directoryPath), exception.Message);
    }

    [Fact]
    public void GetAllFilesFromDirectoryPath_NullDirectoryPath_ThrowsPathException()
    {
        string directoryPath = null;

        var exception = Assert.Throws<PathException>(() => _fileService.GetFilesFromDirectoryPath(directoryPath));
        Assert.Equal(AllConsts.Errors.PathIsNullOrEmpty, exception.Message);
    }

    [Fact]
    public void GetAllFilesFromDirectoryPath_EmptyDirectoryPath_ThrowsPathException()
    {
        string directoryPath = string.Empty;

        var exception = Assert.Throws<PathException>(() => _fileService.GetFilesFromDirectoryPath(directoryPath));
        Assert.Equal(AllConsts.Errors.PathIsNullOrEmpty, exception.Message);
    }
    #endregion

    #region GetFilesFromFilesPaths

    [Fact]
    public void GetFilesFromFilesPaths_ExistsFilesPaths_ReturnsFiles()
    {
        string directoryPath = "TestDirectory";
        string[] filesPaths =
        {
            "TestDirectory\\file1.txt".GetPlatformSpecificPath(), 
            "TestDirectory\\file2.txt".GetPlatformSpecificPath()
        };
        var expectedFiles = new List<FileDto>
        {
            new FileDto(Path.GetFileName(filesPaths[0]), Path.GetExtension(filesPaths[0]), filesPaths[0]),
            new FileDto(Path.GetFileName(filesPaths[1]), Path.GetExtension(filesPaths[1]), filesPaths[1])
        };

        _testService.SetupTestDirectory(directoryPath, filesPaths);

        var result = _fileService.GetFilesFromFilesPaths(filesPaths);

        _testService.AssertFileDtos(expectedFiles, result);

        _testService.CleanupTestDirectory(directoryPath);
    }

    [Fact]
    public void GetFilesFromFilesPaths_NotExistsAnyPath_ThrowsPathException()
    {
        string[] filePaths =
        {
            "NonExistingDirectory\\file1.txt".GetPlatformSpecificPath(),
            "NonExistingDirectory\\file2.txt".GetPlatformSpecificPath()
        };

        var exception = Assert.Throws<PathException>(() => _fileService.GetFilesFromFilesPaths(filePaths));
        Assert.Equal(string.Format(AllConsts.Errors.PathNotExists, filePaths.First()), exception.Message);
    }

    [Fact]
    public void GetFilesFromFilesPaths_NullAnyPaths_ThrowsPathExceptions()
    {
        string[] filesPaths = { null, "NonExistingDirectory\\file2.txt".GetPlatformSpecificPath() };

        var exception = Assert.Throws<PathException>(() => _fileService.GetFilesFromFilesPaths(filesPaths));
        Assert.Equal(AllConsts.Errors.PathIsNullOrEmpty, exception.Message);
    }

    [Fact]
    public void GetFilesFromFilesPaths_EmptyAnyPaths_ThrowsPathException()
    {
        string[] filesPaths = { "", "NonExistingDirectory\\file2.txt".GetPlatformSpecificPath() };

        var exception = Assert.Throws<PathException>(() => _fileService.GetFilesFromFilesPaths(filesPaths));
        Assert.Equal(AllConsts.Errors.PathIsNullOrEmpty, exception.Message);
    }

    #endregion

    #region GetFileFromFilePath

    [Fact]
    public void GetFileFromFilePath_ExistFilePath_ReturnsFile()
    {
        string directoryPath = "TestDirectory";
        string filePath = "TestDirectory\\file1.txt".GetPlatformSpecificPath();
        var expectedFile = 
            new FileDto(Path.GetFileName(filePath), Path.GetExtension(filePath), filePath);

        _testService.SetupTestDirectory(directoryPath, new[] { filePath });

        var result = _fileService.GetFileFromFilePath(filePath);

        _testService.AssertFileDto(expectedFile, result);

        _testService.CleanupTestDirectory(directoryPath);
    }

    [Fact]
    public void GetFileFromFilePath_NotExistFilePath_ThrowsPathException()
    {
        string filePath = "NonExistingDirectory\\file1.txt".GetPlatformSpecificPath();

        var exception = Assert.Throws<PathException>(() => _fileService.GetFileFromFilePath(filePath));
        Assert.Equal(string.Format(AllConsts.Errors.PathNotExists, filePath), exception.Message);
    }

    [Fact]
    public void GetFileFromFilePath_NullFilePath_ThrowsPathException()
    {
        string filePath = null;

        var exception = Assert.Throws<PathException>(() => _fileService.GetFileFromFilePath(filePath));
        Assert.Equal(AllConsts.Errors.PathIsNullOrEmpty, exception.Message);
    }

    [Fact]
    public void GetFileFromFilePath_EmptyFilePath_ThrowsPathException()
    {
        string filePath = "";

        var exception = Assert.Throws<PathException>(() => _fileService.GetFileFromFilePath(filePath));
        Assert.Equal(AllConsts.Errors.PathIsNullOrEmpty, exception.Message);
    }

    #endregion
}