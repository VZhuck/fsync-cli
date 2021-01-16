using System;

namespace TestApp
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    using System;
    using System.IO;
    using System.Threading.Tasks;

    namespace iStradaCli
    {
        internal class Program
        {
            private static async Task<int> Main(string[] args)
            {
               var builder = new HostBuilder()
                    .ConfigureServices((hostContext, services) =>
                    {
                    });

                try
                {
                    return await builder.RunCommandLineApplicationAsync<iStradaCmd>(args);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return 1;
                }
            }
        }
    }
}
