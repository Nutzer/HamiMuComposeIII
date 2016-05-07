using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HamiMuComposeIIITII
{
    public partial class SetPlayerPos : Form
    {
        public SetPlayerPos()
        {
            InitializeComponent();
        }
        public Sound s;
        public bool cl;
        int maxlen = 0;
        public bool hascl = false;
        public SetPlayerPos(Sound S, int Maxlen)
        {
            InitializeComponent();
            s = S;
            cl = false;
            timer1.Start();
            maxlen = Maxlen;
            s.add = -1;
            s.offs = -1;
        }

        private void SetPlayerPos_Load(object sender, EventArgs e)
        {
            trackBar1.Value = 0;
            trackBar1.Maximum = s.getlen();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            if(s.add==-1 || s.offs==-1)
            {
                MessageBox.Show("Please Select Notes First.");
                return;
            }
            s.setpos(0);
            hascl = true;
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            s.StartStop();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            s.setpos(trackBar1.Value);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (s.isplay)
                trackBar1.Value = s.getpos();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            s.add = (float)trackBar1.Value;
            label2.Text = "First Note: " + s.add.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            s.offs = (float)(trackBar1.Value-s.add) / (float)maxlen;
            label3.Text = "Note Offset: " + s.offs.ToString();
        }
    }
}
