using System;
using System.Diagnostics.Eventing.Reader;
using System.IO;

namespace FSyncCli.Domain
{
    public class FileMetadataInfo
    {
        public FileMetadataInfo()
        {
        }

        public FileMetadataInfo(string fullPath)
        {
            FullPath = fullPath;
        }

        public string FullPath { get; set; }

        public string Name { get; set; }

        public string Ext { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime CreationTimeUtc { get; set; }

        public Guid Hash { get; set; }

        //TODO: Data Flow Model Candidate
        public bool IsDuplicate { get; set; }
    }
}