﻿using System;
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

        private PipelineItem CheckIfFileAlreadyExistsTransform(PipelineItem pipelineItemBase)
        {
            var isNewKey = _existingFiles.TryAdd(pipelineItemBase.Hash, pipelineItemBase.Descriptor);

            if (!isNewKey)
            {
                pipelineItemBase.IsDuplicate = true;
            }

            return pipelineItemBase;
        }

        private void LogFilteredFiles(PipelineItem skippedFileDescriptor)
        {
            var fileWhichExists = _existingFiles[skippedFileDescriptor.Hash];

            _logger.LogWarning(
                $"File {skippedFileDescriptor.Descriptor.FullPath} has been identified as duplicate of  {fileWhichExists?.FullPath}");
        }
    }
}