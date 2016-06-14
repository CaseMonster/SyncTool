using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyncTool
{
    class FileHandler
    {
        public static void GenerateLocalRepo(string basePath)
        {
            // Recurse through the directory
            try
            {
                string[] files = Directory.GetFiles(basePath, "*.*", SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    string filePath = file.TrimEnd(fileName.ToCharArray());
                    // for each file get the hash
                    string fileHash = HashGenerator.GetHash(file);

                    // store data in the XML file
                    XML.OutputToXML(Program.LOCAL_REPO, fileName, filePath, fileHash);
                }
            }
            catch
            {
                Log.Info(basePath + " folder does not exist");
            };
        }

        public static void GenerateLocalRepoNoHash(string basePath)
        {
            // Recurse through the directory
            try
            {
                string[] files = Directory.GetFiles(basePath, "*.*", SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    string filePath = file.TrimEnd(fileName.ToCharArray());
                    // for each file get the hash
                    string fileHash = "";

                    // store data in the XML file
                    XML.OutputToXML(Program.QUICK_REPO, fileName, filePath, fileHash);
                }
            }
            catch
            {
                Log.Info(basePath + " folder does not exist");
            };
        }

        public static void HashFolders(RemoteSettings remoteSettings, LocalSettings localSettings)
        {
            Log.InfoStamp("generating new " + Program.LOCAL_REPO);
            foreach (string mod in remoteSettings.modsArray)
            {
                Log.Info("creating hashes for " + mod);
                GenerateLocalRepo(string.Format("{0}\\{1}", localSettings.modfolder, mod));
            }
        }

        public static void ListFolders(RemoteSettings remoteSettings, LocalSettings localSettings)
        {
            Log.InfoStamp("generating new " + Program.QUICK_REPO);
            foreach (string mod in remoteSettings.modsArray)
            {
                GenerateLocalRepoNoHash(string.Format("{0}\\{1}", localSettings.modfolder, mod));
            }
        }

        public static void DeleteList(PBOList deleteList)
        {
            Log.InfoStamp("deleting file(s)");
            foreach (PBO pbo in deleteList)
                if (File.Exists(pbo.sdir))
                    //File.Delete(pbo.sdir);
                    Log.Info("deleting " + pbo.name);
            Log.Info("file(s) deleted");
        }
    }
}
