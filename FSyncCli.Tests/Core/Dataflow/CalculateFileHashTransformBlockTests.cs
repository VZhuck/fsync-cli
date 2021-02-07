using System;
using System.IO;
using System.Text;
using System.Threading.Tasks.Dataflow;
using FSyncCli.Core.Dataflow;
using FSyncCli.Domain;
using FSyncCli.Infrastructure;
using Moq;
using Xunit;

namespace FSyncCli.Tests.Core.Dataflow
{
    public class CalculateFileHashTransformBlockTests: IDisposable
    {
        private readonly Mock<ISourceDirService> _sourceDirServiceMock;
        private readonly Stream _testStream;

        public CalculateFileHashTransformBlockTests()
        {
            _sourceDirServiceMock = new Mock<ISourceDirService>();

            // convert string to stream
            byte[] byteArray = Encoding.ASCII.GetBytes("Test File Content");
            _testStream = new MemoryStream(byteArray);
        }

        [Fact]
        public void EnumerateSourceFilesBlock_TransformDirIntoFiles_Successfully()
        {
            //arrange
            _sourceDirServiceMock
                .Setup(
                    sourceSrv => sourceSrv.GetFilesContentAsStream(
                        It.IsAny<FileMetadataInfo>()))
                .Returns(_testStream);


            var sut = new CalculateFileHashTransformBlock(_sourceDirServiceMock.Object);

            //act
            sut.Block
                .Post(new FileMetadataInfo("fakeFilePath"));
            
            sut.Block.Complete();

            var actualResult = sut.Block.Receive();
            var expected = Guid.Parse("33d0e86568a7b2122b75c20e2fe4061c");

            //assert
            Assert.Equal(expected, actualResult.Hash);
        }

        public void Dispose()
        {
            _testStream?.Dispose();
        }
    }
}