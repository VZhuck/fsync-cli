using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using FSyncCli.Domain;
using FSyncCli.Infrastructure;
using Microsoft.Extensions.Logging;

namespace FSyncCli.Core
{
    public class FSyncPipeline : IFSyncPipeline
    {
        private readonly ILogger<FSyncPipeline> _logger;
        private TransformManyBlock<DirectoryInfo, FileMetadataInfo> _pipeline;
        
        public FSyncPipeline(ILogger<FSyncPipeline> logger, ISourceDirService sourceDirService)
        {
            _logger = logger;

            
        }

        public Task RunFSyncForInAndOutSources(IEnumerable<DirectoryInfo> sourceDirs, DirectoryInfo targetRootDir)
        {
            _logger.LogInformation("Starting FSyncPipeline pipeline for selected in/out sources...");

            foreach (var directoryInfo in sourceDirs)
            {
                _pipeline.Post(directoryInfo);
            }
            _pipeline.Complete();

            return _pipeline.Completion;
        }
    }
}