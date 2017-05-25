using System;

namespace Tasks
{
    public static class Extension
    {
        public static string ToTime(this TimeSpan ts)
        {
            return ts.ToString(@"dd\.hh\:mm\:ss");
        }
    }
}
