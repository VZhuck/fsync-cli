using System;
using System.IO;

namespace FSyncCli.Domain
{
    public class FileMetadataInfo
    {
        public FileMetadataInfo()
        {
        }

        // TODO: Used be tests. Consider to move/remove
        public FileMetadataInfo(string fullPath)
        {
            FullPath = fullPath;
            Name = Path.GetFileName(fullPath);
            Ext = Path.GetExtension(fullPath);
        }

        public string FullPath { get; set; }

        public string Name { get; set; }

        public string Ext { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime CreationTimeUtc { get; set; }

    }
}