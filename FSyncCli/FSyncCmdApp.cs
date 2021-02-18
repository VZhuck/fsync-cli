using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FSyncCli.Core;
using Microsoft.Extensions.Logging;

namespace FSyncCli
{
    public class FSyncCmdApp : IFSyncCmdApp
    {
        private readonly string[] _args;
        private readonly ILogger<FSyncCmdApp> _logger;
        private readonly RootCommand _rootCommand;
        private readonly IPipelineBuilder _pipelineBuilder;

        public FSyncCmdApp(IFSyncCmdArgs fSyncCmdArgs, ILogger<FSyncCmdApp> logger, IPipelineBuilder pipelineBuilder)
        {
            _logger = logger;
            _args = fSyncCmdArgs.Args;
            _pipelineBuilder = pipelineBuilder;

            // Create a root command with some options
            _rootCommand = new RootCommand
             {
                 new Option<DirectoryInfo>(
                     aliases: new [] {"--source-dir", "-s"},
                     description: "source folder, which will be analyzed"),
                 new Option<DirectoryInfo>(
                     aliases: new [] {"--target-dir", "-t"},
                     description: "folder, which will be used as a target folder to compare with and copy to"),
                 new Option<bool>(
                     aliases: new [] {"--dry-run", "-dr"},
                     description: "when 'true' run simulation (no changes are made)")
             };

            _rootCommand.Description = "fSync-cli is a small tool, which is targeted to analyze source folder to and copy/sync all its files with target folder, excluding all duplicates.";

            // Note that the parameters of the handler method are matched according to the names of the options
            _rootCommand.Handler = CommandHandler.Create<DirectoryInfo, DirectoryInfo>(
                async (sourceDir, targetDir) =>
            {
                _logger.LogInformation($"source folder is: {sourceDir}");
                _logger.LogInformation($"target folder is: {targetDir}");


                var pipeline = _pipelineBuilder
                    .WithSourceDirs(new[] { sourceDir })
                    .WithTargetDir(targetDir)
                    .CreateDefaultPipeline()
                    .Build();

                try
                {
                    await pipeline.StartPipeline();
                }
                catch (AggregateException e)
                {
                    _logger.LogError(e.Flatten().ToString());
                    throw;
                }
                catch (Exception e)
                {
                    _logger.LogError(e.ToString());
                    throw;
                }
            });

        }

        public Task<int> RunAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Running {nameof(FSyncCmdApp)}... ");

            return _rootCommand.InvokeAsync(_args);
        }
    }
}