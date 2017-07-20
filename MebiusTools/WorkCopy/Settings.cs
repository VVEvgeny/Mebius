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

        [UserScopedSetting]
        [SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValue("")]
        public string MergeAppPath
        {
            get { return (string)this["MergeAppPath"]; }
            set { this["MergeAppPath"] = value; }
        }
        [UserScopedSetting]
        [SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValue("")]
        public CompareTypes CompareType
        {
            get { return (CompareTypes)this["CompareType"]; }
            set { this["CompareType"] = value; }
        }
        [UserScopedSetting]
        [SettingsSerializeAs(SettingsSerializeAs.Binary)]
        [DefaultSettingValue("")]
        public bool OnlyExistingFilesCompare
        {
            get { return (bool)this["OnlyExistingFilesCompare"]; }
            set { this["OnlyExistingFilesCompare"] = value; }
        }

        public enum CompareTypes
        {
            None,
            Size,
            Date,
            Crc
        }

        public const string HomeSelector = "H";
        public const string BaseSelector = "B";
        public static bool IsHomeSelector(string homeOrBaseText)
        {
            return homeOrBaseText == HomeSelector;
        }
    }
}
