using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
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

        public static void GenerateLocalSettingsXML(string path, string modFolder)
        {
            var doc = new XDocument
            (
                new XElement
                (
                    "SyncTool",
                    new XElement
                    (
                        "Settings",
                        new XElement("ServerAddress", "http://rollingkeg.com/repo/"),
                        new XElement("ModFolder", modFolder),
                        new XElement("Arma3Folder", Reg.GetArmaRegValue()),
                        new XElement("LaunchOptions", "-world=empty -nosplash")
                    )
                )
            );
            doc.Save(path);
            Program.localSettings = new LocalSettings("http://rollingkeg.com/repo/", modFolder, Reg.GetArmaRegValue(), "-world=empty -nosplash");
            //Program.localSettings = XML.ReadLocalSettingsXML(Program.LOCAL_SETTINGS);
        }

        public static void GenerateBlankXML(string s)
        {
            //StreamWriter f = File.CreateText(s);
            //f.Close();

            if (!Directory.Exists(Path.GetDirectoryName(s)))
                Directory.CreateDirectory(Path.GetDirectoryName(s));

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
                    return false; //this should never happen, server xml is busted
                if(s == Program.LOCAL_SETTINGS)
                {
                    MessageBox.Show("This is a temportary window, the gui is coming later.\nSelect the folder you want your mods to be saved in.\nThis will be the folder that holds all of your @mod folders.\nIf you have any problems you can delete your appdata\\roaming\\rollingrepo\\settings.xml to make this box pop up again.");

                    FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                    folderBrowserDialog.ShowDialog();

                    GenerateBlankXML(s);
                    GenerateLocalSettingsXML(s, (string)folderBrowserDialog.SelectedPath);
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

            //recreate xml
            IsSyntaxCorrect(s);
        }
    }
}
