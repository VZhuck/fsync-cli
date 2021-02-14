using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks.Dataflow;
using FSyncCli.Domain;
using FSyncCli.Infrastructure;

namespace FSyncCli.Core.Dataflow
{
    public class CalculateFileHashTransformBlock
    {
        private readonly IFileRepoService _sourceFileRepoService;

        public IPropagatorBlock<FileMetadataInfo, FileMetadataInfo> Block { get; }

        public CalculateFileHashTransformBlock(IFileRepoService sourceFileRepoService)
        {
            _sourceFileRepoService = sourceFileRepoService;

            Block = new TransformBlock<FileMetadataInfo, FileMetadataInfo>(CalculateAndTransform);
        }

        private FileMetadataInfo CalculateAndTransform(FileMetadataInfo fileDescriptor)
        {
            using var fileStream = _sourceFileRepoService.GetFilesContentAsStream(fileDescriptor);
            fileDescriptor.Hash = CalculateFileHash(fileStream);

            return fileDescriptor;
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