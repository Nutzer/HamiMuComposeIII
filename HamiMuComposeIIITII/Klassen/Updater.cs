using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Windows;

namespace HamiMuComposeIIITII
{
    public class Updater
    {
        public string updateString;
        string Version = "0.4.0.05";
        string[] versions;
        string updateUrl = "http://www.nutzer.bplaced.net/hamimu/versions.txt";
        public bool isUpdate;
        
        
        public Updater()
        {
            updateString = "Getting Update data...";
            System.ComponentModel.BackgroundWorker bw1 = new System.ComponentModel.BackgroundWorker();
            bw1.DoWork += Bw1_DoWork;
            isUpdate = false;
            bw1.RunWorkerAsync();
        }

        private void Bw1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                WebClient wc = new WebClient();
                versions = wc.DownloadString(updateUrl).Split('|');
                if (versions[0] != Version)
                {
                    updateString = "New Update available (" + versions[0] + ")";
                    isUpdate = true;
                }
                else
                {
                    updateString = "You're Up to date! (" + versions[0] + ")";
                }
                string str = wc.DownloadString(updateUrl.Replace("versions.txt", "ParseString.json"));
                if(str != Properties.Settings.Default.ParseString)
                {
                    Properties.Settings.Default.ParseString = str;
                    Properties.Settings.Default.Save();
                    updateString += "       Updated ParseString, though...";
                }
            }
            catch (Exception ex)
            {
                updateString = "Could not get Update information: " + ex.Message;
            }
        }

        public void UpdateInit(string sysPath)
        {
            System.Diagnostics.Process.Start(Path.GetDirectoryName(sysPath)+"//Update.exe", "\""+versions[0]+"\"");
        }
    }
}
