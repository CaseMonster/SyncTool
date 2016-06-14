﻿using System;
using System.IO;

namespace SyncTool
{
    class Log
    {
        public static string LOGFILE = "synctool.Log";
        static StreamWriter log;

        public static void Startup()
        {
            Info(Environment.NewLine + "=======================================================================================================================");
            InfoStamp("SyncTool started");
        }

        public static void InfoStamp(string s)
        {
            Write(LinePrep(s));
        }

        public static void Info(string s)
        {
            Write(LinePrepNoStamp(s));
        }

        public static void Error(string s)
        {
            Write(LinePrep(s));
        }

        static string LinePrep(string s)
        {
            return (Environment.NewLine + "[" + DateTime.Now.ToString("yyyyMMMdd HHmm") + "]  " + Environment.NewLine + "   " + s);
        }

        static string LinePrepNoStamp(string s)
        {
            return ("   " + s);
        }

        static void Write(string s)
        {
            log = new StreamWriter(LOGFILE, true);
            log.WriteLine(s);
            log.Close();

            Console.WriteLine(s);
        }
    }
}
