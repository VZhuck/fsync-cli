using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using FSyncCli.Domain;
using FSyncCli.Infrastructure;
using Microsoft.Extensions.Logging;

namespace FSyncCli.Core
{
    public class PipelineBuilder : IPipelineBuilder
    {
        // Services
        private readonly ILogger<PipelineBuilder> _logger;
        private readonly ISourceDirService _sourceDirService;

        private List<DirectoryInfo> _sourceDirs= new List<DirectoryInfo>();
        private DirectoryInfo _targetDir;

        private IPropagatorBlock<DirectoryInfo, FileMetadataInfo> _pipeline;

        public PipelineBuilder(ILogger<PipelineBuilder> logger, ISourceDirService sourceDirService)
        {
            _logger = logger;
            _sourceDirService = sourceDirService;
        }

        public PipelineBuilder WithSourceDirs(IEnumerable<DirectoryInfo> sourceDirs)
        {
            _sourceDirs.AddRange(sourceDirs ?? throw new ArgumentNullException(nameof(sourceDirs)));
            
            return this;
        }

        public PipelineBuilder WithTargetDir(DirectoryInfo targetDir)
        {
            if (_targetDir != null)
            {
                throw new InvalidOperationException($"{nameof(targetDir)} has been already defined. System can have only 1 target.");
            }

            _targetDir = targetDir ?? throw new ArgumentNullException(nameof(targetDir));

            return this;
        }

        public IPipelineBuilder CreateDefaultPipeline()
        {
            var linkOptions = new DataflowLinkOptions { PropagateCompletion = true };

            var folderToFilesBlock = new TransformManyBlock<DirectoryInfo, FileMetadataInfo>(_sourceDirService.GetSourcesFiles);

            var calculateFileHashBlock = new TransformBlock<FileMetadataInfo, FileMetadataInfo>(_sourceDirService.GetFileDescriptorWithCalculatedHash);

            var copyFileToTarget = new ActionBlock<FileMetadataInfo>(info =>
            {
                var destFilePath = Path.Combine(_targetDir.FullName, info.Name);
                File.Copy(info.FullPath, destFilePath);

                _logger.LogInformation($"File {info.FullPath} has been copied to: {destFilePath}");
            });

            //var loggingBlock = new ActionBlock<FileMetadataInfo>(info =>
            //    _logger.LogInformation($"File {info.Name} has been processed. Hash: {info.Hash}")
            //);

            folderToFilesBlock.LinkTo(calculateFileHashBlock, linkOptions);
            calculateFileHashBlock.LinkTo(copyFileToTarget, linkOptions);
            //copyFileToTarget.LinkTo(loggingBlock, linkOptions);

            _pipeline = folderToFilesBlock;

            return this;
        }
        

        public Func<Task> Build()
        {
            var runPipelineAction = new Func<Task>(() =>
            {
                foreach (var directoryInfo in _sourceDirs)
                {
                    _pipeline.Post<DirectoryInfo>(directoryInfo);
                }

                _pipeline.Complete();

                return _pipeline.Completion;
            });

            return runPipelineAction;
        }
    }
}