using UnityEngine;

namespace PigeonCoopToolkit.Generic
{
    [System.Serializable]
    public class VersionInformation
    {
        public string Name;
        public int Major = 1;
        public int Minor = 0;
        public int Build = 1146;

        public VersionInformation(string name, int major, int minor, int build)
        {
            Name = name;
            Major = major;
            Minor = minor;
            Build = build;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}.{2} build {3}", Name, Major, Minor, Build);
        }

        public bool Match(VersionInformation other)
        {
            return other.Name == Name &&
                   other.Major == Major &&
                   other.Minor == Minor &&
                   other.Build == Build;
        }

    }
}
