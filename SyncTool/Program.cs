﻿using System;
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

        static void Main(string[] args)
        {
            //Disable quick edit mode in console window
            IntPtr consoleHandle = GetStdHandle(STD_INPUT_HANDLE);
            uint consoleMode;
            GetConsoleMode(consoleHandle, out consoleMode);
            consoleMode &= ~ENABLE_QUICK_EDIT;
            SetConsoleMode(consoleHandle, consoleMode);
            LocalSettings localSettings;
            RemoteSettings remoteSettings;
            Log.Startup();

            //load settings

            //todo: redo local settings, launch optional first run dialog
            if (true)
            {
                Application.Run(new Launcher());
                Log.InfoStamp("all done");
                return;
            }
            else {
                localSettings = XML.ReadLocalSettingsXML(LOCAL_SETTINGS);
                remoteSettings = XML.ReadRemoteSettingsXML(localSettings.server + "settings.xml");
            }
            if (args.Length > 0)
            {

                if (args[0] == "-server")
                {
                    PBOList serverRepo = new PBOList();
                    serverRepo.ReadFromDisk("server.xml");
                    serverRepo.GeneratePBOListFromDirs(remoteSettings.modsArray, localSettings);
                    serverRepo.AddHashesToList();
                    serverRepo.WriteXMLToDisk("server.xml");
                    return;
                }

                if (args[0] == "-reset")
                {
                    XML.BackupXML(LOCAL_REPO);
                    XML.BackupXML(LOCAL_SETTINGS);
                    return;
                }
            };


            //Add Run Function here
            //Pull local repo, remote repo, generate quick repo
            PBOList remoteRepo = new PBOList();
            remoteRepo = remoteRepo.ReadFromDisk(localSettings.server + "//" + "repo.xml");
            PBOList localRepo = new PBOList();
            localRepo = localRepo.ReadFromDisk(LOCAL_REPO);
            PBOList quickRepo = new PBOList();
            quickRepo.GeneratePBOListFromDirs(remoteSettings.modsArray, localSettings);
            //Comb through directories and hash folders, if nessesary
            if (localRepo.HaveFileNamesChanged(quickRepo))
            {
                quickRepo.AddHashesToList();

                //DeleteFromDisk PBOs that are no longer in Repo
                PBOList deleteList = quickRepo.GetDeleteList(remoteRepo);
                if (deleteList.Count > 0)
                    deleteList.DeleteFilesOnDisk();

                //cycle list of pbo downloads, store in temp location
                PBOList downloadList = quickRepo.GetDownloadList(remoteRepo);
                if (downloadList.Count > 0)
                    HTTP.DownloadList(downloadList, localSettings);

                //add the repo from the server after adding back our modfolder
                localRepo.Clear();
                localRepo.DeleteXML(LOCAL_REPO);
                remoteRepo.AddModPathToList(localSettings);
                localRepo.AddRange(remoteRepo);
                localRepo.WriteXMLToDisk(LOCAL_REPO);

            }
        }
    }
}
