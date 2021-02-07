using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FSyncCli.Core
{
    public interface IPipelineBuilder
    {
        PipelineBuilder WithSourceDirs(IEnumerable<DirectoryInfo> sourceDirs);

        PipelineBuilder WithTargetDir(DirectoryInfo targetDir);

        IPipelineBuilder CreateDefaultPipeline();

        IFSyncPipeline Build();
    }
}