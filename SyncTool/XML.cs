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

        public static ArrayList ReadXML(string s)
        {
            CheckSyntax(s);

            var doc = XDocument.Load(s);
            var list = from x in doc.Descendants("Device")
                select new PBO
                (
                    (string)x.Element("NAME"),
                    (string)x.Element("SDIR"),
                    (string)x.Element("HASH")
                );
            ArrayList array = new ArrayList();
            foreach(PBO x in list)
                array.Add(x);
            return array;
        }

        public static void GenerateXML(string s)
        {
            if (!File.Exists(s))
            {
                Log.info("generating new repo.xml");
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
                Log.error("repo.xml appears to be corrupted, backing up and recreating");
                Log.info(ex.ToString());
                BackupXML(s);
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
