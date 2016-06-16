using System.Collections;

namespace SyncTool
{
    class PBOList : ArrayList
    {
        public static PBOList GeneratePBOListFromDirs(string[] dirs, LocalSettings localSettings)
        {
            Log.InfoStamp("building list of files");
            PBOList list = new PBOList();
            foreach (string dir in dirs)
                list.AddRange(FileHandler.FindPBOinDirectory(localSettings.modfolder + "\\" + dir + "\\"));
            return list;
        }

        public PBOList AddHashesToList()
        {
            Log.InfoStamp("hashing files");
            FileHandler.HashPBOs(this);
            return this;
        }

        public void WriteXMLToDisk(string saveLocation)
        {
            foreach (PBO pbo in this)
                XML.WritePBOXML(saveLocation, pbo);
        }

        public static PBOList ReadFromDisk(string loadLocation)
        {
            return XML.ReadRepoXML(loadLocation);
        }

        public void DeleteFilesOnDisk()
        {
            foreach (PBO p in this)
                FileHandler.DeleteFile(p);
        }

        //the return list contains a list of files not present in the remote repo (Deletion List)
        public PBOList GetDeleteList(PBOList remote)
        {
            Log.InfoStamp("generating delete list");

            PBOList diff = new PBOList();
            diff.AddRange(this);

            ArrayList list = new ArrayList();

            foreach (PBO thisPBO in this)
                foreach (PBO remotePBO in remote)
                    if ((remotePBO.fileHash == thisPBO.fileHash) && (remotePBO.fileName == thisPBO.fileName))
                        list.Add(thisPBO.fileHash);

            foreach (string s in list)
                diff = DeleteFromArray(diff, s);

            Log.Info(diff.Count + " file(s) will be deleted");
            return diff;
        }

        //the return list contains a list of files not present in the local repo (Download List)
        public PBOList GetDownloadList(PBOList remote)
        {
            Log.InfoStamp("generating download list");

            PBOList diff = new PBOList();
            diff.AddRange(remote);

            ArrayList list = new ArrayList();

            foreach (PBO remotePBO in remote)
                foreach (PBO thisPBO in this)
                    if ((remotePBO.fileHash == thisPBO.fileHash) && (remotePBO.fileName == thisPBO.fileName))
                        list.Add(thisPBO.fileHash);
            
            foreach (string s in list)
                diff = DeleteFromArray(diff, s);

            Log.Info(diff.Count + " file(s) will be downloaded");
            return diff;
        }

        public bool HaveFileNamesChanged(PBOList inputList)
        {
            Log.InfoStamp("comparing files");

            PBOList diff = new PBOList();
            diff.AddRange(this);

            ArrayList list = new ArrayList();
            
            foreach (PBO inputPBO in inputList)
                foreach (PBO thisPBO in this)
                    if (inputPBO.fileName == thisPBO.fileName)
                        list.Add(thisPBO.fileHash);

            foreach (string s in list)
                diff = DeleteFromArray(diff, s);

            if(inputList.Count != this.Count)
            {
                Log.Info("files have changed");
                return true;
            };
            if (diff.Count > 0)
            {
                Log.Info("files have changed");
                return true;
            };

            Log.Info("no changes in file detected");
            return false;
        }

        public PBOList DeleteFromArray(PBOList list, string s)
        {
            foreach (PBO p in list)
            {
                if(p.fileHash == s)
                {
                    list.Remove(p);
                    return list;
                };
            }
            return list;
        }

    }
}
