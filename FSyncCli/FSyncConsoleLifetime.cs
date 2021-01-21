using System;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FSyncCli
{
    public class FSyncConsoleLifetime : IHostLifetime, IDisposable
    {
        #region Private Fields 

        private readonly IFSyncCmdApp _cliService;
        private readonly IHostApplicationLifetime _applicationLifetime;
        private readonly HostOptions _hostOptions;
        private readonly ILogger _logger;

        private readonly ManualResetEvent _shutdownBlock = new ManualResetEvent(false);
        private CancellationTokenRegistration _applicationStartedRegistration;
        private CancellationTokenRegistration _applicationStoppingRegistration;

        private int _exitCode;

        #endregion

        public FSyncConsoleLifetime(IFSyncCmdApp cliService, IHostApplicationLifetime applicationLifetime, IOptions<HostOptions> hostOptions, ILoggerFactory loggerFactory)
        {
            _cliService = cliService ?? throw new ArgumentNullException(nameof(cliService));
            _applicationLifetime = applicationLifetime ?? throw new ArgumentNullException(nameof(applicationLifetime));
            _hostOptions = hostOptions?.Value ?? throw new ArgumentNullException(nameof(hostOptions));
            _logger = loggerFactory.CreateLogger(nameof(FSyncConsoleLifetime));

        }

        public Task WaitForStartAsync(CancellationToken cancellationToken)
        {
            _applicationStartedRegistration = _applicationLifetime.ApplicationStarted.Register(async () =>
            {
                _logger.LogInformation("Application started. Press Ctrl+C to shut down.");

                try
                {
                    _exitCode = await _cliService.RunAsync(cancellationToken);

                }
                catch (Exception e)
                {
                    ExceptionDispatchInfo.Capture(e).Throw();
                }
                finally
                {
                    _applicationLifetime.StopApplication();
                }
            });

            _applicationStoppingRegistration = _applicationLifetime.ApplicationStopping.Register(() =>
            {
                _logger.LogInformation("Application is shutting down...");
            });

            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
            Console.CancelKeyPress += OnCancelKeyPress;

            // Console applications start immediately.
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _shutdownBlock.Set();

            AppDomain.CurrentDomain.ProcessExit -= OnProcessExit;
            Console.CancelKeyPress -= OnCancelKeyPress;

            _applicationStartedRegistration.Dispose();
            _applicationStoppingRegistration.Dispose();
        }

        #region Private Methods

        private void OnProcessExit(object sender, EventArgs e)
        {
            _applicationLifetime.StopApplication();

            if (!_shutdownBlock.WaitOne(_hostOptions.ShutdownTimeout))
            {
                _logger.LogInformation("Waiting for the host to be disposed. Ensure all 'IHost' instances are wrapped in 'using' blocks.");
            }
            _shutdownBlock.WaitOne();
            
            Environment.ExitCode = _exitCode;
        }

        private void OnCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            _applicationLifetime.StopApplication();
        }

        #endregion
    }
}