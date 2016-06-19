using System.Diagnostics;

namespace SyncTool
{
    class Run
    {
        public static void Execute()
        {
            Process p = new Process();
            p.StartInfo.FileName = "Arma3.exe";
            p.StartInfo.WorkingDirectory = Program.localSettings.arma3file;

            string modsArg = "";
            foreach (string s in Program.remoteSettings.modsArray)
                modsArg = modsArg + Program.localSettings.modfolder + "\\" + s + ";";
            modsArg = "-mod=\"" + modsArg + "\"";

            p.StartInfo.Arguments = Program.localSettings.arma3args + " " + modsArg + " " + Program.remoteSettings.args;

            try
            {
                p.Start();
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                return;
            };
        }
    }
}