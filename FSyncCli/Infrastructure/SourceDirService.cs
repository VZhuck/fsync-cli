using System;
using System.Collections.Generic;
using System.IO;
using FSyncCli.Domain;
using FSyncCli.Utils;

namespace FSyncCli.Infrastructure
{
    public class SourceDirService : ISourceDirService
    {
        public SourceDirService()
        {
        }

        public Stream GetFilesContentAsStream(FileMetadataInfo fileDescriptor)
        {
            return  File.OpenRead(fileDescriptor.FullPath);
        }

        public IEnumerable<FileMetadataInfo> GetSourcesFiles(DirectoryInfo directoryInfo)
        {
            var fileEnumerator = directoryInfo.EnumerateFiles("*", SearchOption.AllDirectories);

            foreach (var fileInfo in fileEnumerator)
            {
                yield return fileInfo.ToFileMetadataInfo();
            }

        }
    }
}