using System.Windows.Forms;

namespace SyncTool
{
    class Program
    {
        public static string LOCAL_REPO = "repo.xml";
        public static string LOCAL_SETTINGS = "settings.xml";
        public static string QUICK_REPO = "quick.xml";

        static void Main(string[] args)
        {
            Log.Startup();

            //load settings
            LocalSettings localSettings = XML.ReadLocalSettingsXML(LOCAL_SETTINGS);
            RemoteSettings remoteSettings = XML.ReadRemoteSettingsXML(localSettings.server + "settings.xml");

            if (args.Length > 0)
            {
                if (args[0] == "-server")
                {
                    localSettings.modfolder = "\\";
                    //FileHandler.HashFolders(remoteSettings, localSettings);
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
            PBOList remoteRepo = PBOList.ReadFromDisk(localSettings.server + "repo.xml");
            PBOList localRepo  = PBOList.ReadFromDisk(LOCAL_REPO);
            PBOList quickRepo  = PBOList.GeneratePBOListFromDirs(remoteSettings.modsArray, localSettings);

            //Comb through directories and hash folders, if nessesary
            if (localRepo.HaveFileNamesChanged(quickRepo))
            {
                localRepo.Clear();
                localRepo = quickRepo.AddHashesToList();
                localRepo.WriteXMLToDisk();
            }

            //create list of pbos that have changed, hashes that have changed
            PBOList downloadList = localRepo.GetDownloadList(remoteRepo);
            PBOList deleteList = localRepo.GetDeleteList(remoteRepo);

            //DeleteFromDisk PBOs that are no longer in Repo
            if (deleteList.Count > 0)
                localRepo.DeleteFilesOnDisk();

            //cycle list of pbo downloads, store in temp location
            if (downloadList.Count > 0)
                HTTP.DownloadList(downloadList, localSettings);

            //hash downloaded pbos, compare to remote list of pbos

            //replace local pbos from temp location

            //update local xml as replace

            //compare two xml checksums for pbos, again

            //Run A3
            //Run.Execute(localSettings);

            Log.InfoStamp("all done");
            System.Console.ReadKey();
        }
    }
}
