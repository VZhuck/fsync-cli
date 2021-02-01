using System;
using System.IO;
using System.Threading.Tasks;
using FSyncCli.Core;
using FSyncCli.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;

// System.CommandLine API auto conversion options with arguments into main (X,y) static method
// System.CommandLine.DragonFruit
// dotnet-suggest - support for tab completion.

namespace FSyncCli
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(AppDomain.CurrentDomain.BaseDirectory + "\\appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();


            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging(config =>
                    {
                        config.ClearProviders();
                        config.AddProvider(new SerilogLoggerProvider(Log.Logger));
                    });

                    services.AddSingleton<IFSyncCmdArgs>(FSyncCmdArgs.CreateFSyncCmdArgs(args));
                    services.AddTransient<IFSyncCmdApp, FSyncCmdApp>();
                    services.AddTransient<IFSyncPipeline, FSyncPipeline>();
                    services.AddTransient<ISourceDirService, SourceDirService>();
                    services.AddTransient<IPipelineBuilder, PipelineBuilder>();

                });

            builder.Properties[typeof(FSyncCmdArgs)] = FSyncCmdArgs.CreateFSyncCmdArgs(args);

            await builder.UseFSyncConsoleLifetime().Build().RunAsync();
        }
    }
}
