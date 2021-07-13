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
    }
}
