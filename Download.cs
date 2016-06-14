
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Tool
{
    class UpdateList
    {
        public static App[] detect(List<App> list)
        {
            foreach (App a in list.ToArray())
                if ((a.url == "null") || (a.url == ""))
                    list.Remove(a);

            App[] array = list.ToArray();
            return array;
        }
    }

    class Download
    {
        public static void runDownload(App[] a, CheckedListBox c)
        {
            //count checked apps
            int appsChecked = 0;
            for (int i = (c.Items.Count - 1); i >= 0; i--)
            {
                if (c.GetItemCheckState(i) == CheckState.Checked)
                {
                    appsChecked++;
                }
            }

            //start task window
            Form2 form = new Form2();

            //run apps
            int currentAppNum = 1;
            for (int i = 0; i < (a.Length); i++)
            {
                if (c.GetItemCheckState(i) == CheckState.Checked)
                {
                    form.taskName.Text = "Downloading: " + a[i].name;
                    form.taskNumber.Text = currentAppNum.ToString() + " / " + appsChecked.ToString();
                    download(a[i]);
                    form.progressBar.Value = (int)((double)currentAppNum / (double)appsChecked * 100.0);
                    currentAppNum++;
                };
            };
            form.progressBar.Value = 100;
            form.Close();
        }

        static void download(App a)
        {
            App wget = new App("Downloading " + a.name, "wget.exe", a.url + " -O " + a.file, "\\Utilities\\wget\\", "", "", "");
            new Run(wget);
            move(a);
        }

        public static void move(App a)
        {
            if(File.Exists(a.sdir + "\\" + a.file)) File.Delete(a.sdir + "\\" + a.file);
            File.Move("\\Utilities\\wget\\" + a.file, a.sdir + "\\" + a.file);
        }

        public static void extract(string s)
        {



        }
    }
}
