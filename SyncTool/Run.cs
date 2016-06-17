using System.Diagnostics;

namespace SyncTool
{
    class Run
    {
        public static void Execute(LocalSettings localSettings, RemoteSettings remoteSettings)
        {
            Process p = new Process();
            p.StartInfo.FileName = "Arma3.exe";
            p.StartInfo.WorkingDirectory = localSettings.arma3file;

            string modsArg = "";
            foreach (string s in remoteSettings.modsArray)
                modsArg = modsArg + localSettings.modfolder + "\\" + s + ";";
            modsArg = "-mod=\"" + modsArg + "\"";

            p.StartInfo.Arguments = localSettings.arma3args + " " + modsArg;

            try
            {
                p.Start();
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                Log.Error("could not run program");
                Log.Info(ex.ToString());
                return;
            };
        }
    }
}