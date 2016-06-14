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

        public static RemoteSettings ReadRemoteSettingsXML(string s)
        {
            var doc = XDocument.Load(s);
            var list = from x in doc.Descendants("ServerSettings")
                       select new RemoteSettings
                       (
                           (string)x.Element("Mods")
                       );
            Log.Info("loaded remote settings");
            RemoteSettings settings = list.First();
            return settings;
        }

        public static LocalSettings ReadLocalSettingsXML(string s)
        {
            CheckSyntax(s);

            var doc = XDocument.Load(s);
            var list = from x in doc.Descendants("Settings")
                       select new LocalSettings
                       (
                           (string)x.Element("ServerAddress"),
                           (string)x.Element("ModFolder"),
                           (string)x.Element("Arma3Directory"),
                           (string)x.Element("LaunchOptions")
                       );
            Log.Info("loaded local settings");
            LocalSettings settings = list.First();
            return settings;
        }

        public static void GenerateLocalSettingsXML(string s)
        {
            if (!File.Exists(s))
            {
                Log.Info("generating new settings.xml");
                StreamWriter f = File.CreateText(s);
                f.Close();

                var doc = new XDocument
                (
                    new XElement
                    (
                        "SyncTool",
                        new XElement
                        (
                            "Settings",
                            new XElement("ServerAddress", "http://rollingkeg.com/repo/"),
                            new XElement("Arma3Executable", @"c:\program files\steam\steamapps\steamapps\arma3\arma3.exe"),
                            new XElement("LaunchOptions", "")
                        )
                    )
                );
                doc.Save(s);

            }
            else
            {
                BackupXML(s);
                GenerateLocalSettingsXML(s);
            }
        }

        public static void GenerateBlankXML(string s)
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

        public static void OutputToXML(string fileName, string filePath, string fileHash, string basePath)
        {
            XDocument xmlFile = XDocument.Load(Program.LOCAL_REPO);
            var xmlElement = (new XElement("FileNode",
                                  new XElement("FileName", fileName),
                                  new XElement("filePath", filePath),
                                  new XElement("fileHash", fileHash)));

            xmlFile.Element("SyncTool").Add(xmlElement);
            xmlFile.Save(Program.LOCAL_REPO);
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

                if(s == "settings.xml")
                    GenerateLocalSettingsXML(s);
                if(s == "repo.xml")
                    GenerateBlankXML(s);
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
