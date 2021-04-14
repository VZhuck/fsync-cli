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
                LastWriteTimeUtc = fileInfo.LastWriteTimeUtc,
                LastWriteTime = fileInfo.LastWriteTime
            };

            return fileMetadataInfo;
        }
    }
}