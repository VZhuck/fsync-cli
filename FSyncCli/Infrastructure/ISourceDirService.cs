using System.Collections.Generic;
using System.IO;
using FSyncCli.Domain;

namespace FSyncCli.Infrastructure
{
    public interface ISourceDirService
    {
        FileMetadataInfo GetFileDescriptorWithCalculatedHash(FileMetadataInfo fileDescriptor);

        IEnumerable<FileMetadataInfo> GetSourcesFiles(DirectoryInfo directoryInfo);
    }
}