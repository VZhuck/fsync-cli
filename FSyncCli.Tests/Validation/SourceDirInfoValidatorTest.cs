using System;
using System.IO;
using FSyncCli.Validation;
using Xunit;

namespace FSyncCli.Tests.Validation
{
    public class SourceDirInfoValidatorTest :IDisposable
    {
        private readonly DirectoryInfo _dirInfo;

        public SourceDirInfoValidatorTest()
        {
            var tmpPath = Path.GetTempPath();
            var tmpTestDirPath = Path.Combine(tmpPath, Guid.NewGuid().ToString());

            Directory.CreateDirectory(tmpTestDirPath);

            _dirInfo = new DirectoryInfo(tmpTestDirPath);
        }

        [Fact]
        public void Validate_ExistingDir_ValidationFails()
        {
            //arrange
            var sut = new SourceDirInfoValidator();

            //act
            var result = sut.Validate(_dirInfo);

            //assert
            Assert.True(result.IsValid, "Validation succeeded as dirInfo does exists.");
        }

        [Fact]
        public void Validate_NotExistingDir_ValidationFails()
        {
            //arrange
            var nonExistDirInfo = new DirectoryInfo(Path.Combine(_dirInfo.FullName, "NOT_EXISTS"));
            var sut = new SourceDirInfoValidator();

            //act
            var result = sut.Validate(nonExistDirInfo);

            //assert
            Assert.False(result.IsValid, "Validation should NOT succeeded as dirInfo does not exists.");
        }
        public void Dispose()
        {
            if (_dirInfo.Exists)
            {
                _dirInfo.Delete(true);
            }
        }
    }
}
