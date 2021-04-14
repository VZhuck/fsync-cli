using System;
using FSyncCli.Domain;

namespace FSyncCli.Core
{
    public class PipelineItem : PipelineItemBase
    {
        public PipelineItem(FileMetadataInfo fileMetadataInfo)
        {
            FileMetadataInfo = fileMetadataInfo;
        }

        public Guid Hash { get; set; }

        public bool IsDuplicate { get; set; }

        public FileMetadataInfo FileMetadataInfo
        {
            get => GetItemValue<FileMetadataInfo>();
            private set => SetItemValue(value);
        }

        public ImageMetaData ImageMetaData
        {
            get => GetItemValue<ImageMetaData>();
            set => SetItemValue(value);
        }

        public FilePathMetadataInfo FilePathMetadataInfo
        {
            get => GetItemValue<FilePathMetadataInfo>();
            set => SetItemValue(value);
        }
    }
}