using System;
using FSyncCli.Utils;
using Xunit;

namespace FSyncCli.Tests.Utils
{
    public class DateTimeOffsetUtilsTests
    {
        [Fact]
        public void TryParse_ParseValidDateWithValidOffset_Success()
        {
            // arrange
            var dateStr = "2021-03-23T02:01:28";
            var offsetStr = "02:00";
            var expectedResult = new DateTimeOffset(
                year: 2021, month: 03, day: 23,
                hour: 2, minute: 1, second: 28,
                offset: new TimeSpan(hours: 2, minutes: 0, seconds: 0));

            // act
            var result = DateTimeOffsetUtils.TryParse(dateTimeStr: dateStr, offsetStr: offsetStr);

            // assert
            Assert.Equal(expected: expectedResult, actual: result);
        }

        [Fact]
        public void TryParse_ParseValidDateWithNoOffset_Success()
        {
            // arrange
            var dateStr = "2021-03-23T02:01:28";
            
            var expectedResult = new DateTimeOffset(
                year: 2021, month: 03, day: 23,
                hour: 2, minute: 1, second: 28,
                offset: DateTimeOffset.Now.Offset);

            // act
            var result = DateTimeOffsetUtils.TryParse(dateTimeStr: dateStr);

            // assert
            Assert.Equal(expected: expectedResult, actual: result);
        }

        [Fact]
        public void TryParse_InvalidDateAndOffset_ReturnsNull()
        {
            // arrange
            var dateStr = "invalidString";
            var offset = "invalidString";

            // act
            var result = DateTimeOffsetUtils.TryParse(dateTimeStr: dateStr, offsetStr: offset);

            // assert
            Assert.Null(result);
        }
    }
}