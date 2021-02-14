using System.IO;
using FSyncCli.Domain;

namespace FSyncCli.Utils
{
    public static class FileInfoExtension
    {
        public static FileMetadataInfo ToFileMetadataInfo(this FileInfo fileInfo)
        {
            var fileMetadataInfo = new FileMetadataInfo
            {
                FullPath = fileInfo.FullName,
                Name = fileInfo.Name,
                Ext = fileInfo.Extension,
                CreationTimeUtc = fileInfo.CreationTimeUtc,
                CreationTime = fileInfo.CreationTime
            };

            return fileMetadataInfo;
        }
    }
}