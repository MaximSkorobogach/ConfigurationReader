using ConfigurationReader.Infrastructure.Consts;
using ConfigurationReader.Infrastructure.DTO;
using ConfigurationReader.Tests.Services.Interface;

namespace ConfigurationReader.Tests.Services
{ 
    public class TestService : ITestService
    {
        public void SetupTestDirectory(string directoryPath, string[] filePaths)
        {
            Directory.CreateDirectory(directoryPath);
            foreach (var path in filePaths)
            {
                File.Create(path).Dispose();
            }
        }

        public void CleanupTestDirectory(string directoryPath)
        {
            Directory.Delete(directoryPath, true);
        }

        public void AssertFileDtos(List<FileDto> expectedFiles, List<FileDto> resultFiles)
        {
            Assert.Equal(expectedFiles.Count, resultFiles.Count);

            expectedFiles = expectedFiles.OrderBy(f => f.FileName).ToList();
            resultFiles = resultFiles.OrderBy(f => f.FileName).ToList();

            for (int i = 0; i < expectedFiles.Count; i++)
            {
                AssertFileDto(expectedFiles[i], resultFiles[i]);
            }
        }

        public void AssertFileDto(FileDto expectedFile, FileDto resultFile)
        {
            Assert.Equal(expectedFile.FileName, resultFile.FileName);
            Assert.Equal(expectedFile.FileExtension, resultFile.FileExtension);
            Assert.Equal(expectedFile.FilePath, resultFile.FilePath);
        }

        public string GetConfigFullPath(string configName)
        {
            string workingDirectory = Environment.CurrentDirectory;

            string projectDirectory = 
                Directory.GetParent(workingDirectory).Parent.Parent.FullName;

            string testConfigFullPath = 
                Path.Combine(projectDirectory, AllConsts.Tests.TestConfigurationsFileDirectory, configName);

            return testConfigFullPath;
        }
    }
}
