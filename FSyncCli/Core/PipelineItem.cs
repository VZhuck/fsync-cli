using System;
using FSyncCli.Domain;

namespace FSyncCli.Core
{
    public class PipelineItem : PipelineItemBase<FileMetadataInfo>
    {
        public PipelineItem(FileMetadataInfo descriptor): base(descriptor)
        {
        }

        public string BaseDirPath { get; set; }

        public Guid Hash { get; set; }

        public bool IsDuplicate { get; set; }

        public ImageMetaData ImageMetaData
        {
            get => GetItemProperty<ImageMetaData>();
            set => SetItemProperty(value);
        }

        public FilePathMetadataInfo FilePathMetadataInfo
        {
            get => GetItemProperty<FilePathMetadataInfo>();
            set => SetItemProperty(value);
        }
    }
}