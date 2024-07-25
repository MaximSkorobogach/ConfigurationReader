using ConfigurationReader.Infrastructure.DTO;

namespace ConfigurationReader.Tests.Service.Interface;

public interface ITestService
{
    void SetupTestDirectory(string directoryPath, string[] filePaths);
    void CleanupTestDirectory(string directoryPath);
    void AssertFileDtos(List<FileDto> expectedFiles, List<FileDto> resultFiles);
    void AssertFileDto(FileDto expectedFile, FileDto resultFile);
    public string CreateConfigsForTestPath(string configName);
}