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
            Log.Info("reading " + s);

            CheckSyntax(s);

            var doc = XDocument.Load(s);
            var list = from x in doc.Descendants("FileNode")
                select new PBO
                (
                    (string)x.Element("FileName"),
                    (string)x.Element("FilePath"),
                    (string)x.Element("FileHash")
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

        public static void OverWriteSettingsXML(LocalSettings settings, string location)
        {
            var doc = new XDocument
               (
                   new XElement
                   (
                       "SyncTool",
                       new XElement
                       (
                           "Settings",
                           new XElement("ServerAddress", settings.server),
                           new XElement("ModFolder", settings.modfolder),
                           new XElement("Arma3Executable", settings.arma3file),
                           new XElement("LaunchOptions", settings.arma3args)
                       )
                   )
               );
            doc.Save(location);

        }

        public static void GenerateLocalSettingsXML(string s)
        {
            if (!File.Exists(s))
            {
                Log.Info("generating new " + s);
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
                            new XElement("ModFolder", @"C:\Users\User\Documents\Arma 3\Mods\"),
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
            Log.Info("generating new " + s);
            StreamWriter f = File.CreateText(s);
            f.Close();

            var doc = new XDocument
            (
                new XElement("SyncTool")
            );

            doc.Save(s);
        }

        public static void OutputToXML(string fileName, string filePath, string fileHash)
        {
            XDocument xmlFile = XDocument.Load(Program.LOCAL_REPO);
            var xmlElement = (new XElement("FileNode",
                                  new XElement("FileName", fileName),
                                  new XElement("FilePath", filePath),
                                  new XElement("FileHash", fileHash)));

            xmlFile.Element("SyncTool").Add(xmlElement);
            xmlFile.Save(Program.LOCAL_REPO);
        }

        public static void CheckSyntax(string s)
        {
            Log.Info("checking syntax of " + s);
            try
            {
                var doc = XDocument.Load(s);
                Log.Info("syntax of " + s + "is okay");
            }
            catch (Exception ex)
            {
                Log.Info(s + " appears to be corrupted, backing up and recreating");
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
            Log.Info("backing up " + s);

            if (File.Exists(s + ".backup"))
            {
                File.Delete(s + ".backup");
                File.Move(s, s + ".backup");
            }
            else
            {
                if(File.Exists(s))
                    File.Move(s, s + ".backup");
            }
        }
    }
}
