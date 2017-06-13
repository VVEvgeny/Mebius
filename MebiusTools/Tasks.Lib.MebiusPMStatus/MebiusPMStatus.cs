using System;
using System.Collections.Generic;
using System.IO;
using Tasks.Lib.Base;

namespace Tasks.Lib.MebiusPMStatus
{
    [Serializable]
    public class MebiusPmStatusSettings
    {
        public string PathBuilded = @"patches\accept\";
        public string PathCommit = @"patches\commit\done\";
        public string PathReject = @"patches\reject\";
        public string PathBuilding = @"patches\send\";
        public string EtalonPath = "\\\\linux3\\src\\etalon.$$$\\";
    }
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

        public string Name => GetType().Name;
        public string ErrorResult => Results.Unknown.ToString();

        private string GetPathFile(string etalonPath, string dirPath, string pm)
        {
            return etalonPath.Replace("$$$", pm.Substring(0, 3)) + dirPath + pm + ".tar";
        }

        public string Exec(string pm, string _settings)
        {
            //if (settings == null) throw new ArgumentNullException(nameof(_settings));
            //var settings = JsonConvert.DeserializeObject<MebiusPmStatusSettings>(_settings);
            //if (settings == null) throw new InvalidCastException("settings is not MebiusPmStatusSettings=" + _settings);
            var settings = new MebiusPmStatusSettings();

            if (File.Exists(GetPathFile(settings.EtalonPath, settings.PathCommit, pm))) return Results.Commited.ToString();
            if (File.Exists(GetPathFile(settings.EtalonPath, settings.PathReject, pm))) return Results.Rejected.ToString();
            if (File.Exists(GetPathFile(settings.EtalonPath, settings.PathBuilded, pm))) return Results.Builded.ToString();
            if (File.Exists(GetPathFile(settings.EtalonPath, settings.PathBuilding, pm))) return Results.Building.ToString();

            return Results.Unknown.ToString();
        }

        public IEnumerable<string> GetResults => Enum.GetNames(typeof(Results));
        public bool HaveSettings => false;
        public string GetSettings()
        {
            throw new NotImplementedException();
        }
    }
}
