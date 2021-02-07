using System.Collections.Generic;
using System.IO;
using FSyncCli.Domain;

namespace FSyncCli.Infrastructure
{
    public interface ISourceDirService
    {
        Stream GetFilesContentAsStream(FileMetadataInfo fileDescriptor);

        IEnumerable<FileMetadataInfo> GetSourcesFiles(DirectoryInfo directoryInfo);
    }
}