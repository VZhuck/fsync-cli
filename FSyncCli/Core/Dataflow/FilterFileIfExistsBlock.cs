using System;
using System.Collections;
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

        public IPropagatorBlock<FileMetadataInfo, FileMetadataInfo> Block { get; }

        public FilterFileIfExistsBlock(ILogger<FilterFileIfExistsBlock> logger)
        {
            _logger = logger;

            Block = new TransformBlock<FileMetadataInfo, FileMetadataInfo>(CheckIfFileAlreadyExistsTransform);

            var skippedLogging = new ActionBlock<FileMetadataInfo>(LogFilteredFiles);

            Block.LinkTo(skippedLogging,
                new DataflowLinkOptions { PropagateCompletion = true },
                fd => fd.IsDuplicate);

        }

        private FileMetadataInfo CheckIfFileAlreadyExistsTransform(FileMetadataInfo fileDescriptor)
        {
            var isNewKey = _existingFiles.TryAdd(fileDescriptor.Hash, fileDescriptor);

            if (!isNewKey)
            {
                fileDescriptor.IsDuplicate = true;
            }

            return fileDescriptor;
        }

        private void LogFilteredFiles(FileMetadataInfo skippedFileDescriptor)
        {
            var fileWhichExists = _existingFiles[skippedFileDescriptor.Hash];

            _logger.LogWarning(
                $"File {skippedFileDescriptor.FullPath} has been identified as duplicate of  {fileWhichExists?.FullPath}");
        }
    }
}