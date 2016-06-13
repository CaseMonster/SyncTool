using System.Diagnostics;

namespace SyncTool
{
    class Run
    {
        public void Execute(string dir, string file, string args)
        {
            Process p = new Process();
            p.StartInfo.FileName = file;
            p.StartInfo.WorkingDirectory = dir;
            p.StartInfo.Arguments = args;

            if (file == "null" || file == null)
            {
                Log.Info("executable not found");
            }
            else
            {
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
            };
        }
    }
}