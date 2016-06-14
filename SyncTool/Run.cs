using System.Diagnostics;

namespace SyncTool
{
    class Run
    {
        public static void Execute(Settings settings)
        {
            Process p = new Process();
            p.StartInfo.FileName = settings.arma3file;
            p.StartInfo.WorkingDirectory = settings.arma3file;
            p.StartInfo.Arguments = settings.arma3args;

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