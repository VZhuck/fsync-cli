using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FSyncCli.Core
{
    public interface IFSyncPipeline
    {
        Task StartPipeline();
    }
}