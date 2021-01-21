using System.Threading;
using System.Threading.Tasks;

namespace FSyncCli
{
    public interface IFSyncCmdApp
    {
        Task<int> RunAsync(CancellationToken cancellationToken);
    }
}