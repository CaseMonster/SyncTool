using System;
using System.IO;

namespace SyncTool
{
    class Log
    {
        public static string LOGFILE = "synctool.Log";
        static StreamWriter log;

        public Log()
        {
            info("SyncTool Started");
        }

        public static void info(string s)
        {
            write(linePrep(s));
        }

        public static void error(string s)
        {
            write(linePrep(s));
        }

        static string linePrep(string s)
        {
            return (Environment.NewLine + "[" + DateTime.Now.ToString("yyyyMMMdd HHmm") + "]  "
                + Environment.NewLine + "   " + s
                );
        }

        static void write(string s)
        {
            log = new StreamWriter(LOGFILE, true);
            log.WriteLine(s);
            log.Close();
        }
    }
}
