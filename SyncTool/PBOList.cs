using System;
using System.Collections;
using System.Linq;

namespace SyncTool
{
    class PBOList : ArrayList
    {
        public PBOList ListDiff(PBOList remote)
        {
            PBOList diff = this;

            foreach (PBO r in remote)
            {
                foreach (PBO d in diff)
                {
                    if (r.hash == d.hash)
                    {
                        diff.Remove(d);
                    }
                }
            } 

            return diff;
        }
    }
}
