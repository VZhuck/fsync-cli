using System.IO;
using FSyncCli.Domain;

namespace FSyncCli.Utils
{
    public static class FileInfoExtension
    {
        public static FileMetadataInfo ToFileMetadataInfo(this FileInfo fileInfo)
        {
            return new FileMetadataInfo(fileInfo.FullName);
        }
    }
}