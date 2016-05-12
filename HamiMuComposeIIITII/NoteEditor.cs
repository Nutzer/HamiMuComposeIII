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
    public partial class NoteEditor : Form
    {
        public NoteEditor()
        {
            InitializeComponent();
        }
        public NoteEditor(Note x)
        {
            InitializeComponent();
            n = x;
        }
        public Note n;
         Draw d = new Draw();
        private void NoteEditor_Load(object sender, EventArgs e)
        {
            info.Text = info.Text.Replace("Y", n.time.ToString()).Replace("Z", n.Position.ToString());
            if (!n.hold)
                button.SelectedIndex = n.note;
            else
                button.SelectedIndex = n.note+5;
            spmode.SelectedIndex = (int)n.SPNote;
            spbonus.Checked = n.SPLine;
            spbonus.Enabled = n.hold;
            unkn1.Value = n.unkn1;
            unkn1.Value = n.unkn1;
            linest.Value = n.LineNr;
        }

        private void button_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(button.SelectedIndex < 5)
            {
                n.note = button.SelectedIndex;
                n.hold = false;
                spbonus.Enabled = n.hold;
            }
            else
            {
                n.note = button.SelectedIndex-5;
                n.hold = true;
                spbonus.Enabled = n.hold;
            }
            panel1.Refresh();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            panel1.CreateGraphics().DrawImage(d.getImgByNr(n, false), new PointF(0, 0));
        }

        private void spmode_SelectedIndexChanged(object sender, EventArgs e)
        {
            n.SPNote = (SPType)spmode.SelectedIndex;
        }

        private void spbonus_CheckedChanged(object sender, EventArgs e)
        {
            n.SPLine = spbonus.Checked;
        }

        private void linest_ValueChanged(object sender, EventArgs e)
        {
            n.LineNr = (int)linest.Value;
        }

        private void unkn1_ValueChanged(object sender, EventArgs e)
        {
            n.unkn1 = (int)unkn1.Value;
        }

        private void unkn2_ValueChanged(object sender, EventArgs e)
        {
            n.unkn2 = (int)unkn2.Value;
        }
    }
}
