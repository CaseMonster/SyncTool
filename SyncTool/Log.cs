using System;
using System.IO;

namespace SyncTool
{
    class Log
    {
        public static string LOGFILE = "synctool.Log";
        static StreamWriter log;

        public static void Startup()
        {
            Info("SyncTool started");
        }

        public static void Info(string s)
        {
            Write(LinePrep(s));
        }

        public static void InfoNoStamp(string s)
        {
            Write(LinePrepNoStamp(s));
        }

        public static void Error(string s)
        {
            Write(LinePrep(s));
        }

        static string LinePrep(string s)
        {
            return (Environment.NewLine + "[" + DateTime.Now.ToString("yyyyMMMdd HHmm") + "]  "
                + Environment.NewLine + "   " + s
                );
        }

        static string LinePrepNoStamp(string s)
        {
            return (Environment.NewLine + "   " + s);
        }

        static void Write(string s)
        {
            log = new StreamWriter(LOGFILE, true);
            log.WriteLine(s);
            log.Close();
        }
    }
}
