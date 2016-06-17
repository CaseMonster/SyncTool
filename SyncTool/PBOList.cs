using System.Collections;

namespace SyncTool
{
    class PBOList : ArrayList
    {
        public string locationOnDisk;

        public void GeneratePBOListFromDirs(string[] dirs, LocalSettings localSettings)
        {
            Log.InfoStamp("building list of files");
            foreach (string dir in dirs)
                this.AddRange(FileHandler.FindPBOinDirectory(localSettings.modfolder + "\\" + dir + "\\"));
        }

        public PBOList AddHashesToList()
        {
            Log.InfoStamp("hashing files");
            FileHandler.HashPBOs(this);
            return this;
        }

        public void WriteXMLToDisk(string s)
        {
            this.locationOnDisk = s;
            foreach (PBO pbo in this)
                XML.WritePBOXML(this.locationOnDisk, pbo);
        }

        public PBOList ReadFromDisk(string s)
        {
            this.locationOnDisk = s;
            return XML.ReadRepoXML(this.locationOnDisk);
        }

        public void DeleteFilesOnDisk()
        {
            foreach (PBO p in this)
                FileHandler.DeleteFile(p);
        }

        public void DeleteXML(string s)
        {
            XML.BackupXML(s);
        }

        //the return list contains a list of files not present in the remote repo (Deletion List)
        public PBOList GetDeleteList(PBOList remote)
        {
            Log.InfoStamp("generating delete list");

            PBOList diff = new PBOList();
            diff.AddRange(this);

            foreach (PBO thisPBO in this)
                foreach (PBO remotePBO in remote)
                    if ((remotePBO.fileHash == thisPBO.fileHash) && (remotePBO.fileName == thisPBO.fileName))
                        diff = DeleteFromArray(diff, thisPBO);

            Log.Info(diff.Count + " file(s) will be deleted");
            return diff;
        }

        //the return list contains a list of files not present in the local repo (Download List)
        public PBOList GetDownloadList(PBOList remote)
        {
            Log.InfoStamp("generating download list");

            PBOList diff = new PBOList();
            diff.AddRange(remote);

            foreach (PBO remotePBO in remote)
                foreach (PBO thisPBO in this)
                    if ((remotePBO.fileHash == thisPBO.fileHash) && (remotePBO.fileName == thisPBO.fileName))
                        diff = DeleteFromArray(diff, thisPBO);

            Log.Info(diff.Count + " file(s) will be downloaded");
            return diff;
        }

        public bool HaveFileNamesChanged(PBOList inputList)
        {
            Log.Info("comparing files");

            PBOList diff = new PBOList();
            diff.AddRange(this);

            foreach (PBO inputPBO in inputList)
                foreach (PBO thisPBO in this)
                    if (inputPBO.fileName == thisPBO.fileName)
                        diff = DeleteFromArray(diff, thisPBO);

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

        public PBOList DeleteFromArray(PBOList list, PBO pbo)
        {
            foreach (PBO p in list)
            {
                if((p.fileHash == pbo.fileHash) && (p.fileName == pbo.fileName))
                {
                    list.Remove(p);
                    return list;
                };
            }
            return list;
        }

        public void AddModPathToList(LocalSettings localSettings)
        {
            foreach (PBO pbo in this)
                pbo.filePath = localSettings.modfolder + "\\" + pbo.filePath;
        }
    }
}
