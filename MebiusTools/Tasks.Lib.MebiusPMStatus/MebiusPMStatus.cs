using System;
using System.Collections.Generic;
using System.IO;
using Tasks.Lib.Base;

namespace Tasks.Lib.MebiusPMStatus
{
    public class MebiusPmStatus: IMebiusTaskBase
    {
        private enum Results
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
        private static string etalonPath = "\\\\linux3\\src\\etalon.043\\";


        public string Name => GetType().Name;
        public string ErrorResult => Results.Unknown.ToString();
        public string Exec(string param)
        {
            
            if (File.Exists(etalonPath + PATH_COMMIT + param + ".tar"))
            {
                return Results.Commited.ToString();
            }
            if (File.Exists(etalonPath + PATH_REJECT + param + ".tar"))
            {
                return Results.Rejected.ToString();
            }
            if (File.Exists(etalonPath + PATH_ACCEPT + param + ".tar"))
            {
                return Results.Builded.ToString();
            }
            if (File.Exists(etalonPath + PATH_SEND + param + ".tar"))
            {
                return Results.Building.ToString();
            }

            return Results.Unknown.ToString();
        }

        public IEnumerable<string> GetResults => Enum.GetNames(typeof(Results));
    }
}
