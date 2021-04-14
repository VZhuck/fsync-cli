using FSyncCli.Domain;

namespace FSyncCli.Core
{
    public interface ITargetPathResolver
    {
        string Resolve(FileMetadataInfo fileMetaInfo);
    }
}