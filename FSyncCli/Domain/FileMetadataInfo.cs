using System;
using System.IO;

namespace FSyncCli.Domain
{
    public class FileMetadataInfo
    {
        public FileMetadataInfo()
        {
        }

        // TODO: Used by tests. Consider to move/remove
        public FileMetadataInfo(string fullPath)
        {
            FullPath = fullPath;
            Name = Path.GetFileName(fullPath);
            Ext = Path.GetExtension(fullPath);
        }

        public string FullPath { get; set; }

        public string Name { get; set; }

        public string Ext { get; set; }

        public DateTime LastWriteTime { get; set; }

        public DateTime LastWriteTimeUtc { get; set; }

    }
}