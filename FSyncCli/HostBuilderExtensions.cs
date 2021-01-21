using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FSyncCli
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder UseFSyncConsoleLifetime(this IHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureServices((context, collection) =>
            {
                collection.AddSingleton<IHostLifetime, FSyncConsoleLifetime>();
            });
        }
    }
}