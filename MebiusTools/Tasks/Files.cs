using System.IO;

namespace Tasks
{
    public static class Files
    {
        public enum Results
        {
            Unknown,
            Commited,
            Rejected,
            Builded,
            Building
        }

        private static string PATH_ACCEPT = @"patches\accept\";
        private static string PATH_COMMIT = @"patches\commit\done\";
        private static string PATH_REJECT = @"patches\reject\";
        private static string PATH_SEND = @"patches\send\";


        public static string GetStatusPm(string pm, string etalonPath = "\\\\linux3\\src\\etalon.043\\")
        {
            if (File.Exists(etalonPath + PATH_COMMIT + pm +".tar"))
            {
                return Results.Commited.ToString();
            }
            if (File.Exists(etalonPath + PATH_REJECT + pm + ".tar"))
            {
                return Results.Rejected.ToString();
            }
            if (File.Exists(etalonPath + PATH_ACCEPT + pm + ".tar"))
            {
                return Results.Builded.ToString();
            }
            if (File.Exists(etalonPath + PATH_SEND + pm + ".tar"))
            {
                return Results.Building.ToString();
            }

            return Results.Unknown.ToString();
        }
    }
}