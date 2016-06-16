using System.Diagnostics;

namespace SyncTool
{
    class Run
    {
        public static void Execute(LocalSettings localSettings, RemoteSettings remoteSettings)
        {
            Process p = new Process();
            p.StartInfo.FileName = "Arma 3";
            p.StartInfo.WorkingDirectory = localSettings.arma3file;
            p.StartInfo.Arguments = localSettings.arma3args + " " + remoteSettings.launchMods;

            try
            {
                Log.Info("running executable");
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