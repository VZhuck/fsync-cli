namespace FSyncCli.Utils
{
    public static class StringExtensions
    {
        public static string IfWhiteSpaceThenToNull(this string strValue)
        {
            return string.IsNullOrWhiteSpace(strValue)
                ? null
                : strValue;
        }
    }
}