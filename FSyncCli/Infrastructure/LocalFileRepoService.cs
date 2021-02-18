using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FSyncCli.Domain;
using FSyncCli.Utils;

namespace FSyncCli.Infrastructure
{
    public class LocalFileRepoService : IFileRepoService
    {
        private const int DefaultBufferSize = 4096;

        public IEnumerable<FileMetadataInfo> GetFileMetadataInfos(DirectoryInfo directoryInfo)
        {
            var fileMetadataInfos = directoryInfo
                .EnumerateFiles("*", SearchOption.AllDirectories)
                .Select(fileInfo => fileInfo.ToFileMetadataInfo());

            return fileMetadataInfos;
        }

        public FileMetadataInfo GetFileMetadata(string fullPath)
        {
            if (!File.Exists(fullPath))
            {
                return null;
            }

            return new FileInfo(fullPath).ToFileMetadataInfo();
        }

        public bool FileExists(string fullPath)
        {
            return File.Exists(fullPath);
        }

        public Stream GetFilesContentAsStream(FileMetadataInfo fileDescriptor)
        {
            // Do not use File.OpenRead(fileDescriptor.FullPath); as it still perform blocking IO operations

            const FileOptions fileOptions = FileOptions.Asynchronous | FileOptions.SequentialScan;
            
            var fileStream = new FileStream(
                fileDescriptor.FullPath, FileMode.Open, FileAccess.Read, FileShare.Read, DefaultBufferSize, fileOptions);

            return fileStream;
        }

        public async Task<FileMetadataInfo> CreateNewWithContent(string newFilePath, Stream contentStream)
        {
            var fileOptions = FileOptions.Asynchronous | FileOptions.SequentialScan;

            await using var destinationStream = new FileStream(
                newFilePath, FileMode.CreateNew, FileAccess.Write, FileShare.None, DefaultBufferSize, fileOptions);

            await contentStream.CopyToAsync(destinationStream, DefaultBufferSize);

            var newFileInfo = new FileInfo(newFilePath);
            return newFileInfo.ToFileMetadataInfo();
        }
    }
}