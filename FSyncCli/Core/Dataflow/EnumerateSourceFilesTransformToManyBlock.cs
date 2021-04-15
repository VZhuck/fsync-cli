using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks.Dataflow;
using FSyncCli.Infrastructure;
using FSyncCli.Utils;

namespace FSyncCli.Core.Dataflow
{
    public class EnumerateSourceFilesTransformToManyBlock
    {
        private readonly IFileRepoService _sourceFileRepoService;

        public IPropagatorBlock<DirectoryInfo, PipelineItem> Block { get; }

        public EnumerateSourceFilesTransformToManyBlock(IFileRepoService sourceFileRepoService)
        {
            _sourceFileRepoService = sourceFileRepoService;

            Block = new TransformManyBlock<DirectoryInfo, PipelineItem>(Transform);
        }

        private IEnumerable<PipelineItem> Transform(DirectoryInfo directoryInfo)
        {
            return _sourceFileRepoService
                .GetFileMetadataInfos(directoryInfo)
                .Select(fileMetadataInfo =>
                {
                    var pipelineItem = fileMetadataInfo.AsPipelineItem();
                    pipelineItem.BaseDirPath = directoryInfo.FullName;
                    return pipelineItem;
                });
        }
    }
}