using System;
using System.IO;
using System.Linq;
using FSyncCli.Domain;
using FSyncCli.Utils;
using ImageMagick;

namespace FSyncCli.Core.Metadata
{
    public class ImageMetadataProviderService : IImageMetadataProviderService
    {
        private readonly MagickFormat[] _imageFormats =
        {
            MagickFormat.Jpg, MagickFormat.Png, MagickFormat.Gif, MagickFormat.Nef,
            MagickFormat.Cr2, MagickFormat.Cr3, MagickFormat.Crw
        };

        public bool IsSupportedImageFormat(string fileExtension)
        {
            string formatName = fileExtension.TrimStart('.');

            var isValidMagicFormat =
                Enum.TryParse(formatName, true, out MagickFormat result)
                && _imageFormats.Contains(result);

            return isValidMagicFormat;
        }

        public ImageMetaData ExtractImageMetadata(Stream imageFileStream)
        {
            using var image = new MagickImage();
            // Ping offers better performance as load metadata only
            image.Ping(imageFileStream);

            var imageMetadata = GetExifMetaData(image) ?? new ImageMetaData();
            var xmpAndDngMetadata = GetRawFormatMetaData(image);

            imageMetadata.EnrichFrom(xmpAndDngMetadata);
            return imageMetadata;
        }

        protected ImageMetaData GetExifMetaData(MagickImage image)
        {
            // No need to check file format as GetExifProfile is safe operation
            var profile = image.GetExifProfile();

            if (profile == null)
            {
                return null;
            }

            var makeTag = profile.GetValue(ExifTag.Make);
            var dateOriginalTag = profile.GetValue(ExifTag.DateTimeOriginal);
            var originalOffsetTag = profile.GetValue(ExifTag.OffsetTimeOriginal);

            var originalDateTimeOffset =
                DateTimeOffsetUtils.TryParse(dateOriginalTag?.Value, originalOffsetTag?.Value);

            var imageExifData = new ImageMetaData
            {
                Make = makeTag?.Value,
                OriginalDateTimeOffset = originalDateTimeOffset
            };

            return imageExifData;
        }

        protected ImageMetaData GetRawFormatMetaData(MagickImage image)
        {
            var originalDateTimeStr = image.GetAttribute("exif:DateTimeDigitized")
                                      ?? image.GetAttribute("xmp:CreateDate");

            var originalOffsetStr = image.GetAttribute("exif:Tag 36881") //ExifTag.OffsetTimeOriginal
                                    ?? image.GetAttribute("exif:OffsetTimeOriginal");

            var make = image.GetAttribute("exif:make")
                       ?? image.GetAttribute("tiff:make")
                       ?? image.GetAttribute("dng:make");

            var originalDateTimeOffset =
                DateTimeOffsetUtils.TryParse(originalDateTimeStr, originalOffsetStr);

            var imageExifData = new ImageMetaData
            {
                Make = make,
                OriginalDateTimeOffset = originalDateTimeOffset
            };

            return imageExifData;
        }
    }
}