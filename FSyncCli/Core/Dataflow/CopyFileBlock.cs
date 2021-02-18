﻿using System;
using System.IO;
using System.Threading.Tasks.Dataflow;
using FSyncCli.Infrastructure;

namespace FSyncCli.Core.Dataflow
{
    public class CopyFileBlock
    {
        public ITargetBlock<PipelineItem> Block { get; }

        public CopyFileBlock(IPipelineContext context, IFileRepoService fileRepo)
        {
            Block = new ActionBlock<PipelineItem>(async pipelineItem =>
            {
                var destFilePath = Path.Combine(context.TargetDir.FullName, pipelineItem.Item.Name);

                if (fileRepo.FileExists(destFilePath))
                {
                    // Log Warning
                    var origExt = pipelineItem.Item.Ext;
                    var newExt = $"{pipelineItem.Hash.ToString("N")[^12..]}{origExt}";

                    destFilePath = Path.ChangeExtension(destFilePath, newExt);
                }

                await using var sourceStream = fileRepo.GetFilesContentAsStream(pipelineItem.Item);
                await fileRepo.CreateNewWithContent(destFilePath, sourceStream);
            });
        }
    }
}
