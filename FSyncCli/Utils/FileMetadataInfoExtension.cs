using FSyncCli.Core;
using FSyncCli.Domain;

namespace FSyncCli.Utils
{
    public static class FileMetadataInfoExtension
    {
        public static PipelineItem AsPipelineItem(this FileMetadataInfo fileMetadataInfo)
        {
            return new PipelineItem(fileMetadataInfo);
        }
    }
}