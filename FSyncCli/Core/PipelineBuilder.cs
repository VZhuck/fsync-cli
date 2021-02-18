using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using FSyncCli.Core.Dataflow;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSyncCli.Core
{
    public class PipelineBuilder : IPipelineBuilder
    {
        // Services
        private readonly ILogger<PipelineBuilder> _logger;
        private readonly IServiceScope _scope;

        // State
        private readonly IPipelineContext _context;

        //Config Actions
        private Action<IPipelineContext> _configPipelineContext;
        private Func<Tuple<ITargetBlock<DirectoryInfo>, IDataflowBlock>> _pipelineConfigAction;

        public PipelineBuilder(ILogger<PipelineBuilder> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _scope = serviceProvider.CreateScope();

            _context = _scope.ServiceProvider.GetRequiredService<IPipelineContext>();
        }

        public PipelineBuilder WithSourceDirs(IEnumerable<DirectoryInfo> sourceDirs)
        {
            var configContextDelegate = new Action<IPipelineContext>((context) =>
            {
                context.SourceDirs.AddRange(sourceDirs ?? throw new ArgumentNullException(nameof(sourceDirs)));
            });

            _configPipelineContext += configContextDelegate;

            return this;
        }

        public PipelineBuilder WithTargetDir(DirectoryInfo targetDir)
        {
            var configContextDelegate = new Action<IPipelineContext>((context) =>
            {
                context.TargetDir = targetDir ?? throw new ArgumentNullException(nameof(targetDir));
            });

            _configPipelineContext += configContextDelegate;

            return this;
        }

        public IPipelineBuilder CreateDefaultPipeline()
        {
            _pipelineConfigAction = () =>
                {

                    var linkOptions = new DataflowLinkOptions { PropagateCompletion = true };

                    var folderToFilesBlock = GetRequiredService<EnumerateSourceFilesTransformToManyBlock>();

                    var calculateFileHashBlock = GetRequiredService<CalculateFileHashTransformBlock>();

                    var filterFileByHashBlock = GetRequiredService<FilterFileIfExistsBlock>();

                    var copyFileToTarget = GetRequiredService<CopyFileBlock>();


                    folderToFilesBlock.Block.LinkTo(calculateFileHashBlock.Block, linkOptions);
                    calculateFileHashBlock.Block.LinkTo(filterFileByHashBlock.Block, linkOptions);
                    filterFileByHashBlock.Block.LinkTo(copyFileToTarget.Block, linkOptions);

                    return new Tuple<ITargetBlock<DirectoryInfo>, IDataflowBlock>(
                        folderToFilesBlock.Block, copyFileToTarget.Block);
                };

            return this;
        }

        public IFSyncPipeline Build()
        {
            _logger.LogTrace($"Configuring {nameof(IPipelineContext)}...");
            _configPipelineContext?.Invoke(_context);
            _logger.LogTrace($"{nameof(IPipelineContext)} has been configured");

            _logger.LogTrace($"Creating dataflow network...");
            var dataflowPipeline = _pipelineConfigAction();
            _logger.LogTrace($"Dataflow network has been created");

            _logger.LogTrace($"Creating {nameof(FSyncPipeline)}...");
            var fSyncPipelineLogger = _scope.ServiceProvider.GetRequiredService<ILogger<FSyncPipeline>>();

            var pipeline = new FSyncPipeline(
                _scope, _context, dataflowPipeline.Item1, dataflowPipeline.Item2.Completion, fSyncPipelineLogger);
            _logger.LogTrace($"{nameof(FSyncPipeline)} is ready to use");
            
            return pipeline;
        }

        #region Helpers

        


        private T GetRequiredService<T>()
        {
            return _scope.ServiceProvider.GetRequiredService<T>();
        }

        #endregion
    }
}