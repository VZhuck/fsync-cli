using System;
using System.IO;

namespace FSyncCli.Domain
{
    public class FileMetadataInfo
    {
        public FileMetadataInfo(string fullPath)
        {
            FullPath = fullPath;
        }

        public string FullPath { get; }

        public string Name => Path.GetFileName(FullPath);

        public string Ext => Path.GetExtension(FullPath);

        public Guid Hash { get; set; }
    }
}