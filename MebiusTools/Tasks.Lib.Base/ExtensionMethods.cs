using System;
using System.Collections.Generic;
using System.Linq;
using static System.Reflection.MethodBase;
using static BMTools.BmDebug;

namespace Tasks.Lib.Base
{
    public static class TasksExtensions
    {
        public static string ToTime(this TimeSpan ts)
        {
            return ts.ToString(@"dd\.hh\:mm\:ss");
        }
        public static IMebiusTaskBase Get(this IEnumerable<IMebiusTaskBase> iMebiusTaskBases, string name)
        {
            foreach (var t in iMebiusTaskBases)
            {
                if (name == t.Name) return t;
            }

            Debug.InfoAsync(GetCurrentMethod(), "invalid task name=" + name);
            throw new Exception("invalid task name=" + name);
        }
        public static string RemoveSplitUppers(this string s, string splitter = "")
        {
            if (s == null) return null;
            var ss = string.Empty;
            for (var i = 0; i < s.Length; i++) 
            {
                if (i != 0 && i != s.Length - 1 //skip first & last
                     && s[i] == ' ' && char.IsUpper(s[i + 1])
                    )
                {
                    //skip
                }
                else
                {
                    ss += s[i];
                }
            }
            return ss;
        }
        public static string SplitUppers(this string s, string splitter = " ")
        {
            if (s == null) return null;
            var ss = string.Empty;
            for (var i = 0; i < s.Length; i++)
            {
                ss += (i > 0 && char.IsUpper(s[i]) ? splitter : string.Empty) + s[i];
            }
            return ss;
        }

        public static IEnumerable<string> SplitUppers(this IEnumerable<string> e, string splitter = " ")
        {
            return e?.Select(v => v.SplitUppers(splitter));
        }
    }
}
