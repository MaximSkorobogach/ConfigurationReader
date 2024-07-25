using ConfigurationReader.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfigurationReader.Tests.Service.Interface;

namespace ConfigurationReader.Tests.Service
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

        public string CreateConfigsForTestPath(string configName)
        {
            string workingDirectory = Environment.CurrentDirectory;

            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;

            string configTestsDirectory = "ConfigsForTest\\";

            string testConfigFullPath = Path.Combine(projectDirectory, configTestsDirectory, configName);

            return testConfigFullPath;
        }
    }
}
