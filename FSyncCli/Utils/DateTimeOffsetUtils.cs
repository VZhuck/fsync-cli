using System;
using System.Globalization;

namespace FSyncCli.Utils
{
    public class DateTimeOffsetUtils
    {
        private static readonly string[] DateTimeFormats = {
            // Basic formats
            "yyyyMMddTHHmmsszzz",
            "yyyyMMddTHHmmsszz",
            "yyyyMMddTHHmmssZ",
            "yyyyMMddTHHmmss",

            // Extended formats
            "yyyy-MM-ddTHH:mm:sszzz",
            "yyyy-MM-ddTHH:mm:sszz",
            "yyyy-MM-ddTHH:mm:ssZ",
            "yyyy-MM-ddTHH:mm:ss",

            // Image DateTime
            "yyyy:MM:dd HH:mm:sszzz",
            "yyyy:MM:dd HH:mm:sszz",
            "yyyy:MM:dd HH:mm:ssZ",
            "yyyy:MM:dd HH:mm:ss",

            // All of the above with reduced accuracy
            "yyyyMMddTHHmmzzz",
            "yyyyMMddTHHmmzz",
            "yyyyMMddTHHmmZ",
            "yyyy-MM-ddTHH:mmzzz",
            "yyyy-MM-ddTHH:mmzz",
            "yyyy-MM-ddTHH:mmZ"
        };

        public static DateTimeOffset? TryParse(string dateTimeStr, string offsetStr = null)
        {
            if (string.IsNullOrWhiteSpace(offsetStr) || TimeSpan.TryParse(offsetStr, out var offset) == false)
            {
                return TryParse(dateTimeStr, DateTimeOffset.Now.Offset);
            }

            return TryParse(dateTimeStr, offset);
        }

        public static DateTimeOffset? TryParse(string dateTimeStr, TimeSpan offset)
        {
            if (string.IsNullOrWhiteSpace(dateTimeStr) || 
                DateTime.TryParseExact( dateTimeStr, DateTimeFormats, null, DateTimeStyles.None, out var dateTime) == false)
            {
                return null;
            }

            return new DateTimeOffset(dateTime, offset);
        }

    }
}