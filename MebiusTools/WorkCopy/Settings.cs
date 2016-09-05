using System;
using System.Collections.Generic;
using System.Configuration;

namespace WorkCopy
{
    [Serializable]
    public class Settings : ApplicationSettingsBase
    {
        [UserScopedSetting]
        [SettingsSerializeAs(SettingsSerializeAs.Xml)]
        [DefaultSettingValue("")]
        public List<SettingsPathes> SettingsPathes
        {
            get { return (List<SettingsPathes>)this["SettingsPathes"]; }
            set { this["SettingsPathes"] = value; }
        }
    }
}
