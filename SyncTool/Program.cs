using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncTool
{
    class Program
    {
        static void Main(string[] args)
        {
            //load settings (settings.xml)

            //load local repo info (repo.xml), generate if doesn't exist

            //generate object chain of loaded dirs/pbos

            //download remote repo (http://rollingkeg.com/repo/repo.xml) -> (remote.xml)

            //generate object chain of loaded dirs/pbos

            //create list of pbos that have changed, hashes that have changed

            //cycle list of pbo downloads, store in temp location

            //hash downloaded pbos, compare to remote list of pbos

            //replace local pbos from temp location

            //update local xml as replace

            //compare two xml checksums for pbos, again
        }
    }
}
