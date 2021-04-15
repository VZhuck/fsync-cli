using System;
using System.Collections.Generic;
using FSyncCli.Core.Metadata;
using FSyncCli.Domain;
using Xunit;
using Xunit.Abstractions;

namespace FSyncCli.Tests.Core.Metadata
{
    public class FilePathMetadataExtractServiceTests
    {
        private readonly ITestOutputHelper _output;

        public FilePathMetadataExtractServiceTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void ExtractFilePathMetadata_PathMetadataCaptured(string testPath, string basePath, FilePathMetadataInfo expectedMetadata)
        {
            // arrange
            var sut = new FilePathMetadataExtractService();
            
            // act
            var result = sut.ExtractFilePathMetadata(testPath, basePath);

            // assert
            Assert.Equal(expectedMetadata, result);
        }

        public static IEnumerable<object[]> Data()
        {
            yield return new object[]
            {
                @"c:\testFolder\2021.21.12 - Description\rest\file.jpg",
                @"c:\testFolder",
                new FilePathMetadataInfo
                {
                    From = new ImpreciseDate(2021, 21, 12),
                    To = null,
                    CategoryName = @"Description",
                    CategorySubPath = "rest"
                }
            };
            yield return new object[]
            {
                @"c:\testFolder\2020.02-2022.12 - Description\rest\file.jpg",
                @"c:\testFolder",
                new FilePathMetadataInfo
                {
                    From = new ImpreciseDate(2020, 02, null),
                    To = new ImpreciseDate(2022, 12, null),
                    CategoryName = @"Description",
                    CategorySubPath = "rest"
                }
            };
            yield return new object[]
            {
                @"c:/testFolder/2020.02-2022.12.21 - Description/rest/file.jpg",
                @"c:\",
                new FilePathMetadataInfo
                {
                    From = new ImpreciseDate(2020, 02, null),
                    To = new ImpreciseDate(2022, 12, 21),
                    CategoryName = @"Description",
                    CategorySubPath = "rest"
                }
            };
            yield return new object[]
            {
                @"c:/baseDir/file.jpg",
                @"c:/baseDir",
                null
            };

            yield return new object[]
            {
                @"c:/baseDir/Description/file.jpg",
                @"c:/baseDir/",
                new FilePathMetadataInfo
                {
                    From = null,
                    To = null,
                    CategoryName = @"Description",
                    CategorySubPath = null
                }
            };

            yield return new object[]
            {
                @"c:/testFolder/No Date Description/sub_path/file.jpg",
                @"c:\testFolder\",
                new FilePathMetadataInfo
                {
                    From = null,
                    To = null,
                    CategoryName = @"No Date Description",
                    CategorySubPath = "sub_path"
                }
            };
        }
    }
}