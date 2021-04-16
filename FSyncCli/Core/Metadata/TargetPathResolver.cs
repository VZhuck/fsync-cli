using System;
using System.IO;
using System.Text;
using FSyncCli.Domain;

namespace FSyncCli.Core.Metadata
{
    public class TargetPathResolver : ITargetPathResolver
    {
        private const string DefaultDateStrFormat = "yyyy.MM.dd";
        private const bool KeepOriginalSubPath = false;
        private const bool KeepOriginalCategoryName = true;

        // Desired: 2021.12.22 - Description
        public string ResolveTargetDirPath(string targetDir, FileMetadataInfo fileInfo,
            FilePathMetadataInfo filePathMetadata, ImageMetaData imgMetaData)
        {
            var fileDateStamp = imgMetaData?.OriginalDateTimeOffset ??
                           imgMetaData?.XmpCreatedDate ??
                           fileInfo.LastWriteTime;

            var catNameStrBuilder = new StringBuilder();

            catNameStrBuilder.Append(filePathMetadata?.From != null
                    ? filePathMetadata.From.ToString()
                    : fileDateStamp.ToString(DefaultDateStrFormat));


            if (filePathMetadata?.To != null)
            {
                catNameStrBuilder.Append("-");
                catNameStrBuilder.Append(filePathMetadata.To.ToString());
            }

            if (!string.IsNullOrWhiteSpace(filePathMetadata?.CategoryName) && KeepOriginalCategoryName)
            {
                catNameStrBuilder.Append(" ");
                catNameStrBuilder.Append(filePathMetadata.CategoryName);
            }

            var resolvedFilePath = Path.Combine(targetDir, catNameStrBuilder.ToString());

            // Build Category Sub path (turned off be default)
            if (!string.IsNullOrWhiteSpace(filePathMetadata?.CategorySubPath) && KeepOriginalSubPath)
            {
                resolvedFilePath = Path.Combine(resolvedFilePath, filePathMetadata.CategorySubPath);
            }

            return resolvedFilePath;
        }

        public string ResolveFileName(string targetDir, FileMetadataInfo fileInfo, ImageMetaData imgMetaData)
        {
            var fileDateStamp = imgMetaData?.OriginalDateTimeOffset ??
                                imgMetaData?.XmpCreatedDate ??
                                fileInfo.LastWriteTime;

            var targetFileName = $"{fileDateStamp.ToString(DefaultDateStrFormat)}-{fileInfo.Name}";
            return targetFileName;
        }
    }
}