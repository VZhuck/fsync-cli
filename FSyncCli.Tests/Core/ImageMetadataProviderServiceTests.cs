using System;
using System.IO;
using System.Reflection;
using FSyncCli.Core;
using FSyncCli.Core.Metadata;
using FSyncCli.Utils;
using Xunit;
using Xunit.Abstractions;

namespace FSyncCli.Tests.Core
{
    public class ImageMetadataProviderServiceTests
    {
        private readonly ITestOutputHelper _output;

        public ImageMetadataProviderServiceTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [InlineData(".jpg", true)]
        [InlineData(".JPG", true)]
        [InlineData(".NEf", true)]
        public void IsSupportedFormat_CheckIfFormatSupported(string formatExtension, bool expectedResult)
        {
            // arrange
            var sut = new ImageMetadataProviderService();

            // act
            var result = sut.IsSupportedImageFormat(formatExtension);

            // assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void ExtractImageMetadata_ExtractJpgMetadata_ReturnPredifinedMetadata()
        {
            // arrange
            var sut = new ImageMetadataProviderService();
            var jpgFileStream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("FSyncCli.Tests.TestFiles.TestJpgWithMetadata.JPG");
            
            var make = "NIKON CORPORATION";
            var original = DateTimeOffsetUtils.TryParse("2021:02:24 22:27:11", "-04:00");
            
            // act
            var result = sut.ExtractImageMetadata(jpgFileStream);

            // assert
            Assert.Equal(expected:make , result.Make);
            Assert.Equal(expected:original, result.OriginalDateTimeOffset);
        }

        [Fact] //(Skip = "Manual Run Only")
        public void ExtractImageMetadata_ExtractNefRawMetadata()
        {
            // arrange
            var sut = new ImageMetadataProviderService();
            // NEF files are too large to store them in repo, so local file will be used
            var nefFilePath = @"c:\!FsyncTest\\WYR_1199.NEF";

            var make = "NIKON CORPORATION";
            var original = DateTimeOffset.Parse("2021-03-16T15:11:06-04:00");

            // act
            using var imageFileStream = File.OpenRead(nefFilePath);
            var result = sut.ExtractImageMetadata(imageFileStream);

            // assert
            Assert.Equal(expected: make, result.Make);
            Assert.Equal(expected: original, result.OriginalDateTimeOffset);
        }
    }
}