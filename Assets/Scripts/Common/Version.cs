//===================================================
//Author      : DRB
//CreateTime  ：9/22/2017 4:45:41 PM
//Description ：游戏版本
//===================================================

using System;

namespace DRB.Common
{
    public class VersionData
    {
        public Version Version;

        public DownloadDataEntity GamePackData;

        public VersionData(string version, DownloadDataEntity gamePackData)
        {
            Version = new Version(version);
            GamePackData = gamePackData;
        }
    }


    [Serializable]
    public struct Version
    {
        public int GameVersion;

        public int UpdateVersion;

        public int ModifiedVersion;

        public Version(string version)
        {
            string[] arr = version.Split('.');
            if (arr.Length != 3)
            {
                GameVersion = 0;
                UpdateVersion = 0;
                ModifiedVersion = 0;
            }
            else
            {
                GameVersion = arr[0].ToInt();
                UpdateVersion = arr[1].ToInt();
                ModifiedVersion = arr[2].ToInt();
            }
        }

        public static bool operator ==(Version v1, Version v2)
        {
            if (Equals(v1, v2)) return true;
            return (v1.GameVersion == v2.GameVersion && v1.UpdateVersion == v2.UpdateVersion && v1.ModifiedVersion == v2.ModifiedVersion);
        }

        public static bool operator !=(Version v1, Version v2)
        {
            if (Equals(v1, v2)) return false;
            return !(v1.GameVersion == v2.GameVersion && v1.UpdateVersion == v2.UpdateVersion && v1.ModifiedVersion == v2.ModifiedVersion);
        }

        public static bool operator >(Version v1, Version v2)
        {
            if (v1.GameVersion > v2.GameVersion) return true;
            else if (v1.GameVersion == v2.GameVersion)
            {
                if (v1.UpdateVersion > v2.UpdateVersion) return true;
                else if (v1.UpdateVersion == v2.UpdateVersion)
                {
                    return v1.ModifiedVersion > v2.ModifiedVersion;
                }
                else return false;
            }
            else return false;
        }

        public static bool operator <(Version v1, Version v2)
        {
            if (v1.GameVersion < v2.GameVersion) return true;
            else if (v1.GameVersion == v2.GameVersion)
            {
                if (v1.UpdateVersion < v2.UpdateVersion) return true;
                else if (v1.UpdateVersion == v2.UpdateVersion)
                {
                    return v1.ModifiedVersion < v2.ModifiedVersion;
                }
                else return false;
            }
            else return false;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0}.{1}.{2}", GameVersion, UpdateVersion, ModifiedVersion);
        }
    }
}
