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

        public static void GenerateXML(string s)
        {
            if (File.Exists(s))
            {
                Log.Info("generating new repo.xml");
                StreamWriter f = File.CreateText(s);
                f.Close();

                var doc = new XDocument
                (
                    new XElement("SyncTool")
                );

            }
            else
            {
                BackupXML(s);
                GenerateXML(s);
            }
        }

        public static void CheckSyntax(string s)
        {
            try
            {
                var doc = XDocument.Load(s);
            }
            catch (Exception ex)
            {
                Log.Info("repo.xml appears to be corrupted");
                Log.Info(ex.ToString());
                BackupXML(s);
                GenerateXML(s);
            }
        }

        public static void BackupXML(string s)
        {
            if (File.Exists(s + ".backup"))
                File.Delete(s + ".backup");
            File.Move(s, s + ".backup");
        }
    }
}
