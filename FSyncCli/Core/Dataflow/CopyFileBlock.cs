using System;
using System.IO;
using System.Threading.Tasks.Dataflow;
using FSyncCli.Core.Metadata;
using FSyncCli.Infrastructure;

namespace FSyncCli.Core.Dataflow
{
    public class CopyFileBlock
    {
        public ITargetBlock<PipelineItem> Block { get; }

        public CopyFileBlock(IPipelineContext context, IFileRepoService fileRepo, ITargetPathResolver targetPathResolver)
        {
            Block = new ActionBlock<PipelineItem>(async pipelineItem =>
            {
                var desDirPath = targetPathResolver.ResolveTargetDirPath(
                    context.TargetDir.FullName, pipelineItem.Descriptor,
                    pipelineItem.FilePathMetadataInfo, pipelineItem.ImageMetaData);

                if (!Directory.Exists(desDirPath))
                {
                    Directory.CreateDirectory(desDirPath);
                }

                var desFilePath = Path.Combine(desDirPath, targetPathResolver.ResolveFileName(
                    context.TargetDir.FullName, pipelineItem.Descriptor, pipelineItem.ImageMetaData));

                if (fileRepo.FileExists(desFilePath))
                {
                    // Log Warning & auto rename the file
                    var origExt = pipelineItem.Descriptor.Ext;
                    var newExt = $"{pipelineItem.Hash.ToString("N")[^12..]}{origExt}";

                    desFilePath = Path.ChangeExtension(desFilePath, newExt);
                }

                await using var sourceStream = fileRepo.GetFilesContentAsStream(pipelineItem.Descriptor);
                await fileRepo.CreateNewWithContent(desFilePath, sourceStream);
            });
        }
    }
}
