using System;
using FSyncCli.Core;
using FSyncCli.Domain;
using FSyncCli.Utils;

namespace FSyncCli.Tests.TestUtils
{
    public class PipelineItemFactory
    {
        public static PipelineItem Create(string fullPath, Guid hash)
        {
            var pipelineItem = new FileMetadataInfo(fullPath).AsPipelineItem();
            pipelineItem.Hash = hash;

            return pipelineItem;
        }
    }
}