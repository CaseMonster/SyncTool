﻿using System.IO;

namespace SyncTool
{
    class FileHandler
    {
        public static PBOList FindPBOinDirectory(string basePath)
        {
            // Recurse through the directory
            PBOList list = new PBOList();
            try
            {
                string[] files = Directory.GetFiles(basePath, "*.*", SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    string filePath = file.TrimEnd(fileName.ToCharArray());
                    // for each file get the hash
                    //string fileHash = HashGenerator.GetHash(file);
                    string fileHash = "";

                    // store data in new pbo
                    list.Add(new PBO(fileName, filePath, fileHash));
                }
                return list;
            }
            catch
            {
                return list;
            };
        }

        public static void DeleteEmptyFolders(string dir)
        {
            foreach (var directory in Directory.GetDirectories(dir))
            {
                DeleteEmptyFolders(directory);
                if (Directory.GetFiles(directory).Length == 0 && Directory.GetDirectories(directory).Length == 0)
                    Directory.Delete(directory, false);
            };
        }

        public static PBOList HashPBOs(PBOList list)
        {
            foreach (PBO pbo in list)
            {
                pbo.fileHash = HashGenerator.GetHash(pbo.filePath + pbo.fileName);
            }
            return list;
        }

        public static void DeleteFile(PBO pbo)
        {
            Log.Info(pbo.fileName);
            if (File.Exists(pbo.filePath + pbo.fileName))
                File.Delete(pbo.filePath + pbo.fileName);

        }

        public static void CreateFolder(string path)
        {
            Directory.CreateDirectory(path);
        }

        public static void CheckFolder(string file)
        {
            string fileName = Path.GetFileName(file);
            string filePath = file.TrimEnd(fileName.ToCharArray());

            CreateFolder(filePath);
        }
    }
}
