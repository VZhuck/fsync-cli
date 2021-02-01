using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FSyncCli.Core
{
    public interface IFSyncPipeline
    {
        Task RunFSyncForInAndOutSources(IEnumerable<DirectoryInfo> sourceDirs, DirectoryInfo targetRootDir);
    }
}