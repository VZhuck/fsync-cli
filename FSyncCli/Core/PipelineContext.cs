using System;
using System.Collections.Generic;
using System.IO;

namespace FSyncCli.Core
{
    public class PipelineContext : IPipelineContext
    {
        private DirectoryInfo _targetDir;

        public List<DirectoryInfo> SourceDirs { get; } = new List<DirectoryInfo>();

        public DirectoryInfo TargetDir
        {
            get => _targetDir;
            set
            {
                if (_targetDir != null)
                {
                    throw new InvalidOperationException($"{nameof(TargetDir)} can be set only once.");
                }
                _targetDir = value;
            }
        }

        public bool DryRun => false;

    }
}