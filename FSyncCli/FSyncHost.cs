using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FSyncCli
{
    public class FSyncHost : IHostedService
    {
        private readonly string[] _args;
        private ILogger<FSyncHost> _logger;
        private readonly RootCommand _rootCommand;
        private Task<int> _runningTask;

        public FSyncHost(IFSyncCmdArgs fSyncCmdArgs, ILogger<FSyncHost> logger)
        {
            _logger = logger;
            _args = fSyncCmdArgs.Args;

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

            _rootCommand.Description = "fSync-cli is a small tool, which is targeted to analyze source folder to and copy/sync all its files with target folder, excluding all duplicates. ";

            // Note that the parameters of the handler method are matched according to the names of the options
            _rootCommand.Handler = CommandHandler.Create<DirectoryInfo, DirectoryInfo>(
                (sourceDir, targetDir) =>
            {
                

                Console.WriteLine($"source folder is: {sourceDir}");
                Console.WriteLine($"target folder is: {targetDir}");
                Thread.Sleep(3000);
                Console.WriteLine($"After 3 sec .....");
            });

        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _runningTask = Task
                .Run(async () =>
                {
                    await Task.Delay(3000, cancellationToken);
                    return await _rootCommand.InvokeAsync(_args);
                });

            return _runningTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.CompletedTask;
            }
            else
            {
                return _runningTask;
            }
            //_runningTask
        }
    }
}