using System;

namespace ExtensionMethods
{
    public static class StringExtension
    {
        public static string ToUnixPath(this string strIn)
        {
            var index = strIn.IndexOf(@"homec", StringComparison.Ordinal);
            if (index == -1) index = strIn.IndexOf(@"basec", StringComparison.Ordinal);
            if (index == -1) index = 14; //s:\np\2016.04\basec
            index -= 9;
            if (index == -3) index = 5;
            return strIn.Replace(strIn.Substring(0, index), ".").Replace(@"\", "/");
            //return strIn.Replace(@"s:\np", ".").Replace(@"\", "/");
        }
    }
}
