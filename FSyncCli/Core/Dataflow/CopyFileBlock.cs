using System.IO;
using System.Threading.Tasks.Dataflow;
using FSyncCli.Domain;

namespace FSyncCli.Core.Dataflow
{
    public class CopyFileBlock
    {
        public ITargetBlock<FileMetadataInfo> Block { get; }

        public CopyFileBlock(IPipelineContext context)
        {
            Block = new ActionBlock<FileMetadataInfo>(info =>
            {
                var destFilePath = Path.Combine(context.TargetDir.FullName, info.Name);
                File.Copy(info.FullPath, destFilePath);

                // _logger.LogInformation($"File {info.FullPath} has been copied to: {destFilePath}");
            });

        }
    }
}