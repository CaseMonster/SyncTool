using System;
using System.Collections;
using System.IO;
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
        public static string LOCAL_FOLDER = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RollingRepo");
        public static string LOCAL_REPO = Path.Combine(LOCAL_FOLDER, "repo.xml");
        public static string LOCAL_SETTINGS = Path.Combine(LOCAL_FOLDER, "settings.xml");

        public static LocalSettings localSettings = XML.ReadLocalSettingsXML(LOCAL_SETTINGS);
        public static RemoteSettings remoteSettings = XML.ReadRemoteSettingsXML(Path.Combine(localSettings.server, "settings.xml"));

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
            Log.Info("loading config");
            //todo: redo local settings, launch optional first run dialog

            bool argReset = false;
            bool argCLI = false;
            bool argSilent = false;

            if (args.Length > 0)
            {
                if (args[0] == "-server")
                {
                    GenRepo();
                    return;
                }

                if (args[0] == "-reset")
                    argReset = true;

                if (args[0] == "-cli")
                    argCLI = true;

                if (args[0] == "-silent")
                    argCLI = true;
            };

            ArrayList remoteRepoList = new ArrayList();
            ArrayList localRepoList = new ArrayList();
            ArrayList quickRepoList = new ArrayList();

            //Get list of mods from server, pull the XML for each one
            Log.Info("building list of files");
            foreach (string modlist in remoteSettings.modsArray)
            {
                //For remote XMLs
                PBOList tempServerRepo = new PBOList();
                tempServerRepo = tempServerRepo.ReadFromDisk(Path.Combine(localSettings.server, (modlist + ".xml")));
                remoteRepoList.Add(tempServerRepo);

                //For local XMLs
                PBOList tempLocalRepo = new PBOList();
                tempLocalRepo = tempLocalRepo.ReadFromDisk(Path.Combine(LOCAL_FOLDER, (modlist + ".xml")));
                localRepoList.Add(tempLocalRepo);

                //Quick PBO list
                PBOList tempQuickRepo = new PBOList();
                tempQuickRepo.GeneratePBOListFromDir(modlist);
                quickRepoList.Add(tempQuickRepo);
            };

            //Check each quick repo against it's corresponding local repo
            bool haveFileNamesChanged = false;
            ArrayList modsThatChanged = new ArrayList();
            for (int i = 0; i < localRepoList.Count; i++)
            {
                PBOList tempLocalRepo = (PBOList)localRepoList[i];
                PBOList tempQuickRepo = (PBOList)quickRepoList[i];
                if (tempLocalRepo.HaveFileNamesChanged(tempQuickRepo))
                {
                    haveFileNamesChanged = true;
                    modsThatChanged.Add(true);
                }
                else
                {
                    modsThatChanged.Add(false);
                }
            };

            //Run checks, downloads, and deletions if files have changed
            if (haveFileNamesChanged)
            {
                Log.Info("changes detected");

                //Add hashes to each quick repo
                Log.InfoStamp("hashing files stored locally");
                for (int i = 0; i < remoteSettings.modsArray.Length; i++)
                {
                    //Only hash mod folders that have changed
                    if ((bool)modsThatChanged[i])
                    {
                        Log.Info("hashing " + remoteSettings.modsArray[i]);
                        PBOList tempQuickRepo = (PBOList)quickRepoList[i];
                        tempQuickRepo.AddHashesToList();
                        quickRepoList.RemoveAt(i);
                        quickRepoList.Insert(i, tempQuickRepo);
                    };
                };

                //Check each quick repo to each remote repo
                Log.InfoStamp("finding files to delete");
                ArrayList deleteRepoList = new ArrayList();
                for (int i = 0; i < remoteSettings.modsArray.Length; i++)
                {
                    if ((bool)modsThatChanged[i])
                    {
                        PBOList tempQuickRepo = (PBOList)quickRepoList[i];
                        PBOList tempRemoteRepo = (PBOList)remoteRepoList[i];
                        deleteRepoList.Add(tempQuickRepo.GetDeleteList(tempRemoteRepo));
                    };
                };

                //Delete
                Log.Info("deleting extra or corrupt files");
                foreach (PBOList tempDeleteRepo in deleteRepoList)
                        tempDeleteRepo.DeleteFilesOnDisk();
                Log.Info("files deleted");

                //cycle list of pbo downloads
                Log.InfoStamp("downloading files");
                ArrayList downloadRepoList = new ArrayList();
                for (int i = 0; i < remoteSettings.modsArray.Length; i++)
                {
                    if ((bool)modsThatChanged[i])
                    {
                        PBOList tempQuickRepo = (PBOList)quickRepoList[i];
                        PBOList tempRemoteRepo = (PBOList)remoteRepoList[i];
                        downloadRepoList.Add(tempQuickRepo.GetDownloadList(tempRemoteRepo));
                    };
                };

                //Get number of files going to be downloaded
                int tempCount = 0;
                foreach (PBOList tempDownloadRepo in downloadRepoList)
                {
                    tempCount = +tempDownloadRepo.Count;
                };
                Log.Info(tempCount + " files will be downloaded");

                //Download
                foreach (PBOList tempDownloadRepo in downloadRepoList)
                { 
                    Log.Info("downloading " + tempDownloadRepo.ToString());
                    HTTP.DownloadList(tempDownloadRepo);
                };
                Log.Info("files downloaded");

                //add the repo from the server after adding back our modfolder
                Log.InfoStamp("saving XML to disk");
                for (int i = 0; i < remoteSettings.modsArray.Length; i++)
                {
                    Log.Info("saving " + remoteSettings.modsArray[i]);
                    PBOList tempLocalRepo = (PBOList)localRepoList[i];
                    PBOList tempRemoteRepo = (PBOList)remoteRepoList[i];
                    tempLocalRepo.Clear();
                    tempLocalRepo.DeleteXML(Path.Combine(LOCAL_FOLDER, (remoteSettings.modsArray[i] + ".xml")));
                    tempLocalRepo.AddModPathToList();
                    tempLocalRepo.AddRange(tempRemoteRepo);
                    tempLocalRepo.WriteXMLToDisk(Path.Combine(LOCAL_FOLDER, (remoteSettings.modsArray[i] + ".xml")));
                };
            }
            else
            {
                Log.Info("no changes detected");
                Console.ReadKey();
            }

            //Todo: dialog asking to resync or launch the game, times out and exits
            Log.Info("all done");
            Console.ReadKey();
            Run();
        }

        static void GenRepo()
        {
            ArrayList quickRepoList = new ArrayList();

            //Get list of mods from server, pull the XML for each one
            Log.Info("building list of files");
            foreach (string modlist in remoteSettings.modsArray)
            {
                //Quick PBO list
                PBOList tempQuickRepo = new PBOList();
                tempQuickRepo.GeneratePBOListFromDir(modlist);
                quickRepoList.Add(tempQuickRepo);
            };

            //Add hashes to each quick repo
            Log.InfoStamp("hashing files stored locally");
            for (int i = 0; i < remoteSettings.modsArray.Length; i++)
            {
                Log.Info("hashing " + remoteSettings.modsArray[i]);
                PBOList tempQuickRepo = (PBOList)quickRepoList[i];
                tempQuickRepo.AddHashesToList();
                tempQuickRepo.RemoveModFolderForServerRepo();
                quickRepoList.Add(tempQuickRepo);
            };

            Log.InfoStamp("saving XML to disk");
            for (int i = 0; i < remoteSettings.modsArray.Length; i++)
            {
                Log.Info("saving " + remoteSettings.modsArray[i]);
                PBOList tempQuickRepo = (PBOList)quickRepoList[i];
                tempQuickRepo.DeleteXML(Path.Combine(LOCAL_FOLDER, (remoteSettings.modsArray[i] + ".xml")));
                tempQuickRepo.AddModPathToList();
                tempQuickRepo.WriteXMLToDisk(Path.Combine(LOCAL_FOLDER, (remoteSettings.modsArray[i] + ".xml")));
            };
        }

        static void Run()
        { 
            Log.InfoStamp("starting arma");
            SyncTool.Run.Execute();
        }
    }
}
