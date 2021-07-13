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


        static string[] FindFolders(string folder, string search)
        {
            return Directory.GetDirectories(folder, search, SearchOption.AllDirectories)
                .ToArray();
        }
    }
}
