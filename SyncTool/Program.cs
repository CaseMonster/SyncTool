﻿using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
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
        public static string LOCAL_SETTINGS = Path.Combine(LOCAL_FOLDER, "settings.xml");

        //global settings
        public static LocalSettings localSettings = XML.ReadLocalSettingsXML(LOCAL_SETTINGS);
        public static RemoteSettings remoteSettings = XML.ReadRemoteSettingsXML(Path.Combine(localSettings.server, "settings.xml"));

        [STAThread]
        static void Main(string[] args)
        {
            Log.Startup();
            //Disable quick edit mode in console window
            IntPtr consoleHandle = GetStdHandle(STD_INPUT_HANDLE);
            uint consoleMode;
            GetConsoleMode(consoleHandle, out consoleMode);
            consoleMode &= ~ENABLE_QUICK_EDIT;
            SetConsoleMode(consoleHandle, consoleMode);

            //check to see if user is running as admin
            Log.Info("checking to see if running as admin");
            var wi = WindowsIdentity.GetCurrent();
            var wp = new WindowsPrincipal(wi);
            bool runAsAdmin = wp.IsInRole(WindowsBuiltInRole.Administrator);
            if (!runAsAdmin)
            {
                // It is not possible to launch a ClickOnce app as administrator directly,
                // so instead we launch the app as administrator in a new process.
                var processInfo = new ProcessStartInfo(Assembly.GetExecutingAssembly().CodeBase);
                processInfo.UseShellExecute = true;
                processInfo.Verb = "runas";
                if (args.Length > 0)
                {
                    string processArgs = "";
                    foreach (string s in args)
                    processArgs = processArgs + s;
                    processInfo.Arguments = processArgs;
                };

                try
                {
                    Process.Start(processInfo);
                }
                catch (Exception ex)
                {
                    Log.Info("not running as administrator, exiting");
                    Console.ReadKey();
                }
                Application.Exit();
                return;
            };

            //load settings
            Log.Info("loading config");

            bool argCLI = false;

            if (args.Length > 0)
            {
                if (args[0].Contains("-server"))
                {
                    GenRepo();
                    return;
                }

                if (args[0].Contains("-reset"))
                {
                    Log.Info("reseting everything");
                    Reset();
                    return;
                };

                if (args[0].Contains("-cli"))
                    //not done yet

                    if (args[0].Contains("-silent"))
                    {
                        Log.Info("running silent");
                        Sync(false);
                        return;
                    };

                if (args[0].Contains("-force"))
                {
                    Log.Info("forcing hash");
                    Sync(true);
                    Console.ReadKey();
                    Run();
                    return;
                };
            }
            else
            {
                //If sync comes back true(no changes) then it won't run again, if changes were detected
                while (!Sync(false))
                    Sync(true);
                Application.Run(new TempUI());
            };
        }

        public static bool Sync(bool forceSync)
        {
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

            //Check the server repo for mods, if the list is empty, something is wrong
            if (remoteRepoList.Count == 0)
            {
                Log.Info("something is wrong with the server files, the server may be down temporarily");
                return true;
            };

            //Check the local mod directory, if it's blank throw error, recreate settings
            if (localSettings.modfolder == "")
            {
                Log.Info("the mod folder appears to be blank, removing");
                PBOList temp = new PBOList();
                temp.DeleteXML(LOCAL_SETTINGS);
                return false;
            };

            //Check each quick repo against it's corresponding local repo
            bool haveFilesChanged = false;
            ArrayList modsThatChanged = new ArrayList();
            //Check quick vs local(xml) repo
            for (int i = 0; i < localRepoList.Count; i++)
            {
                PBOList tempLocalRepo = (PBOList)localRepoList[i];
                PBOList tempQuickRepo = (PBOList)quickRepoList[i];
                if (tempLocalRepo.HaveFileNamesChanged(tempQuickRepo) || forceSync)
                {
                    haveFilesChanged = true;
                    modsThatChanged.Add(true);
                }
                else
                {
                    modsThatChanged.Add(false);
                }
            };
            //Check quick vs remote repo
            for (int i = 0; i < localRepoList.Count; i++)
            {
                PBOList tempRemoteRepo = (PBOList)remoteRepoList[i];
                PBOList tempQuickRepo = (PBOList)quickRepoList[i];
                if (tempRemoteRepo.HaveFileNamesChanged(tempQuickRepo) || forceSync)
                {
                    haveFilesChanged = true;
                    modsThatChanged[i] = true; //modifies the value from the first check
                };
            };

            //Run checks, downloads, and deletions if files have changed
            if (haveFilesChanged || forceSync || remoteSettings.forceHash)
            {
                Log.Info("changes detected or checking has been forced");

                //Add hashes to each quick repo
                Log.InfoStamp("hashing files stored locally");
                for (int i = 0; i < remoteSettings.modsArray.Length; i++)
                {
                    //Only hash mod folders that have changed
                    if ((bool)modsThatChanged[i])
                    {
                        Log.Info(remoteSettings.modsArray[i]);
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
                    if ((bool)modsThatChanged[i])
                    {
                        PBOList tempQuickRepo = (PBOList)quickRepoList[i];
                        PBOList tempRemoteRepo = (PBOList)remoteRepoList[i];
                        deleteRepoList.Add(tempQuickRepo.GetDeleteList(tempRemoteRepo));
                    };

                //Get number of files going to be downloaded
                int tempCountDelete = 0;
                foreach (PBOList tempDeleteRepo in deleteRepoList)
                    tempCountDelete = tempCountDelete + tempDeleteRepo.Count;

                if (tempCountDelete > 0)
                {
                    Log.Info(tempCountDelete + " files will be deleted");

                    //Delete
                    foreach (PBOList tempDeleteRepo in deleteRepoList)
                        tempDeleteRepo.DeleteFilesOnDisk();
                    Log.Info("files deleted");

                    //Check for empty folders to delete
                    Log.Info("deleting any empty folders");
                    foreach (string dir in remoteSettings.modsArray)
                        if (Directory.Exists(Path.Combine(localSettings.modfolder, dir)))
                            FileHandler.DeleteEmptyFolders(Path.Combine(localSettings.modfolder, dir));
                }
                else
                {
                    Log.Info("no files to delete");
                };

                //cycle list of pbo downloads
                Log.InfoStamp("finding files to download");
                ArrayList downloadRepoList = new ArrayList();
                for (int i = 0; i < remoteSettings.modsArray.Length; i++)
                    if ((bool)modsThatChanged[i])
                    {
                        PBOList tempQuickRepo = (PBOList)quickRepoList[i];
                        PBOList tempRemoteRepo = (PBOList)remoteRepoList[i];
                        downloadRepoList.Add(tempQuickRepo.GetDownloadList(tempRemoteRepo));
                    };

                //Get number of files going to be downloaded
                int tempCountDownload = 0;
                foreach (PBOList tempDownloadRepo in downloadRepoList)
                    tempCountDownload = +tempDownloadRepo.Count;

                if (tempCountDownload > 0)
                {
                    Log.Info(tempCountDownload + " files will be downloaded");

                    //Download
                    foreach (PBOList tempDownloadRepo in downloadRepoList)
                    {
                        HTTP.DownloadList(tempDownloadRepo);
                    };
                    Log.Info("files downloaded");
                }
                else
                {
                    Log.Info("no files to download");
                };

                //save to xml, add the repo from the server after adding back our modfolder
                Log.InfoStamp("saving xml");
                for (int i = 0; i < remoteSettings.modsArray.Length; i++)
                {
                    Log.Info(remoteSettings.modsArray[i]);
                    PBOList tempLocalRepo = (PBOList)localRepoList[i];
                    PBOList tempRemoteRepo = (PBOList)remoteRepoList[i];
                    tempLocalRepo.Clear();
                    tempLocalRepo.DeleteXML(Path.Combine(LOCAL_FOLDER, (remoteSettings.modsArray[i] + ".xml")));
                    tempLocalRepo.AddModPathToList();
                    tempLocalRepo.AddRange(tempRemoteRepo);
                    tempLocalRepo.WriteXMLToDisk(Path.Combine(LOCAL_FOLDER, (remoteSettings.modsArray[i] + ".xml")));
                };

                //Mods changed
                Log.InfoStamp("checking files for consistency");
                return false;
            }
            else
            {
                //Mods did not change
                Log.Info("all done, no changes detected");
                return true;
            };
        }

        public static void Run()
        { 
            Log.InfoStamp("starting arma");
            SyncTool.Run.Execute();
        }

        public static void GenRepo()
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
                Log.Info(remoteSettings.modsArray[i]);
                PBOList tempQuickRepo = (PBOList)quickRepoList[i];
                tempQuickRepo.AddHashesToList();
                tempQuickRepo.RemoveModFolderForServerRepo();
                quickRepoList.Add(tempQuickRepo);
            };

            Log.InfoStamp("saving xml");
            for (int i = 0; i < remoteSettings.modsArray.Length; i++)
            {
                Log.Info(remoteSettings.modsArray[i]);
                PBOList tempQuickRepo = (PBOList)quickRepoList[i];
                tempQuickRepo.DeleteXML(Path.Combine(LOCAL_FOLDER, (remoteSettings.modsArray[i] + ".xml")));
                tempQuickRepo.AddModPathToList();
                tempQuickRepo.WriteXMLToDisk(Path.Combine(LOCAL_FOLDER, (remoteSettings.modsArray[i] + ".xml")));
            };
        }

        public static void Reset()
        {
            Log.Info("deleting xml");
            PBOList tempPBO = new PBOList();
            for (int i = 0; i < remoteSettings.modsArray.Length; i++)
            {
                tempPBO.DeleteXML(Path.Combine(LOCAL_FOLDER, (remoteSettings.modsArray[i] + ".xml")));
            };
            tempPBO.DeleteXML(LOCAL_SETTINGS);
            Log.Info("all xmls removed");
        }
    }
}
