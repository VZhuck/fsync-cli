using System;
using FSyncCli.Domain;

namespace FSyncCli.Core
{
    public class PipelineItem : PipelineItem<FileMetadataInfo>
    {
        public PipelineItem(FileMetadataInfo item) : base(item)
        {
        }

        public Guid Hash { get; set; }

        public bool IsDuplicate { get; set; }
    }


    public class PipelineItem<T> where T : class
    {
        public PipelineItem(T item)
        {
            Item = item;
        }
        public T Item { get; }
    }
}