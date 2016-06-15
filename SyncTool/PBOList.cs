using System.Collections;

namespace SyncTool
{
    class PBOList : ArrayList
    {
        //the return list contains a list of files not present in the remote repo (Deletion List)
        public PBOList GenerateDeleteList(PBOList remote)
        {
            Log.InfoStamp("generating delete list");

            PBOList diff = new PBOList();
            diff.AddRange(this);

            ArrayList list = new ArrayList();

            foreach (PBO thisPBO in this)
                foreach (PBO remotePBO in remote)
                    if ((remotePBO.hash == thisPBO.hash) && (remotePBO.name == thisPBO.name))
                        list.Add(thisPBO.hash);

            foreach (string s in list)
                diff = DeleteFromArray(diff, s);

            Log.Info(diff.Count + " file(s) will be deleted");
            return diff;
        }

        //the return list contains a list of files not present in the local repo (Download List)
        public PBOList GenerateDownloadList(PBOList remote)
        {
            Log.InfoStamp("generating download list");

            PBOList diff = new PBOList();
            diff.AddRange(remote);

            ArrayList list = new ArrayList();

            foreach (PBO remotePBO in remote)
                foreach (PBO thisPBO in this)
                    if ((remotePBO.hash == thisPBO.hash) && (remotePBO.name == thisPBO.name))
                        list.Add(thisPBO.hash);
            
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
                    if (quickPBO.name == thisPBO.name)
                        list.Add(thisPBO.hash);

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
                if(p.hash == s)
                {
                    list.Remove(p);
                    return list;
                };
            }
            return list;
        }
    }
}
