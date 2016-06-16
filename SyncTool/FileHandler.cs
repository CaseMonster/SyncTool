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

        private const string KeyFile = ".fuckboi";
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
                    string fileHash = HashGenerator.GetHash(file);

                    // store data in new pbo
                    list.Add(new PBO(fileName, filePath, fileHash));
                }
                return list;
            }
            catch
            {
                Log.Info(basePath + " folder does not exist");
                return list;
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
            //Delete only if the path has a keyfile and is not a keyfile
            if (File.Exists(pbo.filePath + KeyFile) && !(pbo.filePath + KeyFile).Equals(pbo.filePath + pbo.fileName))
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
