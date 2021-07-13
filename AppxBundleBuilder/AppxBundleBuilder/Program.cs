/*
 * License: MIT
 * Created by Ramtin Jokar [ Ramtinak@live.com ]
 * 
 * July 13, 2021
 * 
 */

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;
using static System.Console;

namespace AppxBundleBuilder
{
    class Program
    {
        static readonly Stopwatch StopwatchAll = new();

        static void Main()
        {

        }

        static bool RunCommand(string cmd)
        {
            using Process process = new();
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.FileName = @"C:\Windows\System32\cmd.exe";
            process.StartInfo.Arguments = "/C " + cmd;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            StreamReader sr = process.StandardOutput;
            while (!sr.EndOfStream)
            {
                var v = sr.ReadLine();
                WriteLine(v);
            }
            Thread.Sleep(5000);
            return true;
        }

        static bool ChangeDependencyVersion(string path, string dependencyName, string dependencyVersion)
        {
            const string EXTNESION = "*.csproj";
            //<PackageReference Include="Microsoft.Toolkit.Uwp.UI.Controls">
            //  <Version>4.0.0</Version>
            //</PackageReference>

            try
            {
                var files = FindFiles(path, EXTNESION, false);
                var dName = $"<PackageReference Include=\"{dependencyName}\"";
                foreach (var file in files)
                {
                    try
                    {
                        if (File.Exists(file))
                        {
                            var text = File.ReadAllText(file);
                            if (text.IndexOf(dName, StringComparison.OrdinalIgnoreCase) != -1)
                            {
                                var findText = FindAndReplaceValue(text, dName, "</PackageReference>");
                                var findVersion = FindAndReplaceValue(findText, "<Version>", "</Version>", true, false);
                                var replacedText = findText.Replace(findVersion, dependencyVersion);
                                File.WriteAllText(file, text.Replace(findText, replacedText));
                            }
                        }
                    }
                    catch (Exception) 
                    { 
                        // ignore
                    }
                }
            }
            catch (Exception ex) { PrintException(ex, "ChangeDependencyVersion"); }

            return false;
        }

        static bool ChangeMinimumSdkVersion(string path, string newVersion)
        {
            const string EXTENSION = "*.csproj";
            const string START = "<TargetPlatformMinVersion>";
            const string END = "</TargetPlatformMinVersion>";
            // <TargetPlatformMinVersion>10.0.15063.0</TargetPlatformMinVersion>

            try
            {
                var files = FindFiles(path, EXTENSION, false);
                foreach (var file in files)
                {
                    try
                    {
                        if (File.Exists(file))
                        {
                            var text = File.ReadAllText(file);
                            if (text.IndexOf(START, StringComparison.OrdinalIgnoreCase) != -1)
                            {
                                var findText = FindAndReplaceValue(text, START, END, true, false);
                                File.WriteAllText(file, text.Replace(findText, newVersion));
                            }
                        }
                    }
                    catch (Exception) 
                    {
                        // ignore
                    }
                }
            }
            catch (Exception ex) { PrintException(ex, "ChangeMinimumSdkVersion"); }

            return false;
        }

        static bool ChangeAppVersion(string path, string newVersion)
        {
            const string FILE = "Package.appxmanifest";
            //  <Identity
            //      Name="BLUH BLUH BLUH"
            //      Publisher="CN=BLUH-BLUH-BLUH-BLUH-BLUH"
            //      Version="1.0.18.0" />

            try
            {
                var file = FindFile(path, FILE);
                if (!string.IsNullOrEmpty(file) && File.Exists(file))
                {
                    var text = File.ReadAllText(file);
                    var findText = FindAndReplaceValue(text, "<Identity", "/>");
                    var findVersion = FindAndReplaceValue(findText, "Version=\"", "\"", true, false);
                    var replacedText = findText.Replace(findVersion, newVersion);
                    File.WriteAllText(file, text.Replace(findText, replacedText));
                    return true;
                }
            }
            catch (Exception ex) { PrintException(ex, "ChangeAppVersion"); }

            return false;
        }

        static bool DeleteObjBinFolders(string path)
        {
            var bins = FindFolders(path, "bin");
            var objs = FindFolders(path, "obj");
            foreach (var folder in bins)
            {
                DeleteIfExists(folder);
            }

            foreach (var folder in objs)
            {
                DeleteIfExists(folder);
            }

            static void DeleteIfExists(string folderPath)
            {
                if (Directory.Exists(folderPath))
                {
                    Directory.Delete(folderPath, true);
                }
            }
            return false;
        }

        static string FindAndReplaceValue(string source,
            string start,
            string end,
            bool appendLengthToStart = false,
            bool appendLengthToEnd = true)
        {
            string s = source.Substring(source.IndexOf(start) + (appendLengthToStart ? start.Length : 0));
            s = s.Substring(0, s.IndexOf(end) + (appendLengthToEnd ? end.Length : 0));
            return s;
        }

        static string FindFile(string folder, string exactName)
        {
            string[] files = FindFiles(folder, exactName);

            return files?.Length > 0 ? files[0] : null;
        }

        static string[] FindFiles(string folder, string exactName, bool checkIf = true)
        {
            return Directory.GetFiles(folder, exactName, SearchOption.AllDirectories)
                .Where(x => Path.GetFileName(x).Equals(exactName, StringComparison.OrdinalIgnoreCase) || !checkIf)
                .ToArray();
        }

        static string[] FindFolders(string folder, string search)
        {
            return Directory.GetDirectories(folder, search, SearchOption.AllDirectories)
                .ToArray();
        }

        static void PrintException(Exception ex, string title = "")
        {
            Debug.WriteLine($"Exception thrown for {title} at {DateTime.Now}");
            Debug.WriteLine($"Message: {ex.Message}");
            Debug.WriteLine($"Source: {ex.Source}");
            Debug.WriteLine($"Stacktrace: {ex.StackTrace}");
        }
    }
}
