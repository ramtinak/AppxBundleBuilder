/*
 * License: MIT
 * Created by Ramtin Jokar [ Ramtinak@live.com ]
 * 
 * July 13, 2021
 * 
 */

using System.Collections.Generic;

namespace AppxBundleBuilder
{
    public class ConfigurationSettings
    {
        public string SolutionDir { get; set; }
        public string ProjectDir { get; set; }
        public bool RestoreNugetPackages { get; set; }
        public bool GenerateAppxUpload { get; set; }
        public bool DeleteBinObjFolders { get; set; }
        public List<Platform> Platforms { get; set; } = new List<Platform>();
    }

    public class Platform
    {
        public string AppxBundlePlatforms { get; set; }
        public string MinimumSdkVersion { get; set; }
        public string AppVersion { get; set; }
        public List<Dependency> Dependencies { get; set; } = new List<Dependency>();
    }

    public class Dependency
    {
        public string Name { get; set; }
        public string Version { get; set; }
    }
}
