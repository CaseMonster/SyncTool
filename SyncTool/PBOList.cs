using System.Collections;

namespace SyncTool
{
    class PBOList : ArrayList
    {
        public static PBOList GeneratePBOListFromDirs(string[] dirs, LocalSettings localSettings)
        {
            PBOList list = new PBOList();
            foreach (string dir in dirs)
                list.AddRange(FileHandler.FindPBOinDirectory(localSettings.modfolder + "\\" + dir + "\\"));
            return list;
        }

        public PBOList AddHashesToList()
        {
            Log.InfoStamp("hashing files");
            foreach (PBO pbo in this)
                FileHandler.HashPBOs(this);
            return this;
        }

        public void WriteXMLToDisk()
        {
            foreach (PBO pbo in this)
                XML.WritePBOXML(Program.LOCAL_REPO, pbo);
        }

        public static PBOList ReadFromDisk(string s)
        {
            return XML.ReadRepoXML(s);
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

        public bool HaveFileNamesChanged(PBOList quickRepo)
        {
            Log.Info("comparing files");

            PBOList diff = new PBOList();
            diff.AddRange(this);

            ArrayList list = new ArrayList();
            
            foreach (PBO quickPBO in quickRepo)
                foreach (PBO thisPBO in this)
                    if (quickPBO.fileName == thisPBO.fileName)
                        list.Add(thisPBO.fileHash);

            foreach (string s in list)
                diff = DeleteFromArray(diff, s);

            if(quickRepo.Count != this.Count)
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
