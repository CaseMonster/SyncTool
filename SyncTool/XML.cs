using System;
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
            IsSyntaxCorrect(s);

            var doc = XDocument.Load(s);
            var list = from x in doc.Descendants("ServerSettings")
                select new RemoteSettings
                (
                    (string)x.Element("Mods"),
                    (bool)x.Element("ForceHash"),
                    (string)x.Element("Args")
                );
            RemoteSettings settings = list.First();
            return settings;
        }

        public static LocalSettings ReadLocalSettingsXML(string s)
        {
            IsSyntaxCorrect(s);

            var doc = XDocument.Load(s, LoadOptions.PreserveWhitespace);
            var set = doc.Element("SyncTool").Element("Settings");
            LocalSettings settings = new LocalSettings
            (
                set.Element("ServerAddress").Value,
                set.Element("ModFolder").Value,
                set.Element("Arma3Folder").Value,
                set.Element("LaunchOptions").Value
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
                           new XElement("Arma3Folder", settings.arma3file),
                           new XElement("LaunchOptions", settings.arma3args)
                       )
                   )
               );
            doc.Save(location);

        }

        public static void GenerateLocalSettingsXML(string s)
        {
            if (!Directory.Exists(Path.GetDirectoryName(s)))
                Directory.CreateDirectory(Path.GetDirectoryName(s));
            if (!File.Exists(s))
            {
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
                            new XElement("Arma3Folder", Reg.GetArmaRegValue()),
                            new XElement("LaunchOptions", "-world=empty -nosplash")
                        )
                    )
                );
                doc.Save(s);

            }
            else
            {
                BackupXML(s);
                GenerateLocalSettingsXML(s);
            };
        }

        public static void GenerateBlankXML(string s)
        {
            //StreamWriter f = File.CreateText(s);
            //f.Close();

            var doc = new XDocument
            (
                new XElement("SyncTool")
            );

            doc.Save(s);
        }

        public static void WritePBOXML(string xmlName, PBOList list)
        {
            XDocument xmlFile = XDocument.Load(xmlName);
            foreach (PBO pbo in list)
            { 
                var xmlElement = (new XElement("FileNode",
                new XElement("FileName", pbo.fileName),
                new XElement("FilePath", pbo.filePath),
                new XElement("FileHash", pbo.fileHash)));
                xmlFile.Element("SyncTool").Add(xmlElement);
            };

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
                if (s.Contains("http"))
                    return false;
                if(s == Program.LOCAL_SETTINGS)
                {
                    GenerateLocalSettingsXML(s);
                }
                else
                {
                    GenerateBlankXML(s);
                }

            return false;
            }
        }

        public static void BackupXML(string s)
        {
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
