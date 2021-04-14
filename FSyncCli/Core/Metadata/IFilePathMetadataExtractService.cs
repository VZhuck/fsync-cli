using FSyncCli.Domain;

namespace FSyncCli.Core.Metadata
{
    public interface IFilePathMetadataExtractService
    {
        FilePathMetadataInfo ExtractFilePathMetadata(string filePath, string baseDirPath = null);
    }
}