using System;
using System.IO;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace FSyncCli.Core
{
    public class FSyncPipeline : IFSyncPipeline, IDisposable
    {
        private readonly ILogger<FSyncPipeline> _logger;

        private readonly IServiceScope _scope;
        private readonly ITargetBlock<DirectoryInfo> _pipeline;
        private readonly Task _completion;
        private readonly IPipelineContext _context;

        public FSyncPipeline(IServiceScope scope, IPipelineContext context,
            ITargetBlock<DirectoryInfo> pipeline, Task completion, ILogger<FSyncPipeline> logger)
        {
            _scope = scope;
            _pipeline = pipeline;
            _context = context;
            _completion = completion;

            _logger = logger ?? new NullLogger<FSyncPipeline>();
        }

        public Task StartPipeline()
        {
            _logger.LogInformation($"Starting {nameof(FSyncPipeline)} with {nameof(PipelineContext)}:", _context);

            foreach (var directoryInfo in _context.SourceDirs)
            {
                _pipeline.Post(directoryInfo);
            }

            _pipeline.Complete();

            _logger.LogInformation(
                $"Last {nameof(FSyncPipeline)} item has been submitted for execution.Waiting pipeline completion...");

            return _completion;
        }

        public void Dispose()
        {
            _scope?.Dispose();
        }
    }
}