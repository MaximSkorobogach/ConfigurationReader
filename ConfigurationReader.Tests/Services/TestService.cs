using ConfigurationReader.Infrastructure.DTO;
using ConfigurationReader.Tests.DI;
using ConfigurationReader.Tests.Services.Interface;
using ConfigurationReader.Tests.Settings;
using Microsoft.Extensions.Options;

namespace ConfigurationReader.Tests.Services;

public class TestService : ITestService
{
    private readonly IOptions<TestSettings> _testSettings = Resolver.Resolve<IOptions<TestSettings>>();

    public void SetupTestDirectory(string directoryPath, string[] filePaths)
    {
        Directory.CreateDirectory(directoryPath);
        foreach (var path in filePaths) File.Create(path).Dispose();
    }

    public void CleanupTestDirectory(string directoryPath)
    {
        Directory.Delete(directoryPath, true);
    }

    public void AssertFileDtos(List<FileDto> expectedFiles, List<FileDto> resultFiles)
    {
        Assert.Equal(expectedFiles.Count, resultFiles.Count);

        expectedFiles = [..expectedFiles.OrderBy(f => f.FileName)];
        resultFiles = [..resultFiles.OrderBy(f => f.FileName)];

        for (var i = 0; i < expectedFiles.Count; i++) AssertFileDto(expectedFiles[i], resultFiles[i]);
    }

    public void AssertFileDto(FileDto expectedFile, FileDto resultFile)
    {
        Assert.Equal(expectedFile, resultFile);
    }

    public string GetConfigFullPath(string configName)
    {
        var workingDirectory = Environment.CurrentDirectory;

        var projectDirectory =
            Directory.GetParent(workingDirectory)!.Parent!.Parent!.FullName;

        var testConfigFullPath =
            Path.Combine(projectDirectory, _testSettings.Value.TestConfigurationsFileDirectory, configName);

        return testConfigFullPath;
    }
}