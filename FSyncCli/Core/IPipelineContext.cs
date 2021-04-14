using System.Collections.Generic;
using System.IO;

namespace FSyncCli.Core
{
    public interface IPipelineContext
    {
        List<DirectoryInfo> SourceDirs { get; }
        DirectoryInfo TargetDir { get; set; }
        bool DryRun { get; }
    }
}