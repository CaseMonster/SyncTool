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

        public static PBOList ReadRepoXML(string s)
        {
            Log.Info("reading " + s);
            IsSyntaxCorrect(s);

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
            Log.Info("reading " + s);
            IsSyntaxCorrect(s);

            var doc = XDocument.Load(s);
            var list = from x in doc.Descendants("ServerSettings")
                select new RemoteSettings
                (
                    (string)x.Element("Mods"),
                    (string)x.Element("Version"),
                    (string)x.Element("ForceHash")

                );
            RemoteSettings settings = list.First();
            return settings;
        }

        public static LocalSettings ReadLocalSettingsXML(string s)
        {
            Log.Info("reading " + s);
            IsSyntaxCorrect(s);

            var doc = XDocument.Load(s, LoadOptions.PreserveWhitespace);
            var set = doc.Element("SyncTool").Element("Settings");
            LocalSettings settings = new LocalSettings
            (
                set.Element("ServerAddress").Value,
                set.Element("ModFolder").Value,
                set.Element("Arma3Executable").Value,
                set.Element("LaunchOptions").Value,
                set.Element("LaunchedOnce").Value
            );

            return settings;
        }

        public static void OverWriteLocalSettingsXML(LocalSettings settings, string location)
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
                           new XElement("LaunchOptions", settings.arma3args),
                           new XElement("LaunchedOnce", settings.launched_once)
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
                            new XElement("Arma3Executable", Reg.GetArmaRegValue()),
                            new XElement("LaunchOptions", ""),
                            new XElement("LaunchedOnce", "False")
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

        public static void WritePBOXML(string xmlName, PBO pbo)
        {
            XDocument xmlFile = XDocument.Load(xmlName);
            var xmlElement = (new XElement("FileNode",
                new XElement("FileName", pbo.fileName),
                new XElement("FilePath", pbo.filePath),
                new XElement("FileHash", pbo.fileHash)));

            xmlFile.Element("SyncTool").Add(xmlElement);
            xmlFile.Save(xmlName);
        }

        public static bool IsSyntaxCorrect(string s)
        {
            try
            {
                var doc = XDocument.Load(s);
                return true;
            }
            catch (Exception ex)
            {
                Log.Info("the XML is missing or corrupted, recreating");
                //Log.Info(ex.ToString());

                if (s == Program.LOCAL_SETTINGS)
                    GenerateLocalSettingsXML(s);
                if (s == Program.LOCAL_REPO)
                    GenerateBlankXML(s);
                if (s == Program.QUICK_REPO)
                    GenerateBlankXML(s);
                if (s == "server.xml")
                    GenerateBlankXML(s);

                return false;
            }
        }

        public static void BackupXML(string s)
        {
            Log.Info("deleting/backing up " + s);

            if (File.Exists(s + ".backup"))
            {
                File.Delete(s + ".backup");
                if(File.Exists(s))
                    File.Move(s, s + ".backup");
            }
            else
            {
                if(File.Exists(s))
                    File.Move(s, s + ".backup");
            }

            IsSyntaxCorrect(s);
        }
    }
}
