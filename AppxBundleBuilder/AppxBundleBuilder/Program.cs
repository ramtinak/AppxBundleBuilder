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
            catch(Exception ex) { }
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
