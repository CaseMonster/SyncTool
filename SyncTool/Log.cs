using System;
using System.IO;
using System.Windows.Forms;

namespace Tool
{
    class Log
    {
        public static string LOGDIR_MAIN = "C:\\IS_Logs\\" + DateTime.Now.ToString("yyyyMMMdd HHmm");
        public static string LOGDIR_VR = LOGDIR_MAIN + "\\" + "Virus Removal";
        public static string LOGDIR_RB = LOGDIR_MAIN + "\\" + "Registry Backup";
        public static string LOGDIR_S  = LOGDIR_MAIN + "\\" + "Settings";
        public static string LOGFILE = LOGDIR_MAIN + "\\" + "Tool_Log.txt";
        static StreamWriter log;

        //init
        public Log()
        {
            //create folders
            if (!Directory.Exists("C:\\IS_Logs"))
                Directory.CreateDirectory("C:\\IS_Logs");

            //make a new folder for the logs
            Directory.CreateDirectory(LOGDIR_MAIN);
            Directory.CreateDirectory(LOGDIR_VR);
            Directory.CreateDirectory(LOGDIR_RB);
            Directory.CreateDirectory(LOGDIR_S);

            //timestamp log
            info("TOOL STARTED");
        }

        public static void info(App a, string s)
        {
            write(linePrep(a, s));
        }

        public static void info(string s)
        {
            write(linePrep(s));
        }

        public static void error(App a, string s)
        {
            write(linePrep(a, s));
            MessageBox.Show(linePrep(a, s));
        }

        public static void error(string s)
        {
            write(linePrep(s));
            MessageBox.Show(linePrep(s));
        }

        static string linePrep(App a, string s)
        {
            return (Environment.NewLine + "[" + DateTime.Now.ToString("yyyyMMMdd HHmm") + "]  "
                + Environment.NewLine + "   " + s
                + Environment.NewLine + "   NAME: " + a.name
                + Environment.NewLine + "   FILE: " + a.file
                + Environment.NewLine + "   ARGS: " + a.args
                + Environment.NewLine + "   SDIR: " + a.sdir
                );
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
