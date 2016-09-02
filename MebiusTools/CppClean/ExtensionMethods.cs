using System;
using System.Collections.Generic;
using System.Linq;

namespace ExtensionMethods
{
    public static class StringExtension
    {
        public static bool Contains(this string str, IEnumerable<string> list)
        {
            return list.Any(str.Contains);
        }
    }

    public static class TimeSpanExtension
    {
        public static string ToTime(this TimeSpan ts)
        {
            return @"Time: " + (ts.Hours == 0 ? "" : ts.Hours + ":") +
                   (ts.Minutes == 0 && ts.Hours == 0
                       ? ""
                       : (ts.Minutes > 10 || ts.Hours == 0 ? "" : "0") + ts.Minutes + ":") +
                   (ts.Seconds == 0 && ts.Minutes == 0 && ts.Hours == 0
                       ? ""
                       : (ts.Seconds > 10 || ts.Minutes == 0 ? "" : "0") + ts.Seconds + ":") + ts.Milliseconds;
        }
    }
}
