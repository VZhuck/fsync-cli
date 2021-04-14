using System.IO;
using System.Text.RegularExpressions;
using FSyncCli.Domain;

namespace FSyncCli.Core.Metadata
{
    public class TargetPathResolver : ITargetPathResolver
    {
        private const string SingleDateTmpl = 
            @"(?<year>\d{4})[-.\/](?<month>\d{2})[-._\/](?<day>\d{2})[\W\s_]*(?<fname>[\w\s]*)[\W\s_]*";

        private static Regex RegEx = new Regex(SingleDateTmpl);

        // Desired: 2021.12.22 - Description
        public string Resolve(FileMetadataInfo fileMetaInfo)
        {
            var fileDirName = Path.GetFileName(Path.GetDirectoryName(fileMetaInfo.FullPath));
            var fileCreationTime = fileMetaInfo.LastWriteTime;

            return $"{fileCreationTime.Year:D4}.{fileCreationTime.Month:D2}.{fileCreationTime.Day:D2} - {fileDirName}";
        }

        public void FileNameHasDates(string filePath)
        {
            //var folderName = Path.GetPathRoot()
        }
    }
}  