using System.Collections;

namespace SyncTool
{
    class PBOList : ArrayList
    {
        //the return list contains a list of files not present in the remote repo (Deletion List)
        public PBOList DeleteList(PBOList remote)
        {
            PBOList diff = new PBOList();
            diff.AddRange(this);
            Log.InfoStamp("generating delete list");

            foreach (PBO thisPBO in this)
                foreach (PBO remotePBO in remote)
                    if (remotePBO.hash == thisPBO.hash)
                        diff.Remove(thisPBO);

                Log.Info(diff.Count + " file(s) will be deleted");
            return diff;
        }

        //the return list contains a list of files not present in the local repo (Download List)
        public PBOList DownloadList(PBOList remote)
        {
            PBOList diff = new PBOList();
            diff.AddRange(remote);
            Log.InfoStamp("generating download list");

            foreach (PBO remotePBO in remote)
                foreach (PBO thisPBO in this)
                    if (remotePBO.hash == thisPBO.hash)
                        diff.Remove(remotePBO);

                Log.Info(diff.Count + " file(s) will be downloaded");
            return diff;
        }

        public bool HaveFileNamesChanged(PBOList quickRepo)
        {
            PBOList diff = new PBOList();
            diff.AddRange(this);
            Log.Info("comparing files");

            foreach (PBO thisPBO in this)
                foreach (PBO quickPBO in quickRepo)
                    if (quickPBO.name == thisPBO.name)
                        diff.Remove(thisPBO);

            if(diff.Count > 0)
            {
                Log.Info("file names have changed");
                return true;
            };
            Log.Info("no changes in file names detected");
            return false;
        }
    }
}
