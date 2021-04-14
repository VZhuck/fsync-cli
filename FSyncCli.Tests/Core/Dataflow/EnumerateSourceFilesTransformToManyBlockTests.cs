using System.IO;
using System.Threading.Tasks.Dataflow;
using FSyncCli.Core.Dataflow;
using FSyncCli.Domain;
using FSyncCli.Infrastructure;
using Moq;
using Xunit;

namespace FSyncCli.Tests.Core.Dataflow
{
    public class EnumerateSourceFilesTransformToManyBlockTests
    {
        private readonly Mock<IFileRepoService> _sourceDirServiceMock;

        public EnumerateSourceFilesTransformToManyBlockTests()
        {
            _sourceDirServiceMock = new Mock<IFileRepoService>();
        }

        [Fact]
        public void EnumerateSourceFilesBlock_TransformDirIntoFiles_Successfully()
        {
            //arrange
            var files = new[] { new FileMetadataInfo("Path1"), new FileMetadataInfo("Path2") };
            
            _sourceDirServiceMock
                .Setup(sourceSrv => sourceSrv.GetFileMetadataInfos(
                    It.Is<DirectoryInfo>(dir => dir.Name == "TestDir_01")))
                .Returns(files);

            var sut = new EnumerateSourceFilesTransformToManyBlock(_sourceDirServiceMock.Object);

            //act
            sut
                .Block
                .Post(new DirectoryInfo("TestDir_01"));
            sut.Block.Complete();

            var firstFile = sut.Block.Receive();
            var secondFile = sut.Block.Receive();

            //assert
            Assert.Equal("Path1", firstFile.FileMetadataInfo.FullPath);
            Assert.Equal("Path2", secondFile.FileMetadataInfo.FullPath);
        }
    }
}