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
    public partial class CreateProject : Form
    {
        public CreateProject()
        {
            InitializeComponent();
        }
        public CreateProject(string arg)
        {
            InitializeComponent();
            textBox1.Text = arg;
        }

        public Project p = new Project();
        Parse parse;

        private void CreateProject_Load(object sender, EventArgs e)
        {
            if(textBox1.Text != "")
            {
                parse = new Parse(textBox1.Text);
                if (parse.IsLoaded)
                {
                    textBox4.Text = "DSC loaded Successfully!\r\nEvents: " + parse.NonBeat.Count + "; Notes: " + (parse.LineBottom.Count + parse.LineMiddle.Count + parse.LineTop.Count);
                    textBox2.Text = textBox1.Text.Replace(".dsc", ".hmcprj");
                }
                else
                {
                    textBox4.Text = "DSC Load Error!!";
                    textBox1.Text = "";
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "DSC files|*.dsc";
            if(of.ShowDialog() == DialogResult.OK)
            {
                parse = new Parse(of.FileName);
                if (parse.IsLoaded)
                {
                    textBox4.Text = "DSC loaded Successfully!\r\nEvents: " + parse.NonBeat.Count + "; Notes: " + (parse.LineBottom.Count+parse.LineMiddle.Count+parse.LineTop.Count);
                    textBox1.Text = of.FileName;
                }else
                {
                    textBox4.Text = "DSC Load Error!!";
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "HamiMuComposeIII Project V1.0|*.hmcprj";
            sf.AddExtension = true;
            if(sf.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = sf.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "DSC file|*.dsc";
            sf.AddExtension = true;
            if (sf.ShowDialog() == DialogResult.OK)
            {
                textBox3.Text = sf.FileName;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                p.Create(textBox2.Text, textBox1.Text);
                if (checkBox1.Checked)
                    p.Export(textBox3.Text);
                p.Save("");
                Close();
            }
        }
    }
}
