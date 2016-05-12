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
using Newtonsoft.Json;

namespace HamiMuComposeIIITII
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public Form1(string arg)
        {
            InitializeComponent();
            args = arg;
        }

        Project p;
        Draw d;
        Point mouse;
        bool updateAvailQuest = false;
        string args = "";

        public string[] recent;

        private void Form1_Load(object sender, EventArgs e)
        {
            recent = JsonConvert.DeserializeObject<string[]>(Properties.Settings.Default.Recent);
            for(int i = 0; i < 10; i++)
                if(recent[i] != "")
                {
                    System.Windows.Forms.ToolStripMenuItem tmp = new ToolStripMenuItem(i.ToString() + ": " +Path.GetFileNameWithoutExtension(recent[i]));
                    tmp.Click += Tmp_Click;
                    openProjectToolStripMenuItem.DropDownItems.Add(tmp);
                    
                }

            menuStrip1.Visible = false;
            string version = "0.4.0.06";
            if (Properties.Settings.Default.UpdateNotice != version)
            {
                Properties.Settings.Default.UpdateNotice = version;
                UpdateMessage um = new UpdateMessage(Properties.Settings.Default.UpdateNotice);
                um.ShowDialog();
                Properties.Settings.Default.Save();
            }

            mouse = new Point(0, 0);
           
            p = new Project();
            d = new Draw(version);
            timer1.Interval = 1;
            timer1.Start();
            Size = new Size(Size.Width, 320);

            Font f = new Font(SystemFonts.DefaultFont.FontFamily, 25.0f);
            d.items.Add(new FadeInItem(FadeMode.FadeBottom, "HamiMuCompose III "+version+"!!", f, new PointF(100, 20), 10, false));
            f = new Font(SystemFonts.DefaultFont.FontFamily, 18.0f);
            d.items.Add(new FadeInItem(FadeMode.FadeRight, "By Nutzer!", f, new PointF(600, 40), 12, false));
            d.items.Add(new FadeInItem(FadeMode.FadeBottom, "Recent Projects:", f, new PointF(40, 100), 15, false));
            d.items.Add(new FadeInItem(FadeMode.FadeTop, "Create Project", f, new PointF(40, 200), 15, true));
            d.items.Add(new FadeInItem(FadeMode.FadeTop, "Open Project", f, new PointF(240, 200), 15, true));
            f = new Font(SystemFonts.DefaultFont.FontFamily, 12.0f);
            for (int i = 0; i < 10; i++)
                if (recent[i] != "")
                    d.items.Add(new FadeInItem(FadeMode.FadeRight, i.ToString() + ": " + Path.GetFileNameWithoutExtension(recent[i]), f, new PointF(100 + (i * 170), 150 + (i % 2)*15), 20 + i * 4, true));

            hScrollBar1.Maximum = 300;
            f = new Font(SystemFonts.DefaultFont.FontFamily, 8.0f);
            d.items.Add(new FadeInItem(FadeMode.FadeBottom, "Retrieving Update data...", f, new PointF(20, panel1.Size.Height-35), 15, false));

            if (args.Contains(".dsc"))
            {
                CreateProject cp = new CreateProject(args);
                cp.ShowDialog();
                if (cp.p.isOpen)
                {
                    p = cp.p;
                    hScrollBar1.Value = 0;
                    hScrollBar1.Maximum = p.getDsc().LastNote;
                    for (int i = 9; i > 0; i--)
                        recent[i] = recent[i - 1];
                    recent[0] = p.getPath();
                    Properties.Settings.Default.Recent = JsonConvert.SerializeObject(recent);
                    Properties.Settings.Default.Save();
                    openProjectToolStripMenuItem.DropDownItems.Clear();
                    for (int i = 0; i < 10; i++)
                        if (recent[i] != "")
                        {
                            System.Windows.Forms.ToolStripMenuItem tmp = new ToolStripMenuItem(i.ToString() + ": " + Path.GetFileNameWithoutExtension(recent[i]));
                            tmp.Click += Tmp_Click;
                            openProjectToolStripMenuItem.DropDownItems.Add(tmp);
                        }
                }
            }
            else if (args.Contains(".hmcprj"))
            {
                p = new Project();
                p.Open(args);
                hScrollBar1.Value = 0;
                hScrollBar1.Maximum = p.getDsc().LastNote;
                for (int i = 9; i > 0; i--)
                    recent[i] = recent[i - 1];
                recent[0] = args;
                Properties.Settings.Default.Recent = JsonConvert.SerializeObject(recent);
                Properties.Settings.Default.Save();
                openProjectToolStripMenuItem.DropDownItems.Clear();
                for (int i = 0; i < 10; i++)
                    if (recent[i] != "")
                    {
                        System.Windows.Forms.ToolStripMenuItem tmp = new ToolStripMenuItem(i.ToString() + ": " + Path.GetFileNameWithoutExtension(recent[i]));
                        tmp.Click += Tmp_Click;
                        openProjectToolStripMenuItem.DropDownItems.Add(tmp);
                    }
            }
        }



        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(p != null)
            {
                try
                {
                    p.Save("");
                }catch(Exception ex)
                {
                    MessageBox.Show("Could not save. Please select a different path.\r\n\r\n(Message: " + ex.Message + ")");
                    saveAsToolStripMenuItem_Click(null, null);
                }
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (p != null)
            {
                try
                {
                    p.Export("");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not export. Please select a different path.\r\n\r\n(Message: " + ex.Message + ")");
                    exportAsToolStripMenuItem_Click(null, null);
                }
            }
        }

        public void rewritePanel()
        {
            Bitmap bufl = new Bitmap(panel1.Width, panel1.Height);
            using (Graphics g = Graphics.FromImage(bufl))
            {
                try
                {
                    g.FillRectangle(Brushes.WhiteSmoke, new RectangleF(0, 0, panel1.Width, panel1.Height));
                    d.DrawMain(g, p, hScrollBar1.Value, panel1.Size, mouse);
                }
                catch (Exception ex) { }
                panel1.CreateGraphics().DrawImageUnscaled(bufl, 0, 0);
            }
            bufl.Dispose();

        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            if (p.isOpen && d.mode == DrawMode.MainMenu)
                d.mode = DrawMode.NoteDrag;
            rewritePanel();
            if (!menuStrip1.Visible && p.isOpen)
                menuStrip1.Visible = true;
            if (!updateAvailQuest && d.u.isUpdate)
            {
                updateAvailQuest = true;
                if (MessageBox.Show(d.u.updateString + "\r\nDo you want to update?", "Update Notice", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    d.u.UpdateInit(Application.ExecutablePath);
                    Close();
                }
            }
            if(p.s != null && p.s.isplay)
            {
                try
                {
                    hScrollBar1.Value = (int)((float)(p.s.getpos() - p.s.add) / p.s.offs + p.getDsc().GetAllLine()[0].time);
                }
                catch (Exception ex)
                {
                }
            }
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            if(p.s != null)
            {
                try
                {
                    p.s.setpos((int)(((float)(hScrollBar1.Value - p.getDsc().GetAllLine()[0].time) * p.s.offs) + p.s.add));
                }
                catch (Exception ex)
                {
                }
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (Size.Height != 320)
                Size = new Size(Size.Width, 320);
            hScrollBar1.Size = new Size( panel1.Size.Width, hScrollBar1.Size.Height);
            hScrollBar1.Location = new Point(0, panel1.Size.Height - hScrollBar1.Size.Height);
        }


        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            mouse = new Point(e.X, e.Y);
        }

        bool isClick = false;

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (d.mode == DrawMode.NoteDrag)
            {
                if (e.Button == MouseButtons.Left && mouse.Y > 222 && d.SelNote != -1)
                {
                    NoteEditor ne = new NoteEditor(p.getDsc().GetAllLine()[d.SelNote]);
                    ne.ShowDialog();
                    p.getDsc().SeteNoteAllLineAt(d.SelNote, ne.n);
                }
                if (e.Button == MouseButtons.Left)
                    d.getSelNote(mouse, p);
                else
                    d.SetSelNote(mouse, p);
            }
            else if(d.mode == DrawMode.MainMenu)
            {
                foreach(FadeInItem i in d.items)
                {
                    if (i.isMouseOn(mouse))
                    {
                        if (i.message.Contains(":"))
                        {
                            int pos = Convert.ToInt32(i.message.Split(':')[0]);
                            p.Open(recent[pos]);
                            hScrollBar1.Value = 0;
                            hScrollBar1.Maximum = p.getDsc().LastNote;
                        }
                        else if (i.message.Contains("Create"))
                        {
                            createProjectToolStripMenuItem_Click(null, null);
                        }
                        else if (i.message.Contains("Open"))
                        {
                            openProjectToolStripMenuItem_Click(null, null);
                        }
                        else
                        {
                            d.u.UpdateInit(Application.ExecutablePath);
                            Close();
                        }
                    }
                }
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (d.mode == DrawMode.NoteDrag)
                d.DropDragNoteAt(mouse, p);
        }

        private void panel1_MouseLeave(object sender, EventArgs e)
        {
            mouse = new Point(-100, -100);
        }

        private void panel1_DoubleClick(object sender, EventArgs e)
        {

        }

        private void moreDistanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (d.Abstand < 100)
                d.Abstand += 5;
            d.lastPos = panel1.Size.Width / 2 - hScrollBar1.Value * d.Abstand;
        }

        private void lessDistanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (d.Abstand > 10)
                d.Abstand -= 5;
            d.lastPos = panel1.Size.Width / 2 - hScrollBar1.Value * d.Abstand;
        }

        private void createProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateProject cp = new CreateProject();
            cp.ShowDialog();
            if (cp.p.isOpen)
            {
                p = cp.p;
                hScrollBar1.Value = 0;
                hScrollBar1.Maximum = p.getDsc().LastNote;
                for (int i = 9; i > 0; i--)
                    recent[i] = recent[i - 1];
                recent[0] = p.getPath();
                Properties.Settings.Default.Recent = JsonConvert.SerializeObject(recent);
                Properties.Settings.Default.Save();
                openProjectToolStripMenuItem.DropDownItems.Clear();
                for (int i = 0; i < 10; i++)
                    if (recent[i] != "")
                    {
                        System.Windows.Forms.ToolStripMenuItem tmp = new ToolStripMenuItem(i.ToString() + ": " + Path.GetFileNameWithoutExtension(recent[i]));
                        tmp.Click += Tmp_Click;
                        openProjectToolStripMenuItem.DropDownItems.Add(tmp);
                    }
            }
        }

        private void Tmp_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            int pos = Convert.ToInt32(item.Text.Split(':')[0]);
            p.Open(recent[pos]);
            hScrollBar1.Value = 0;
            hScrollBar1.Maximum = p.getDsc().LastNote;
        }

        private void openProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "HamiMuComposeIII Project V1.0|*.hmcprj";
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                p = new Project();
                p.Open(ofd.FileName);
                hScrollBar1.Value = 0;
                hScrollBar1.Maximum = p.getDsc().LastNote;
                for (int i = 9; i > 0; i--)
                    recent[i] = recent[i - 1];
                recent[0] = ofd.FileName;
                Properties.Settings.Default.Recent = JsonConvert.SerializeObject(recent);
                Properties.Settings.Default.Save();
                openProjectToolStripMenuItem.DropDownItems.Clear();
                for (int i = 0; i < 10; i++)
                    if (recent[i] != "")
                    {
                        System.Windows.Forms.ToolStripMenuItem tmp = new ToolStripMenuItem(i.ToString() + ": " + Path.GetFileNameWithoutExtension(recent[i]));
                        tmp.Click += Tmp_Click;
                        openProjectToolStripMenuItem.DropDownItems.Add(tmp);
                    }
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "HamiMuComposeIII Project V1.0|*.hmcprj";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                p.Save(sfd.FileName);
            }
        }

        private void exportAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "DSC File|*.dsc";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                p.Export(sfd.FileName);
            }
        }

        private void forwardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (hScrollBar1.Value < hScrollBar1.Maximum)
                hScrollBar1.Value++;
            hScrollBar1_Scroll(null, null);
        }

        private void backwardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (hScrollBar1.Value > 0)
                hScrollBar1.Value--;
            hScrollBar1_Scroll(null, null);
        }

        private void toStartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hScrollBar1.Value = 0;
            hScrollBar1_Scroll(null, null);
        }

        private void toEndToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hScrollBar1.Value = hScrollBar1.Maximum - 1;
            hScrollBar1_Scroll(null, null);
        }

        private void addSoundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Wave Sound|*.wav";
            if (ofd.ShowDialog() == DialogResult.OK) {
                Sound s = new Sound();
                s.loadSound(ofd.FileName);
                SetPlayerPos spp = new SetPlayerPos(s, p.getDsc().LastNote);
                spp.ShowDialog();
                if (spp.s.isplay)
                    spp.s.stopAll();
                if(spp.hascl)
                    p.AddMusic(ofd.FileName, spp.s.add, spp.s.offs);
            }
        }

        private void playPauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (p.s != null)
                p.s.StartStop();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (p.s != null && p.s.isplay)
            {
                p.s.StartStop();
            }
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            p.getHistory().undo(p);
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            p.getHistory().redo(p);
        }

        private void addNoteSpaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            p.getDsc().LastNote++;
            hScrollBar1.Maximum++;
        }
    }
}
