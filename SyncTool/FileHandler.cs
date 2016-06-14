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

            string[] files = Directory.GetFiles(basePath, "*.*", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                string filePath = file.TrimEnd(fileName.ToCharArray());
                // for each file get the hash
                string fileHash = HashGenerator.GetHash(file);

                // store data in the XML file
                XML.OutputToXML(fileName, filePath, fileHash);
            }
        }

    }
}
