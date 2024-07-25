using ConfigurationReader.Infrastructure.Consts;
using ConfigurationReader.Infrastructure.DTO;
using ConfigurationReader.Infrastructure.Exceptions;
using ConfigurationReader.Infrastructure.Services;
using ConfigurationReader.Infrastructure.Services.Interfaces;
using ConfigurationReader.Tests.Service;
using ConfigurationReader.Tests.Service.Interface;

namespace ConfigurationReader.Tests.Tests.ServicesTests;

public class FileServiceTests
{
    private readonly IFileService _fileService = new FileService();
    private readonly ITestService _testService = new TestService();

    #region GetAllFilesFromDirectoryPath

    [Fact]
    public void GetAllFilesFromDirectoryPath_ShouldReturnFiles_WhenDirectoryExists()
    {
        string directoryPath = "TestDirectory";
        string[] filePaths = { "TestDirectory\\file1.txt", "TestDirectory\\file2.txt" };
        var expectedFiles = new List<FileDto>
        {
            new FileDto("file1.txt", ".txt", "TestDirectory\\file1.txt"),
            new FileDto("file2.txt", ".txt", "TestDirectory\\file2.txt")
        };

        _testService.SetupTestDirectory(directoryPath, filePaths);

        var result = _fileService.GetAllFilesFromDirectoryPath(directoryPath);

        _testService.AssertFileDtos(expectedFiles, result);

        _testService.CleanupTestDirectory(directoryPath);
    }

    [Fact]
    public void GetAllFilesFromDirectoryPath_ShouldThrowException_WhenDirectoryDoesNotExist()
    {
        string directoryPath = "NonExistingDirectory";

        var exception = Assert.Throws<PathException>(() => _fileService.GetAllFilesFromDirectoryPath(directoryPath));
        Assert.Equal(string.Format(AllConsts.Errors.PathNotExists, directoryPath), exception.Message);
    }

    [Fact]
    public void GetAllFilesFromDirectoryPath_ShouldThrowException_WhenPathIsNull()
    {
        string directoryPath = null;

        var exception = Assert.Throws<PathException>(() => _fileService.GetAllFilesFromDirectoryPath(directoryPath));
        Assert.Equal(AllConsts.Errors.PathIsNullOrEmpty, exception.Message);
    }

    [Fact]
    public void GetAllFilesFromDirectoryPath_ShouldThrowException_WhenPathIsEmpty()
    {
        string directoryPath = string.Empty;

        var exception = Assert.Throws<PathException>(() => _fileService.GetAllFilesFromDirectoryPath(directoryPath));
        Assert.Equal(AllConsts.Errors.PathIsNullOrEmpty, exception.Message);
    }
    #endregion

    #region GetFilesFromFilesPaths

    [Fact]
    public void GetFilesFromFilesPaths_ShouldReturnFiles_WhenDirectoryExists()
    {
        string directoryPath = "TestDirectory";
        string[] filesPaths = { "TestDirectory\\file1.txt", "TestDirectory\\file2.txt" };
        var expectedFiles = new List<FileDto>
        {
            new FileDto("file1.txt", ".txt", "TestDirectory\\file1.txt"),
            new FileDto("file2.txt", ".txt", "TestDirectory\\file2.txt")
        };

        _testService.SetupTestDirectory(directoryPath, filesPaths);

        var result = _fileService.GetFilesFromFilesPaths(filesPaths);

        _testService.AssertFileDtos(expectedFiles, result);

        _testService.CleanupTestDirectory(directoryPath);
    }

    [Fact]
    public void GetFilesFromFilesPaths_ShouldThrowException_WhenAnyPathNotExists()
    {
        string[] filePaths = { "NonExistingDirectory\\file1.txt", "NonExistingDirectory\\file2.txt" };

        var exception = Assert.Throws<PathException>(() => _fileService.GetFilesFromFilesPaths(filePaths));
        Assert.Equal(string.Format(AllConsts.Errors.PathNotExists, filePaths.First()), exception.Message);
    }

    [Fact]
    public void GetFilesFromFilesPaths_ShouldThrowException_WhenAnyNullPath()
    {
        string[] filesPaths = { null, "NonExistingDirectory\\file2.txt" };

        var exception = Assert.Throws<PathException>(() => _fileService.GetFilesFromFilesPaths(filesPaths));
        Assert.Equal(AllConsts.Errors.PathIsNullOrEmpty, exception.Message);
    }

    [Fact]
    public void GetFilesFromFilesPaths_ShouldThrowException_WhenPathIsEmpty()
    {
        string[] filesPaths = { "", "NonExistingDirectory\\file2.txt" };

        var exception = Assert.Throws<PathException>(() => _fileService.GetFilesFromFilesPaths(filesPaths));
        Assert.Equal(AllConsts.Errors.PathIsNullOrEmpty, exception.Message);
    }

    #endregion

    #region GetFileFromFilePath

    [Fact]
    public void GetFileFromFilePath_ShouldReturnFiles_WhenDirectoryExists()
    {
        string directoryPath = "TestDirectory";
        string filePath = "TestDirectory\\file1.txt";
        var expectedFile = new FileDto("file1.txt", ".txt", "TestDirectory\\file1.txt");

        _testService.SetupTestDirectory(directoryPath, new[] { filePath });

        var result = _fileService.GetFileFromFilePath(filePath);

        _testService.AssertFileDto(expectedFile, result);

        _testService.CleanupTestDirectory(directoryPath);
    }

    [Fact]
    public void GetFileFromFilePath_ShouldThrowException_WhenAnyPathNotExists()
    {
        string filePath = "NonExistingDirectory\\file1.txt";

        var exception = Assert.Throws<PathException>(() => _fileService.GetFileFromFilePath(filePath));
        Assert.Equal(string.Format(AllConsts.Errors.PathNotExists, filePath), exception.Message);
    }

    [Fact]
    public void GetFileFromFilePath_ShouldThrowException_WhenAnyNullPath()
    {
        string filePath = null;

        var exception = Assert.Throws<PathException>(() => _fileService.GetFileFromFilePath(filePath));
        Assert.Equal(AllConsts.Errors.PathIsNullOrEmpty, exception.Message);
    }

    [Fact]
    public void GetFileFromFilePath_ShouldThrowException_WhenPathIsEmpty()
    {
        string filePath = "";

        var exception = Assert.Throws<PathException>(() => _fileService.GetFileFromFilePath(filePath));
        Assert.Equal(AllConsts.Errors.PathIsNullOrEmpty, exception.Message);
    }

    #endregion
}