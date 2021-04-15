using FSyncCli.Domain;

namespace FSyncCli.Core.Metadata
{
    public interface ITargetPathResolver
    {
        string ResolveTargetDirPath(string targetDir, FileMetadataInfo fileInfo,
            FilePathMetadataInfo filePathMetadata, ImageMetaData imgMetaData);

        string ResolveFileName(string targetDir, FileMetadataInfo fileInfo, ImageMetaData imgMetaData);
    }
}