using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace SyncTool
{
    class XML
    {
        public XML()
        {
        }

        public static PBOList ReadXML(string s)
        {
            if(!File.Exists(s))
            {
                GenerateBlankXML(s);
            }

            CheckSyntax(s);

            var doc = XDocument.Load(s);
            var list = from x in doc.Descendants("FILE")
                select new PBO
                (
                    (string)x.Element("NAME"),
                    (string)x.Element("SDIR"),
                    (string)x.Element("HASH")
                );
            PBOList p = new PBOList();
            foreach(PBO x in list)
                p.Add(x);
            return p;
        }

        public static void GenerateBlankXML(string s)
        {
            if (!File.Exists(s))
            {
                Log.Info("generating new repo.xml");
                StreamWriter f = File.CreateText(s);
                f.Close();

                var doc = new XDocument
                (
                    new XElement("SyncTool")
                );

                doc.Save(s);

            }
            else
            {
                //BackupXML(s);
                //GenerateBlankXML(s);
            }
        }

        public static void OutputToXML(string fileName, string filePath, string fileHash, string basePath)
        {
            string fullFilePath = string.Format("{0}\\SyncTool.xml", basePath);

            if (File.Exists(fullFilePath))
            {
                // Nothing
            }
            else
            {
                GenerateBlankXML(fullFilePath);
                OutputToXML(fileName, filePath, fileHash, basePath);
            }

            XDocument xmlFile = XDocument.Load(fullFilePath);

            var xmlElement = (new XElement("FileNode",
                                  new XElement("FileName", fileName),
                                  new XElement("filePath", filePath),
                                  new XElement("fileHash", fileHash)));

            xmlFile.Element("SyncTool").Add(xmlElement);
            xmlFile.Save(fullFilePath);
        }

        public static void CheckSyntax(string s)
        {
            try
            {
                var doc = XDocument.Load(s);
            }
            catch (Exception ex)
            {
                Log.Info("repo.xml appears to be corrupted, backing up and recreating");
                Log.Info(ex.ToString());
                BackupXML(s);
            }
        }

        public static void BackupXML(string s)
        {
            if (File.Exists(s + ".backup"))
            {
                File.Delete(s + ".backup");
                File.Move(s, s + ".backup");
            }
        }
    }
}
