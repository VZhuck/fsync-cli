using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FSyncCli.Domain;
using FSyncCli.Utils;

namespace FSyncCli.Infrastructure
{
    public class LocalFileRepoService : IFileRepoService
    {
        public LocalFileRepoService()
        {
        }

        public IEnumerable<FileMetadataInfo> GetFileMetadataInfos(DirectoryInfo directoryInfo)
        {
            var fileEnumerator = directoryInfo.EnumerateFiles("*", SearchOption.AllDirectories);

            foreach (var fileInfo in fileEnumerator)
            {
                yield return fileInfo.ToFileMetadataInfo();
            }

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

            var fileOptions = FileOptions.Asynchronous | FileOptions.SequentialScan;
            var bufferSize = 4096;

            var fileStream = new FileStream(fileDescriptor.FullPath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, fileOptions);

            return fileStream;
        }

        public async Task CopyFileAsync(string sourceFile, string destinationFile)
        {
            var fileOptions = FileOptions.Asynchronous | FileOptions.SequentialScan;
            var bufferSize = 4096;

            await using var sourceStream =
                new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, fileOptions);
            
            await using var destinationStream =
                new FileStream(destinationFile, FileMode.CreateNew, FileAccess.Write, FileShare.None, bufferSize, fileOptions);

            await sourceStream.CopyToAsync(destinationStream, bufferSize)
                .ConfigureAwait(continueOnCapturedContext: false);
        }
    }
}