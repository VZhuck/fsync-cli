using System.IO;
using System.Threading.Tasks.Dataflow;
using FSyncCli.Domain;
using FSyncCli.Infrastructure;

namespace FSyncCli.Core.Dataflow
{
    public class EnumerateSourceFilesTransformToManyBlock
    {
        public IPropagatorBlock<DirectoryInfo, FileMetadataInfo> Block { get; }

        public EnumerateSourceFilesTransformToManyBlock (ISourceDirService sourceDirService)
        {
            Block = new TransformManyBlock<DirectoryInfo, FileMetadataInfo>(sourceDirService.GetSourcesFiles);
        }
    }
}