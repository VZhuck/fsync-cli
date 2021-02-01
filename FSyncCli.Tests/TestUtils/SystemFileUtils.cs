using System;
using System.Collections.Generic;
using System.IO;

namespace FSyncCli.Tests.TestUtils
{
    public static class SystemFileUtils
    {
        public static DirectoryInfo CreateTmpTestDir(string rootDirPath, string dirPrefix = default)
        {
            var tmpDirName = $"{dirPrefix ?? string.Empty}{Guid.NewGuid():D}";
            var tmpDirPath = Path.Combine(rootDirPath, tmpDirName);

            return Directory.CreateDirectory(tmpDirPath);
        }


        public static FileInfo CreateTestFile(this DirectoryInfo directoryInfo, string fileContent = default)
        {
            var testFilePath = Path.Combine(
                directoryInfo.FullName,
                Guid.NewGuid().ToString("D"));

            using (var txtWriter = File.CreateText(testFilePath))
            {
                txtWriter.Write(
                    string.IsNullOrWhiteSpace(fileContent)
                        ? $"Test File Content: {Guid.NewGuid()}"
                        : fileContent);
            }

            return new FileInfo(testFilePath);
        }

        public static IEnumerable<FileInfo> CreateMultipleTestFiles(this DirectoryInfo directoryInfo, int count)
        {
            var files = new List<FileInfo>(count);

            for (int i = 0; i < count; i++)
            {
                files.Add(directoryInfo.CreateTestFile());
            }

            return files;
        }

        public static void DeleteTmpTestDirWithAllFiles(this DirectoryInfo directoryInfo)
        {
            if (directoryInfo!=null && directoryInfo.Exists)
            {
                directoryInfo.Delete(true);
            }
        }
    }
}