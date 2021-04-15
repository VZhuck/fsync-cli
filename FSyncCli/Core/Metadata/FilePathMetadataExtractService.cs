using System;
using System.IO;
using System.Text.RegularExpressions;
using FSyncCli.Domain;
using FSyncCli.Utils;

namespace FSyncCli.Core.Metadata
{
    public class FilePathMetadataExtractService : IFilePathMetadataExtractService
    {
        private const char CurrentFolderPath = '.';
        private const string DefaultMetadataRegExStr =
            @"(?<yearFrom>\d{4})([._\/](?<monthFrom>\d{2}))?([._\/](?<dayFrom>\d{2}))?([\W-_]+(?<yearTo>\d{4})([.\/](?<monthTo>\d{2}))?([._\/](?<dayTo>\d{2}))?)?[\W\s_]*(?<category>[\w\s]*)[\W\s_]*[\/\\]?(?<subpath>[\w\s\/\\]*)";

        private readonly Regex _dateRegex = new Regex(DefaultMetadataRegExStr);

        public FilePathMetadataExtractService()
        {
        }

        public FilePathMetadataInfo ExtractFilePathMetadata(string filePath, string baseDirPath = null)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("File must be represented by a valid path", nameof(filePath));
            }

            var ignoredBasePath = !string.IsNullOrWhiteSpace(baseDirPath)
                ? baseDirPath
                : Path.GetPathRoot(filePath) ?? CurrentFolderPath.ToString();

            var fileDirName = Path.GetDirectoryName(filePath) ?? CurrentFolderPath.ToString();

            var relativeDirPath = Path
                .GetRelativePath(ignoredBasePath, fileDirName)
                .Trim(CurrentFolderPath);

            if (string.IsNullOrWhiteSpace(relativeDirPath))
            {
                return null;
            }

            return TryGetDateMarkedDirMetadata(relativeDirPath, out var metadata)
                ? metadata
                : GetDefaultMetadata(relativeDirPath);
        }

        #region Private

        private FilePathMetadataInfo GetDefaultMetadata(string relativeDirPath)
        {
            var relativePathAssignments =
                relativeDirPath?.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            if (relativePathAssignments == null || relativePathAssignments?.Length <= 0)
            {
                return null;
            }

            return new FilePathMetadataInfo
            {
                CategoryName = relativePathAssignments[0],
                CategorySubPath = Path
                    .Combine(relativePathAssignments[1..])
                    .IfWhiteSpaceThenToNull()
            };
        }

        private bool TryGetDateMarkedDirMetadata(string relativeDirPath, out FilePathMetadataInfo metaData)
        {
            Match result = _dateRegex.Match(relativeDirPath);

            if (result.Success)
            {
                var fromDate = GetImpreciseDateFromRegExMatch(
                    result, "yearFrom", "monthFrom", "dayFrom");
                var toDate = GetImpreciseDateFromRegExMatch(
                    result, "yearTo", "monthTo", "dayTo");

                var fileCategoryName = result.Groups["category"].Value;
                var subPath = result.Groups["subpath"].Value;

                metaData = new FilePathMetadataInfo
                {
                    From = fromDate,
                    To = toDate,
                    CategoryName = !string.IsNullOrWhiteSpace(fileCategoryName) ? fileCategoryName : null,
                    CategorySubPath = !string.IsNullOrWhiteSpace(subPath) ? subPath : null
                };
                return true;
            }

            metaData = null;
            return false;
        }

        private ImpreciseDate? GetImpreciseDateFromRegExMatch(Match regexMatch, string yearKey, string monthKey, string dayKey)
        {
            var isYearParsed = int.TryParse(regexMatch.Groups[yearKey].Value, out var year);
            var isMonthParsed = int.TryParse(regexMatch.Groups[monthKey].Value, out var month);
            var isDayParsed = int.TryParse(regexMatch.Groups[dayKey].Value, out var day);

            if (!isYearParsed) return null;

            var impreciseDate = new ImpreciseDate(
                year,
                isMonthParsed ? month : (int?)null,
                isDayParsed ? day : (int?)null);

            return impreciseDate;
        }

        #endregion
    }
}