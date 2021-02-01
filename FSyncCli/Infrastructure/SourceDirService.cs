using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using FSyncCli.Domain;
using FSyncCli.Utils;

namespace FSyncCli.Infrastructure
{
    public class SourceDirService : ISourceDirService
    {
        public SourceDirService()
        {
        }

        public FileMetadataInfo GetFileDescriptorWithCalculatedHash(FileMetadataInfo fileDescriptor)
        {
            using Stream stream = File.OpenRead(fileDescriptor.FullPath);
            fileDescriptor.Hash = CalculateFileHash(stream);

            return fileDescriptor;
        }

        public IEnumerable<FileMetadataInfo> GetSourcesFiles(DirectoryInfo directoryInfo)
        {
            var fileEnumerator = directoryInfo.EnumerateFiles("*", SearchOption.AllDirectories);

            foreach (var fileInfo in fileEnumerator)
            {
                yield return fileInfo.ToFileMetadataInfo();
            }

        }

        private Guid CalculateFileHash(Stream stream)
        {
            using var md5 = MD5.Create();
            var md5FileHash = md5.ComputeHash(stream);

            var hasString = BitConverter
                .ToString(md5FileHash)
                .Replace("-", string.Empty);

            return new Guid(hasString);
        }
    }
}