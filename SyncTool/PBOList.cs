using System.Collections;

namespace SyncTool
{
    class PBOList : ArrayList
    {
        public string locationOnDisk;

        public void GeneratePBOListFromDir(string dir)
        {
            this.AddRange(FileHandler.FindPBOinDirectory(Program.localSettings.modfolder + "\\" + dir + "\\"));
        }

        public PBOList AddHashesToList()
        {
            FileHandler.HashPBOs(this);
            return this;
        }

        public void RemoveModFolderForServerRepo()
        {
            foreach (PBO pbo in this)
                pbo.filePath = pbo.filePath.Remove(0, Program.localSettings.modfolder.Length);
        }

        public void WriteXMLToDisk(string s)
        {
            this.locationOnDisk = s;
            XML.WritePBOXML(this.locationOnDisk, this);
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
            PBOList diff = new PBOList();
            diff.AddRange(this);

            foreach (PBO thisPBO in this)
                foreach (PBO remotePBO in remote)
                    if ((remotePBO.fileHash == thisPBO.fileHash) && (remotePBO.fileName == thisPBO.fileName))
                        diff = DeleteFromArray(diff, thisPBO);

            return diff;
        }

        //the return list contains a list of files not present in the local repo (Download List)
        public PBOList GetDownloadList(PBOList remote)
        {
            PBOList diff = new PBOList();
            diff.AddRange(remote);

            foreach (PBO remotePBO in remote)
                foreach (PBO thisPBO in this)
                    if ((remotePBO.fileHash == thisPBO.fileHash) && (remotePBO.fileName == thisPBO.fileName))
                        diff = DeleteFromArray(diff, thisPBO);
            return diff;
        }

        public bool HaveFileNamesChanged(PBOList inputList)
        {
            PBOList diff = new PBOList();
            diff.AddRange(this);

            foreach (PBO inputPBO in inputList)
                foreach (PBO thisPBO in this)
                    if (inputPBO.fileName == thisPBO.fileName)
                        diff = DeleteFromArray(diff, thisPBO);

            if(inputList.Count != this.Count)
            {
                return true;
            };
            if (diff.Count > 0)
            {
                return true;
            };
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

        public void AddModPathToList()
        {
            foreach (PBO pbo in this)
                pbo.filePath = Program.localSettings.modfolder + "\\" + pbo.filePath;
        }
    }
}
