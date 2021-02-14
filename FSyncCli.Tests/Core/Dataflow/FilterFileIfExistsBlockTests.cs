using System;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;
using FluentAssertions;
using FSyncCli.Core.Dataflow;
using FSyncCli.Domain;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FSyncCli.Tests.Core.Dataflow
{
    public class FilterFileIfExistsBlockTests
    {
        private readonly Mock<ILogger<FilterFileIfExistsBlock>> _loggerMock;

        public FilterFileIfExistsBlockTests()
        {
            _loggerMock = new Mock<ILogger<FilterFileIfExistsBlock>>()
                .As<ILogger<FilterFileIfExistsBlock>>();

        }

        [Fact]
        public void GetNonDuplicatedFiles_PassAllOfThemToOutput_Successful()
        {
            // arrange
            var noDupFiles = new[]
            {
                new FileMetadataInfo(@"c:/folder/testFile1") {Hash = Guid.Parse("D8E675A9-1069-4A61-A766-97B6E78B9468")},
                new FileMetadataInfo(@"c:/folder/testFile2") {Hash = Guid.Parse("5CD5B8F0-3E60-4537-9C9A-5871F9E31967")},
                new FileMetadataInfo(@"c:/folder/testFile3") {Hash = Guid.Parse("08690017-DDEC-4A8D-981F-16987DD4CBB8")}
            };

            var result = new List<FileMetadataInfo>();

            var sut = new FilterFileIfExistsBlock(_loggerMock.Object);
            sut.Block.LinkTo(
                new ActionBlock<FileMetadataInfo>(info => result.Add(info)),
                new DataflowLinkOptions() { PropagateCompletion = true });

            // act
            Array.ForEach(noDupFiles, fileDescriptor => sut.Block.Post(fileDescriptor));
            sut.Block.Complete();
            sut.Block.Completion.Wait();

            // assert
            result.Should().Equal(noDupFiles);
        }

        [Fact]
        public void PostDuplicatedFiles_DuplicatesWillBeFiltered_Successful()
        {
            // arrange
            var noDupFiles = new[]
            {
                new FileMetadataInfo(@"c:/folder/testFile1") {Hash = Guid.Parse("D8E675A9-1069-4A61-A766-97B6E78B9468")},
                new FileMetadataInfo(@"c:/folder/testFile2") {Hash = Guid.Parse("D8E675A9-1069-4A61-A766-97B6E78B9468")},
                new FileMetadataInfo(@"c:/folder/testFile3") {Hash = Guid.Parse("08690017-DDEC-4A8D-981F-16987DD4CBB8")}
            };

            var result = new List<FileMetadataInfo>();

            var sut = new FilterFileIfExistsBlock(_loggerMock.Object);
            sut.Block.LinkTo(
                new ActionBlock<FileMetadataInfo>(info => result.Add(info)),
                new DataflowLinkOptions() { PropagateCompletion = true });

            // act
            Array.ForEach(noDupFiles, fileDescriptor => sut.Block.Post(fileDescriptor));
            sut.Block.Complete();
            sut.Block.Completion.Wait();

            // assert
            result.Should()
                .NotBeEmpty()
                .And.HaveCount(2)
                .And.Contain(info =>
                    info.Hash == Guid.Parse("D8E675A9-1069-4A61-A766-97B6E78B9468") ||
                    info.Hash == Guid.Parse("08690017-DDEC-4A8D-981F-16987DD4CBB8"));
        }
    }
}