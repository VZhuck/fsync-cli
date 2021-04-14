using System;
using FSyncCli.Core;
using FSyncCli.Core.Metadata;
using FSyncCli.Domain;
using Xunit;

namespace FSyncCli.Tests.Core
{
    public class TargetPathResolverTests
    {
        private string _regexStr = @"(?<year>\d{4}).(?<month>\d{2}).(?<day>\d{2})\s*(?<folderName>\S*)";
        private TargetPathResolver _targetPathResolver;

        public TargetPathResolverTests()
        {
            _targetPathResolver = new TargetPathResolver();
        }

        [Fact]
        public void Resolve_ValidFileMetaInfoIntoPath_Success()
        {
            // arrange
            var modified = new DateTime(2021, 01, 30);
            var fileMetaInfo = new FileMetadataInfo("C:\\source\\testImg.jpeg")
            {
                LastWriteTime = modified,
                LastWriteTimeUtc = modified.ToUniversalTime()
            };

            // act
            var result =_targetPathResolver.Resolve(fileMetaInfo);

            // assert
            Assert.Equal("2021.01.30 - source", result, ignoreCase: true);
        }
    }
}