using System.Xml;
using System.Collections.Generic;

namespace SyncTool
{
    class XML
    {
        public LinkedList<pbo> Read(string s)
        {
            //Open XML Doc
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(s);
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                System.Console.WriteLine("Problem reading from XML - " + ex.ToString());
            }

            //Init
            XmlNodeList name = xmlDoc.GetElementsByTagName("name");
            XmlNodeList sdir = xmlDoc.GetElementsByTagName("sdir");
            XmlNodeList hash = xmlDoc.GetElementsByTagName("hash");
            LinkedList<pbo> list = new LinkedList<pbo>();

            for (int i = 0; i < name.Count; i++)
            {
                pbo p = new pbo();

                try
                {
                    //copy values to array
                    if (name[i].InnerText != null)
                    {
                        p.name = name[i].InnerText;
                    }
                    if (sdir[i].InnerText != null)
                    {
                        p.sdir = sdir[i].InnerText;
                    }
                    if (hash[i].InnerText != null)
                    {
                        p.hash = hash[i].InnerText;
                    }

                    //push to list
                    list.AddLast(p);
                }
                catch (System.ComponentModel.Win32Exception ex)
                {
                    System.Console.WriteLine("Problem reading from XML - " + ex.ToString());
                };
            };
            return list;
        }
    }
}
