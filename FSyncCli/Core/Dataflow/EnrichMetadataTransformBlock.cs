using System.IO;
using System.Threading.Tasks.Dataflow;
using FSyncCli.Core.Metadata;
using FSyncCli.Infrastructure;

namespace FSyncCli.Core.Dataflow
{
    public class EnrichMetadataTransformBlock
    {
        private readonly IFileRepoService _sourceFileRepoService;
        private readonly IImageMetadataProviderService _imageMetadataProviderService;
        private readonly IFilePathMetadataExtractService _filePathMetadataExtractService;

        public IPropagatorBlock<PipelineItem, PipelineItem> Block { get; }

        public EnrichMetadataTransformBlock(IFileRepoService sourceFileRepoService, IImageMetadataProviderService imageMetadataProviderService,
            IFilePathMetadataExtractService filePathMetadataExtractService)
        {
            _sourceFileRepoService = sourceFileRepoService;
            _imageMetadataProviderService = imageMetadataProviderService;
            _filePathMetadataExtractService = filePathMetadataExtractService;

            Block = new TransformBlock<PipelineItem, PipelineItem>(EnrichPipeLineWithMetadata);
        }

        private PipelineItem EnrichPipeLineWithMetadata(PipelineItem pipelineItem)
        {
            if (_imageMetadataProviderService.IsSupportedImageFormat(pipelineItem.FileMetadataInfo.Ext))
            {
                using var fileStream = _sourceFileRepoService.GetFilesContentAsStream(pipelineItem.FileMetadataInfo);
                pipelineItem.ImageMetaData = _imageMetadataProviderService.ExtractImageMetadata(fileStream);
                pipelineItem.FilePathMetadataInfo =
                    _filePathMetadataExtractService.ExtractFilePathMetadata(
                        pipelineItem.FileMetadataInfo.FullPath);
            }

            return pipelineItem;
        }
    }
}