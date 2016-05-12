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
    public enum FadeMode { FadeTop, FadeBottom, FadeLeft, FadeRight };
    public class FadeInItem
    {
        public FadeMode mode;
        public string message;
        public Font font;
        public PointF pos, tpos;
        public int div, fade;
        public bool isClick;
        public FadeInItem(FadeMode mde, string msg, Font f, PointF p, int d, bool isclick)
        {
            mode = mde;
            message = msg;
            tpos = p;
            pos = p;
            div = d;
            isClick = isclick;
            font = f;
            fade = 0;
            switch (mode)
            {
                case FadeMode.FadeBottom:
                    tpos.Y += 40;
                    break;
                case FadeMode.FadeTop:
                    tpos.Y -= 40;
                    break;
                case FadeMode.FadeRight:
                    tpos.X += 40;
                    break;
                case FadeMode.FadeLeft:
                    tpos.X -= 40;
                    break;
            }
        }
        public void UpdateAndDraw(Graphics g, int p, Point mouse)
        {
            tpos.X += (pos.X - tpos.X) / div;
            tpos.Y += (pos.Y - tpos.Y) / div;
            if (tpos.X > pos.X)
                tpos.X -= 0.5f;
            if (tpos.X < pos.X)
                tpos.X += 0.5f;
            if (tpos.Y > pos.Y)
                tpos.Y -= 0.5f;
            if (tpos.Y < pos.Y)
                tpos.Y += 0.5f;
            fade += (255 - fade) / (div+2);
            if (mouse.X > tpos.X && mouse.Y > tpos.Y && mouse.X < tpos.X + 100 && mouse.Y < tpos.Y + 20 && isClick)
                g.DrawString(message, font, new SolidBrush(Color.FromArgb(255, Color.Blue)), new PointF(tpos.X - p, tpos.Y));
            else
                g.DrawString(message, font, new SolidBrush(Color.FromArgb(fade, Color.Black)), new PointF(tpos.X - p, tpos.Y));
        }
        public bool isMouseOn(Point mouse)
        {
            return (mouse.X > tpos.X && mouse.Y > tpos.Y && mouse.X < tpos.X + 100 && mouse.Y < tpos.Y + 20 && isClick);
        }
    };

    public enum DrawMode { NoteDrag, MainMenu };
    class Draw
    {
        public Image a, b, x, y, st, da, db, dx, dy, ds, cir2, line, bnl, spl, anzl;
        public int Abstand, lastPos;
        public Note DragNote;
        public int SelNote;
        public List<FadeInItem> items;
        public DrawMode mode;
        List<int> handledNotes;
        int PreLoad;
        public Updater u;
        public Draw(string version)
        {
            a = Image.FromFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\img\\a.png");
            b = Image.FromFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\img\\b.png");
            x = Image.FromFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\img\\x.png");
            y = Image.FromFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\img\\y.png");
            st = Image.FromFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\img\\s.png");
            da = Image.FromFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\img\\da.png");
            db = Image.FromFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\img\\db.png");
            dx = Image.FromFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\img\\dx.png");
            dy = Image.FromFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\img\\dy.png");
            ds = Image.FromFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\img\\ds.png");
            cir2 = Image.FromFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\img\\cir2.png");
            line = Image.FromFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\img\\hold.png");
            bnl = Image.FromFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\img\\bln.png");
            spl = Image.FromFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\img\\holds.png");
            anzl = Image.FromFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\img\\line.png");
            Abstand = 32;
            SelNote = -1;
            lastPos = -1;
            mode = DrawMode.MainMenu;
            PreLoad = 100;
            items = new List<FadeInItem>();
            u = new Updater(version);
        }
        public Draw()
        {
            a = Image.FromFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\img\\a.png");
            b = Image.FromFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\img\\b.png");
            x = Image.FromFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\img\\x.png");
            y = Image.FromFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\img\\y.png");
            st = Image.FromFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\img\\s.png");
            da = Image.FromFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\img\\da.png");
            db = Image.FromFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\img\\db.png");
            dx = Image.FromFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\img\\dx.png");
            dy = Image.FromFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\img\\dy.png");
            ds = Image.FromFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\img\\ds.png");
            cir2 = Image.FromFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\img\\cir2.png");
            line = Image.FromFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\img\\hold.png");
            bnl = Image.FromFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\img\\bln.png");
            spl = Image.FromFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\img\\holds.png");
            anzl = Image.FromFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\img\\line.png");
            Abstand = 32;
            SelNote = -1;
            lastPos = -1;
            mode = DrawMode.MainMenu;
            PreLoad = 100;
            items = new List<FadeInItem>();
        }
        public int min(int a, int b)
        {
            if (a < b) return b;
            return a;
        }
        public void DrawMain(Graphics g, Project p, int pos, Size s, Point mouse)
        {
            if (mode == DrawMode.NoteDrag)
                DrawNotes(g, p, pos, s, mouse);
            if (mode == DrawMode.MainMenu)
                DrawMainMenu(g, p, pos, s, mouse);
        }

        public void DrawMainMenu(Graphics g, Project p, int pos, Size s, Point mouse)
        {
            foreach (FadeInItem f in items)
                f.UpdateAndDraw(g, pos*5, mouse);
            items[items.Count - 1].message = u.updateString;
            items[items.Count - 1].isClick = u.isUpdate;
        }
        public void DrawNotes(Graphics g, Project p, int pos, Size s, Point mouse)
        {
            handledNotes = new List<int>();
            if (lastPos == -1)
                lastPos = s.Width / 2 - pos * Abstand;
            int npos = s.Width / 2 - pos * Abstand;
            lastPos += (npos - lastPos) / 5;

            g.DrawImage(anzl, new PointF(s.Width / 2+lastPos-npos, 0));

            g.DrawImage(a, new PointF(10, 40));
            g.DrawImage(b, new PointF(60, 40));
            g.DrawImage(x, new PointF(110, 40));
            g.DrawImage(y, new PointF(160, 40));
            g.DrawImage(st, new PointF(210, 40));
            g.DrawImage(line, new PointF(280, 40));
            g.DrawImage(line, new PointF(340, 40));
            g.DrawImage(line, new PointF(400, 40));
            g.DrawImage(line, new PointF(460, 40));
            g.DrawImage(a, new PointF(260, 40));
            g.DrawImage(b, new PointF(320, 40));
            g.DrawImage(x, new PointF(380, 40));
            g.DrawImage(y, new PointF(440, 40));
            g.DrawString(pos.ToString() + " of " + p.getDsc().LastNote.ToString(), SystemFonts.DefaultFont, Brushes.Black, new PointF(s.Width - 100, s.Height - 35));
            if(p.s != null)
            {
                g.DrawString("Real Time: " + (int)(p.s.getpos()/1000)+":"+(p.s.getpos()%1000/10).ToString("D2"), SystemFonts.DefaultFont, Brushes.Black, new PointF(s.Width - 100, s.Height - 50));
            }
            else
                g.DrawString("Add music for Real time", SystemFonts.DefaultFont, Brushes.Gray, new PointF(s.Width - 130, s.Height - 50));

            for (int i = min(pos-PreLoad, 0); i < pos+PreLoad; i++)
            {
                Note n = p.getDsc().GetNoteLine(Line.Upper, i);
                if (n != null)
                    DrawNote(n, g, p, lastPos + (i * Abstand), mouse);

                n = p.getDsc().GetNoteLine(Line.Middle, i);
                if (n != null)
                    DrawNote(n, g, p, lastPos + (i * Abstand), mouse);

                n = p.getDsc().GetNoteLine(Line.Lower, i);
                if (n != null)
                    DrawNote(n, g, p, lastPos + (i * Abstand), mouse);
            }
            if(DragNote != null)
            {
                bool isDark = false;
                if (mouse.Y <= 120 || mouse.Y > 222)
                    isDark = true;
                g.DrawImage(getImgByNr(DragNote, isDark), new PointF(mouse.X - 16, mouse.Y - 16));
            }
            if(SelNote > -1)
            {
                g.FillRectangle(Brushes.LightGray, new RectangleF(0, s.Height-40, s.Width, 40));
                Note n = p.getDsc().GetAllLine()[SelNote];
                int xpos = 5;
                g.DrawString("Note at " + n.time, SystemFonts.DefaultFont, Brushes.Black, new PointF(xpos, s.Height - 35));
                xpos += 120;
                if (n.LineNr != 0)
                {
                    g.DrawString("Line " + n.LineNr + " start", SystemFonts.DefaultFont, Brushes.Black, new PointF(xpos, s.Height - 35));
                    xpos += 120;
                }
                if(n.SPNote != SPType.none)
                {
                    if (n.SPNote == SPType.start)
                        g.DrawString("SP line start", SystemFonts.DefaultFont, Brushes.Black, new PointF(xpos, s.Height - 35));
                    else
                        g.DrawString("SP line stop", SystemFonts.DefaultFont, Brushes.Black, new PointF(xpos, s.Height - 35));
                    xpos += 120;
                }
                if (n.SPLine)
                {
                    g.DrawString("Bonus Hold Trigger", SystemFonts.DefaultFont, Brushes.Black, new PointF(xpos, s.Height - 35));
                    xpos += 120;
                }
                if (n.unkn1 != 0 || n.unkn2 != 0)
                {
                    g.DrawString("EARGS: " + n.unkn1 + " " + n.unkn2, SystemFonts.DefaultFont, Brushes.Black, new PointF(xpos, s.Height - 35));
                    xpos += 120;
                }
                g.DrawString("Click To Edit!!", SystemFonts.DefaultFont, Brushes.Blue, new PointF(xpos+10, s.Height - 35));
            }
        }
        public void SetSelNote(Point mouse, Project p)
        {
            int npos = -1;
            for (int i = 0; i < p.getDsc().GetAllLine().Count; i++)
            {
                Note n = p.getDsc().GetAllLine()[i];
                if (mouse.X > lastPos + (n.time * Abstand) && mouse.X < lastPos + ((n.time + 1) * Abstand))
                {
                    if (n.Position == Line.Upper && mouse.Y > 120 && mouse.Y < 154)
                        npos = i;
                    if (n.Position == Line.Middle && mouse.Y > 154 && mouse.Y < 188)
                        npos = i;
                    if (n.Position == Line.Lower && mouse.Y > 188 && mouse.Y < 222)
                        npos = i;
                }
            }

            if(npos != -1)
            {
                SelNote = npos;
            }
        }
        public void getSelNote(Point mouse, Project p)
        {
            int npos = -1;
            SelNote = -1;
            for(int i = 0; i < p.getDsc().GetAllLine().Count; i++)
            {
                Note n = p.getDsc().GetAllLine()[i];
                if(mouse.X > lastPos + (n.time * Abstand) && mouse.X < lastPos + ((n.time + 1) * Abstand))
                {
                    if (n.Position == Line.Upper && mouse.Y > 120 && mouse.Y < 154)
                        npos = i;
                    if (n.Position == Line.Middle && mouse.Y > 154 && mouse.Y < 188)
                        npos = i;
                    if (n.Position == Line.Lower && mouse.Y > 188 && mouse.Y < 222)
                        npos = i;
                }
            }

            if(npos != -1)
            {
                DragNote = new Note(p.getDsc().GetAllLine()[npos]);
                p.getHistory().NoteRemoved(new Note(DragNote));
                p.getDsc().DeleteNoteAllLineAt(npos);
            }
            else
            {
                if(mouse.Y > 40 && mouse.Y < 80)
                {
                    if (mouse.X > 10 && mouse.X < 34)
                        DragNote = new Note(new int[] { 0, 6, 1, 0, 0, 0, 0, 0, 0 });
                    if (mouse.X > 60 && mouse.X < 94)
                        DragNote = new Note(new int[] { 0, 6, 2, 0, 0, 0, 0, 0, 0 });
                    if (mouse.X > 110 && mouse.X < 144)
                        DragNote = new Note(new int[] { 0, 6, 0, 0, 0, 0, 0, 0, 0 });
                    if (mouse.X > 160 && mouse.X < 194)
                        DragNote = new Note(new int[] { 0, 6, 3, 0, 0, 0, 0, 0, 0 });
                    if (mouse.X > 210 && mouse.X < 244)
                        DragNote = new Note(new int[] { 0, 6, 12, 0, 0, 0, 0, 0, 0 });
                    if (mouse.X > 260 && mouse.X < 260+34)
                        DragNote = new Note(new int[] { 0, 6, 9, 0, 0, 0, 0, 0, 0 });
                    if (mouse.X > 320 && mouse.X < 320+34)
                        DragNote = new Note(new int[] { 0, 6, 10, 0, 0, 0, 0, 0, 0 });
                    if (mouse.X > 380 && mouse.X < 380+34)
                        DragNote = new Note(new int[] { 0, 6, 8, 0, 0, 0, 0, 0, 0 });
                    if (mouse.X > 440 && mouse.X < 440+34)
                        DragNote = new Note(new int[] { 0, 6, 11, 0, 0, 0, 0, 0, 0 });
                }
            }
        }
        public void DrawNote(Note n, Graphics g, Project p, int posx, Point mouse)
        {
            bool dark = false;
            if (mouse.X > posx && mouse.X < posx + Abstand)
                dark = true;

            if (n.hold && !handledNotes.Contains(p.getDsc().getRealAllNoteNr(n)))
            {
                for (double time = n.time + 0.25; time < p.getDsc().LastNote; time += 0.25)
                {
                    if (p.getDsc().GetNoteLine(n.Position, (int)time) != null && (int)time != n.time)
                    {
                        handledNotes.Add(p.getDsc().getRealAllNoteNr(p.getDsc().GetNoteLine(n.Position, (int)time)));
                        break;
                    }
                    if (n.SPLine)
                        g.DrawImage(spl, new PointF(posx + (int)(Abstand * (time - n.time)), 120 + ((int)n.Position * 32)));
                    else
                        g.DrawImage(line, new PointF(posx + (int)(Abstand * (time - n.time)), 120 + ((int)n.Position * 32)));
                }
            }
            if (n.SPNote == SPType.start)
            {
                for (double time = n.time + 0.5; time < p.getDsc().LastNote; time += 0.25)
                {
                    if (p.getDsc().GetNoteLine(n.Position, (int)(time-0.5)) != null && p.getDsc().GetNoteLine(n.Position, (int)(time - 0.5)).SPNote == SPType.stop)
                    {
                        break;
                    }
                    g.DrawImage(bnl, new PointF(posx + (int)(Abstand * (time - n.time)), 150 + ((int)n.Position * 32)));
                }
            }

            g.DrawImage(getImgByNr(n, !(dark && (mouse.Y > 120 + ((int)n.Position * 32) && mouse.Y < 120 + ((int)n.Position * 32)+32))), new PointF(posx, 120 + ((int)n.Position * 32)));
        }
        public int timeAt(Point mouse, Project p)
        {
            for (int i = 0; i < p.getDsc().LastNote; i++)
            {
                if (mouse.X > lastPos + (i * Abstand) && mouse.X < lastPos + ((i + 1) * Abstand))
                {
                    return i;
                }
            }
            return 0;
        }
        public void DropDragNoteAt(Point mouse, Project p)
        {
            if (DragNote != null)
                for (int i = 0; i < p.getDsc().LastNote; i++)
                {
                    if (mouse.X > lastPos + (i * Abstand) && mouse.X < lastPos + ((i + 1) * Abstand))
                    {
                        Line l = 0;
                        if (mouse.Y > 120 && mouse.Y <= 154)
                            l = Line.Upper;
                        else if (mouse.Y > 154 && mouse.Y <= 188)
                            l = Line.Middle;
                        else if (mouse.Y > 188 && mouse.Y < 222)
                            l = Line.Lower;

                        if (p.getDsc().isNoteAtLineTime(l, i))
                        {
                            DragNote = null;
                            p.getHistory().NoteAbschluss();
                            return;
                        }
                        DragNote.time = i;
                        if (mouse.Y > 120 && mouse.Y <= 154)
                        {
                            DragNote.Position = Line.Upper;
                            p.getDsc().LineTop.Add(new Note(DragNote));
                            p.getHistory().NoteAdded(new Note(DragNote));
                            DragNote = null;
                        }
                        else if (mouse.Y > 154 && mouse.Y <= 188)
                        {
                            DragNote.Position = Line.Middle;
                            p.getDsc().LineMiddle.Add(new Note(DragNote));
                            p.getHistory().NoteAdded(new Note(DragNote));
                            DragNote = null;
                        }
                        else if (mouse.Y > 188 && mouse.Y < 222)
                        {
                            DragNote.Position = Line.Lower;
                            p.getDsc().LineBottom.Add(new Note(DragNote));
                            p.getHistory().NoteAdded(new Note(DragNote));
                            DragNote = null;
                        }
                        else
                        {
                            DragNote = null;
                        }

                        return;
                    }
                }
            DragNote = null;
        }
        public Image getImgByNr(Note n, bool isBlack)
        {
            switch (n.note)
            {
                case (int)Notes.a:
                    if (isBlack)
                        return da;
                    return a;
                    break;
                case (int)Notes.b:
                    if (isBlack)
                        return db;
                    return b;
                    break;
                case (int)Notes.x:
                    if (isBlack)
                        return dx;
                    return x;
                    break;
                case (int)Notes.y:
                    if (isBlack)
                        return dy;
                    return y;
                    break;
                case (int)Notes.S:
                    if (isBlack)
                        return ds;
                    return st;
                    break;
            }
            return st;
            throw new Exception("Note not Differenciable: " + n.note.ToString());
        }   
    }
}
