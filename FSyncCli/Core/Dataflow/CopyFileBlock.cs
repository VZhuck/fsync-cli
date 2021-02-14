using System;
using System.IO;
using System.Threading.Tasks.Dataflow;
using FSyncCli.Domain;
using FSyncCli.Infrastructure;
using Microsoft.Extensions.Logging;

namespace FSyncCli.Core.Dataflow
{
    public class CopyFileBlock
    {
        public ITargetBlock<FileMetadataInfo> Block { get; }

        public CopyFileBlock(IPipelineContext context, IFileRepoService fileRepo)
        {
            Block = new ActionBlock<FileMetadataInfo>(fileDescriptor =>
            {
                var destFilePath = Path.Combine(context.TargetDir.FullName, fileDescriptor.Name);

                if (fileRepo.FileExists(destFilePath))
                {
                    // Log Warning
                    var origExt = fileDescriptor.Ext;
                    var newExt = $"{fileDescriptor.Hash.ToString("N")[^12..]}{origExt}";

                    destFilePath = Path.ChangeExtension(destFilePath, newExt);
                }

                // TODO: Error Handling
                fileRepo.CopyFileAsync(fileDescriptor.FullPath, destFilePath);
            });
        }
    }
}
