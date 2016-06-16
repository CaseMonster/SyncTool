using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SyncTool
{
    class Program
    {
        //Disable quick edit mode in console window
        const uint ENABLE_QUICK_EDIT = 0x0040;
        const int STD_INPUT_HANDLE = -10;
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);
        [DllImport("kernel32.dll")]
        static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);
        [DllImport("kernel32.dll")]
        static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        //global vars
        public static string LOCAL_REPO = "repo.xml";
        public static string LOCAL_SETTINGS = "settings.xml";
        public static string QUICK_REPO = "quick.xml";

        static void Main(string[] args)
        {
            //Disable quick edit mode in console window
            IntPtr consoleHandle = GetStdHandle(STD_INPUT_HANDLE);
            uint consoleMode;
            GetConsoleMode(consoleHandle, out consoleMode);
            consoleMode &= ~ENABLE_QUICK_EDIT;
            SetConsoleMode(consoleHandle, consoleMode);

            Log.Startup();

            //load settings
            LocalSettings localSettings = XML.ReadLocalSettingsXML(LOCAL_SETTINGS);
            RemoteSettings remoteSettings = XML.ReadRemoteSettingsXML(localSettings.server + "settings.xml");
            //todo: redo local settings, launch optional first run dialog

            if (args.Length > 0)
            {
                if (args[0] == "-server")
                {
                    PBOList serverRepo = new PBOList();
                    serverRepo.ReadFromDisk("server.xml");
                    serverRepo.GeneratePBOListFromDirs(remoteSettings.modsArray, localSettings);
                    serverRepo.AddHashesToList();
                    serverRepo.WriteXMLToDisk();
                    return;
                }

                if (args[0] == "-reset")
                {
                    XML.BackupXML(LOCAL_REPO);
                    XML.BackupXML(LOCAL_SETTINGS);
                    return;
                }
            };

            if (false)
            {
                Application.Run(new Launcher());
                return;
            }

            //Pull local repo, remote repo, generate quick repo
            PBOList remoteRepo = new PBOList();
                remoteRepo.ReadFromDisk(localSettings.server + "repo.xml");
            PBOList localRepo = new PBOList();
                localRepo.ReadFromDisk(LOCAL_REPO);
            PBOList quickRepo = new PBOList();
                quickRepo.GeneratePBOListFromDirs(remoteSettings.modsArray, localSettings);

            //Comb through directories and hash folders, if nessesary
            if (localRepo.HaveFileNamesChanged(quickRepo))
            {
                localRepo.DeleteXML();
                localRepo.Clear();
                quickRepo.AddHashesToList();
                localRepo.AddRange(quickRepo);
                localRepo.WriteXMLToDisk();

                //DeleteFromDisk PBOs that are no longer in Repo
                PBOList deleteList = localRepo.GetDeleteList(remoteRepo);
                if (deleteList.Count > 0)
                    deleteList.DeleteFilesOnDisk();

                //cycle list of pbo downloads, store in temp location
                PBOList downloadList = localRepo.GetDownloadList(remoteRepo);
                if (downloadList.Count > 0)
                    HTTP.DownloadList(downloadList, localSettings);
            };

            //Todo: dialog asking to resync or launch the game, times out and exits
            //Run A3
            //Run.Execute(localSettings);

            Log.InfoStamp("all done");
            System.Console.ReadKey();
        }
    }
}
