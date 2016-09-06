namespace ExtensionMethods
{
    public static class StringExtension
    {
        public static string ToUnixPath(this string strIn)
        {
            return strIn.Replace(@"s:\np", ".").Replace(@"\", "/");
        }
    }
}
