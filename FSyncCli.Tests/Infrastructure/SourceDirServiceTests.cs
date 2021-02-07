using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FSyncCli.Infrastructure;
using FSyncCli.Tests.TestUtils;
using FSyncCli.Utils;
using Xunit;

namespace FSyncCli.Tests.Infrastructure
{
    public class SourceDirServiceTests : IDisposable
    {
        private readonly DirectoryInfo _sourceDirInfo;
        private readonly DirectoryInfo _inputDir1NestedDir;

        public SourceDirServiceTests()
        {
            var tmpTestRootDirPath = Path.GetTempPath();
            _sourceDirInfo = SystemFileUtils.CreateTmpTestDir(tmpTestRootDirPath, "fSyncIn_");
            _inputDir1NestedDir = SystemFileUtils.CreateTmpTestDir(_sourceDirInfo.FullName, "fSyncIn_");
        }

        [Fact]
        public void GetSourcesFiles_EnumerateAllFiles_ReturnValidCount()
        {
            //arrange
            var expected = new List<FileInfo>();
            expected.AddRange(_sourceDirInfo.CreateMultipleTestFiles(3));

            var sut = new SourceDirService();
            
            //act
            var result = sut.GetSourcesFiles(_sourceDirInfo).ToArray();

            //assert
            Assert.Equal(expected.Count, result.Length);
        }

        [Fact]
        public void GetSourcesFiles_EnumerateNestedFolders_ReturnValidCount()
        {
            //arrange
            var expected = new List<FileInfo>();
            expected.AddRange(_sourceDirInfo.CreateMultipleTestFiles(3));
            expected.AddRange(_inputDir1NestedDir.CreateMultipleTestFiles(2));

            var sut = new SourceDirService();

            //act
            var result = sut.GetSourcesFiles(_sourceDirInfo).ToArray();

            //assert
            Assert.Equal(expected.Count, result.Length);
        }

        public void Dispose()
        {
            _inputDir1NestedDir.DeleteTmpTestDirWithAllFiles();
            _sourceDirInfo.DeleteTmpTestDirWithAllFiles();
        }
    }
}