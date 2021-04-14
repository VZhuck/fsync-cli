using System;
using System.Data;

namespace FSyncCli.Domain
{
    public class ImageMetaData
    {
        public string Make { get; set; }


        public DateTimeOffset? OriginalDateTimeOffset  { get; set; }

        public DateTime? XmpCreatedDate { get; set; }
    }

    public static class ImageMetaDataExtensions
    {
        public static ImageMetaData EnrichFrom(this ImageMetaData target, ImageMetaData sourceMetaData)
        {
            if (string.IsNullOrWhiteSpace(target.Make))
            {
                target.Make = sourceMetaData.Make;
            }

            target.OriginalDateTimeOffset ??= sourceMetaData.OriginalDateTimeOffset;

            target.XmpCreatedDate ??= sourceMetaData.XmpCreatedDate;

            return target;
        }
    }
}