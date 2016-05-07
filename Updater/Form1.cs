using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;

namespace Updater
{
    public partial class Form1 : Form
    {
        string ver;
        public Form1()
        {
            InitializeComponent();
        }
        public Form1(string msg)
        {
            InitializeComponent();
            ver = msg;
        }

        string tb1, tb2;
        int pb1, pb2, pb1max;
        string updateUrl = "http://www.nutzer.bplaced.net/hamimu/";

        private void timer1_Tick(object sender, EventArgs e)
        {
            textBox1.Text = tb1;
            textBox2.Text = tb2;
            if (progressBar1.Maximum != pb1max)
                progressBar1.Maximum = pb1max;
            progressBar1.Value = pb1;
            progressBar2.Value = pb2;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            System.Diagnostics.Process.Start("HamiMuComposeIII.exe");
            Close();
        }

        bool DownComplete;

        private async void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            WebClient wc = new WebClient();
            wc.DownloadProgressChanged += Wc_DownloadProgressChanged;
            wc.DownloadFileCompleted += Wc_DownloadFileCompleted;
            string[] files = wc.DownloadString(updateUrl + ver + "/fileList.txt").Split('|');
            pb1 = 0;
            pb1max = files.Length;
            foreach(string f in files)
            {
                tb1 = "Downloading File " + (pb1 + 1) + " of " + pb1max;
                tb2 = updateUrl + ver + "/" + f + " => " + f.Replace("/", "\\");
                if (f.Contains("/"))
                {
                    try
                    {
                        Directory.CreateDirectory(f.Split('/')[0]);
                    }catch(Exception ex)
                    {

                    }
                }
                DownComplete = false;
                wc.DownloadFileAsync(new Uri(updateUrl + ver + "/" + f), f.Replace("/", "\\"));
                while (!DownComplete) ;
                pb1++;
            }
        }

        private void Wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            DownComplete = true;
        }

        private void Wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            pb2 = e.ProgressPercentage;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tb1 = "Getting File list...";
            backgroundWorker1.RunWorkerAsync();
            timer1.Interval = 1;
            timer1.Start();
        }
    }
}
