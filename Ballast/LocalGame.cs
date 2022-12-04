using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;
using System.Diagnostics;
using HtmlAgilityPack;

namespace Ballast
{
    //this is the class for retrieving data from steams local files
    public class LocalGame
    {
        public string appid { get; protected set; } = "unknown";
        public string name { get; protected set; } = "unknown";
        public string steam_directory { get; protected set; } = "unknown";

        //constructor will set all the store item properties
        public LocalGame(string id)
        {
            /*auto retrieving steam directory through checking processes,
            this will only work if Steam is running!*/
            foreach (Process proc in Process.GetProcesses())
            {
                if (proc.ProcessName.ToLower() == "steam")
                {
                    steam_directory = System.IO.Path.GetDirectoryName(proc.MainModule.FileName);
                }
            }

            //retrieving the local name
            try
            {
                foreach (string line in File.ReadLines(steam_directory + $"\\steamapps\\appmanifest_{id}.acf"))
                {
                    if (line.Contains("\"name\""))
                    {
                        name = (line.Replace("\"name\"", "")).Split('"')[1];
                    }
                }
            }
            catch
            {

            }
          
        }

    }
}
