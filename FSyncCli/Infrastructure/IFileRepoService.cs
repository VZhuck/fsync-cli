using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FSyncCli.Domain;

namespace FSyncCli.Infrastructure
{
    public interface IFileRepoService
    {
        IEnumerable<FileMetadataInfo> GetFileMetadataInfos(DirectoryInfo directoryInfo);

        FileMetadataInfo GetFileMetadata(string fullPath);

        bool FileExists(string fullPath);

        Stream GetFilesContentAsStream(FileMetadataInfo fileDescriptor);

        Task<FileMetadataInfo> CreateNewWithContent(string newFilePath, Stream contentStream);

    }
}