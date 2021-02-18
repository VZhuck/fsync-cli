using System;
using System.Collections.Concurrent;
using System.Threading.Tasks.Dataflow;
using FSyncCli.Domain;
using Microsoft.Extensions.Logging;

namespace FSyncCli.Core.Dataflow
{
    public class FilterFileIfExistsBlock
    {
        // Services
        private readonly ILogger<FilterFileIfExistsBlock> _logger;

        // State
        private readonly ConcurrentDictionary<Guid, FileMetadataInfo> _existingFiles = new ConcurrentDictionary<Guid, FileMetadataInfo>();

        public IPropagatorBlock<PipelineItem, PipelineItem> Block { get; }

        public FilterFileIfExistsBlock(ILogger<FilterFileIfExistsBlock> logger)
        {
            _logger = logger;

            Block = new TransformBlock<PipelineItem, PipelineItem>(CheckIfFileAlreadyExistsTransform);

            var skippedLogging = new ActionBlock<PipelineItem>(LogFilteredFiles);

            Block.LinkTo(skippedLogging,
                new DataflowLinkOptions { PropagateCompletion = true },
                fd => fd.IsDuplicate);

        }

        private PipelineItem CheckIfFileAlreadyExistsTransform(PipelineItem pipelineItem)
        {
            var isNewKey = _existingFiles.TryAdd(pipelineItem.Hash, pipelineItem.Item);

            if (!isNewKey)
            {
                pipelineItem.IsDuplicate = true;
            }

            return pipelineItem;
        }

        private void LogFilteredFiles(PipelineItem skippedFileDescriptor)
        {
            var fileWhichExists = _existingFiles[skippedFileDescriptor.Hash];

            _logger.LogWarning(
                $"File {skippedFileDescriptor.Item.FullPath} has been identified as duplicate of  {fileWhichExists?.FullPath}");
        }
    }
}