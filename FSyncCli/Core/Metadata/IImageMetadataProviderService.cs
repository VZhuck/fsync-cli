using System.IO;
using FSyncCli.Domain;

namespace FSyncCli.Core.Metadata
{
    public interface IImageMetadataProviderService
    {
        bool IsSupportedImageFormat(string fileExtension);
        ImageMetaData ExtractImageMetadata(Stream imageFileStream);
    }
}