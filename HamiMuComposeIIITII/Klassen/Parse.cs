using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace HamiMuComposeIIITII
{
    public class Parse
    {
        public bool IsLoaded;
        public List<Note> LineTop, LineBottom, LineMiddle;
        public List<instruction> NonBeat;
        public int NoteOffset, LastNote;
        InstructionInterpreterType[] interprete;
        public Parse(string dscPath)
        {
            using (BinaryReader br = new BinaryReader(new StreamReader(dscPath).BaseStream))
            {
                ParseReader(br, (int)br.BaseStream.Length);
            }
        }
        public Parse()
        {

        }
        public Parse(BinaryReader br, int length)
        {
            ParseReader(br, length);
        }
        public void ParseReader(BinaryReader br, int length)
        {
            //--------------------------
            //   STEP 0: Initialize
            //--------------------------
            LineTop = new List<Note>();
            LineMiddle = new List<Note>();
            LineBottom = new List<Note>();
            NonBeat = new List<instruction>();
            IsLoaded = false;
            interprete = JsonConvert.DeserializeObject<InstructionInterpreterType[]>(Properties.Settings.Default.ParseString);


            //--------------------------
            //   STEP 1: Check Magic
            //--------------------------
            int[] magic = { 0x16, 0x01, 0x12, 0x10, 0x01, 0x00, 0x00, 0x00 };
            foreach (int i in magic)
                if (br.ReadChar() != i)
                {
                    MessageBox.Show("Magic failed!\r\nPlese make sure this is a valid type 0x12080100 DSC file.");
                    return;
                }


            //--------------------------
            // TODO: STEP 2: Retr. Args
            //--------------------------

            //UNKNOWN


            //--------------------------
            //  STEP 3: Dumping Instr.
            //--------------------------
            List<instruction> inst = new List<instruction>();
            while (br.BaseStream.Position < length)
            {
                int time=-2, type=-2;
                try
                {
                    time = br.ReadInt32();
                    type = br.ReadInt32();
                }catch(Exception ex)
                {
                    if (time == -2)
                        break;
                    if (type == -2)
                    {
                        inst.Add(new instruction(type, time, new List<int>()));
                    }
                }
                bool handle = false;
                foreach (InstructionInterpreterType t in interprete)
                    if (t.isMyType(type))
                    {
                        inst.Add(t.parse(br, time, interprete));
                        handle = true;
                    }
                if (!handle)
                    inst.Add(new instruction(type, time, new List<int>()));
            }


            //--------------------------
            //  STEP 3: Dumping Notes
            //--------------------------
            for (int i = 0; i < inst.Count; i++)
            {
                if ((inst[i].type == 6 || (inst[i].type == 14 && inst[i].args.Count > 4 && inst[i].args[4] == 6) || (inst[i].type == 19 && inst[i].args.Count > 5 && inst[i].args[5] == 6)) && inst[i].time > 200)
                {   
                    if(inst[i].type != 6)
                    {
                        int test = 0;
                    }
                    List<int> args = new List<int>();
                    args.Add(inst[i].time);
                    args.Add(inst[i].type);
                    args.AddRange(inst[i].args);
                    Note n = new Note(args.ToArray());
                    if (n.Position == Line.Upper)
                        LineTop.Add(n);
                    else if (n.Position == Line.Middle)
                        LineMiddle.Add(n);
                    else LineBottom.Add(n);

                    if(args[9] == 6)
                    {
                        for(int x = 2; x < 9; x++)
                        {
                            args[x] = args[x + 8];
                        }
                        n = new Note(args.ToArray());
                        if (n.Position == Line.Upper)
                            LineTop.Add(n);
                        else if (n.Position == Line.Middle)
                            LineMiddle.Add(n);
                        else LineBottom.Add(n);
                    }

                    inst.RemoveAt(i);
                    i--;
                }
            }


            //--------------------------
            //STEP 4: Calculating Offset
            //--------------------------

            List<Note> LineAll = GetAllLine();
            NoteOffset = 1000000;
            for (int i = 1; i < LineAll.Count; i++)
                {
                    int bufOffs = LineAll[i].time - LineAll[i-1].time;
                    if (bufOffs < NoteOffset && bufOffs > 1000)
                        NoteOffset = bufOffs;
                }

            for (int i = 0; i < LineTop.Count; i++) LineTop[i].MakeOffset(NoteOffset);
            for (int i = 0; i < LineMiddle.Count; i++) LineMiddle[i].MakeOffset(NoteOffset);
            for (int i = 0; i < LineBottom.Count; i++) LineBottom[i].MakeOffset(NoteOffset);

            LastNote = 0;
            LineAll = GetAllLine();
            for (int i = 0; i < LineAll.Count; i++)
                if (LineAll[i].time > LastNote)
                    LastNote = LineAll[i].time;


            //--------------------------
            //  STEP 5: Finishing Up
            //--------------------------
            NonBeat = inst;
            IsLoaded = true;
        }
        public int Write(BinaryWriter bw)
        {
            //--------------------------
            //  STEP 0: Initialize
            //--------------------------
            long len = bw.BaseStream.Position; //Keep Track of Length


            //--------------------------
            //STEP 1: Recalculate Offsets
            //--------------------------

            LineTop = LineTop.OrderBy(o => o.time).ToList();
            LineMiddle = LineMiddle.OrderBy(o => o.time).ToList();
            LineMiddle = LineMiddle.OrderBy(o => o.time).ToList();

            List<Note> backup = new List<Note>(GetAllLine());

            for (int i = 0; i < LineTop.Count; i++) LineTop[i].RemoveOffset(NoteOffset, backup);
            for (int i = 0; i < LineMiddle.Count; i++) LineMiddle[i].RemoveOffset(NoteOffset, backup);
            for (int i = 0; i < LineBottom.Count; i++) LineBottom[i].RemoveOffset(NoteOffset, backup);


            //--------------------------
            //STEP 3: Notes=>Instructions
            //--------------------------
            
            List<Note> n = GetAllLine();
            List<instruction> instr = new List<instruction>();
            for(int i = 0; i < n.Count; i++)
            {
                if (isNoteInLine(instr, n[i].time))
                {
                    for(int x = 0; x < instr.Count; x++)
                    {
                        if (instr[x].time == n[i].time)
                        {
                            if (instr[x].args.Count > 8)
                            {
                                MessageBox.Show("Three notes are on the same time. Have to abort.");
                                throw new Exception("NOTE ADD ERROR");
                            }
                            instr[x].args.RemoveAt(instr[x].args.Count - 1);
                            instr[x].args.Add(6);
                            instr[x].args.AddRange(n[i].ToInstruction().args);
                        }
                    }
                }
                else instr.Add(n[i].ToInstruction());
            }


            //--------------------------
            //   STEP 3: Write Notes
            //--------------------------

            int[] magic = { 0x16, 0x01, 0x12, 0x10, 0x01, 0x00, 0x00, 0x00 };
            foreach (byte i in magic)
                bw.Write(i);

            int bpos = 0;
            instr = instr.OrderBy(o => o.time).ToList();
            for (int i = 0; i < NonBeat.Count; i++)
            {
                if (NonBeat[i].args.Count <= 1 || NonBeat[i].time <= 0)
                {
                    if (NonBeat[i].type == -1)
                        bw.Write(NonBeat[i].time);
                    else
                        NonBeat[i].Write(bw);
                }
                else if (bpos < instr.Count && instr[bpos].time < NonBeat[i].time)
                {
                    instr[bpos].Write(bw);
                    bpos++;
                    i--;
                }
                else
                    NonBeat[i].Write(bw);
            }


            //--------------------------
            //   STEP 3: Add Offsets
            //--------------------------

            for (int i = 0; i < LineTop.Count; i++) LineTop[i].MakeOffset(NoteOffset);
            for (int i = 0; i < LineMiddle.Count; i++) LineMiddle[i].MakeOffset(NoteOffset);
            for (int i = 0; i < LineBottom.Count; i++) LineBottom[i].MakeOffset(NoteOffset);

            return (int)(bw.BaseStream.Position-len);
        }
        public List<Note> GetAllLine()
        {
            LineTop = LineTop.OrderBy(o => o.time).ToList();
            LineMiddle = LineMiddle.OrderBy(o => o.time).ToList();
            LineMiddle = LineMiddle.OrderBy(o => o.time).ToList();

            List<Note> LineAll = new List<Note>();
            LineAll.AddRange(LineMiddle);
            LineAll.AddRange(LineTop);
            LineAll.AddRange(LineBottom);

            return LineAll;
        }
        public void DeleteNoteAllLineAt(int pos)
        {
            if (pos >= LineMiddle.Count + LineTop.Count)
                LineBottom.RemoveAt(pos - (LineMiddle.Count + LineTop.Count));
            else if (pos >= LineMiddle.Count)
                LineTop.RemoveAt(pos - (LineMiddle.Count));
            else
                LineMiddle.RemoveAt(pos);
        }
        public void DeleteNoteAtLine(Line line, int time)
        {
            switch (line)
            {
                case Line.Upper:
                    LineTop.RemoveAt(GetNotePosLine(line, time));
                    break;
                case Line.Middle:
                    LineMiddle.RemoveAt(GetNotePosLine(line, time));
                    break;
                case Line.Lower:
                    LineBottom.RemoveAt(GetNotePosLine(line, time));
                    break;
            }
        }
        public void AddNote(Note n)
        {
            switch (n.Position)
            {
                case Line.Upper:
                    LineTop.Add(n);
                    break;
                case Line.Middle:
                    LineMiddle.Add(n);
                    break;
                case Line.Lower:
                    LineBottom.Add(n);
                    break;
            }
        }
        public int getRealAllNoteNr(Note n)
        {
            int sppos = GetNotePosLine(n.Position, n.time);
            if (n.Position != Line.Middle)
                sppos += LineMiddle.Count;
            if (n.Position == Line.Lower)
                sppos += LineTop.Count;
            return sppos;
        }
        public void SeteNoteAllLineAt(int pos, Note n)
        {
            if (pos >= LineMiddle.Count + LineTop.Count)
               LineBottom[(pos - (LineMiddle.Count + LineTop.Count))] = n;
            else if (pos >= LineMiddle.Count)
                LineTop[(pos - (LineMiddle.Count))] = n;
            else
                LineMiddle[pos] = n;
        }
        public bool isNoteAtTime(int time)
        {
            foreach (Note n in GetAllLine())
                if (n.time == time)
                    return true;
            return false;
        }
        public bool isNoteAtLineTime(Line l, int time)
        {
            return GetNoteLine(l, time)!= null;
        }
        public bool isNoteInLine(List<instruction> line, int time)
        {
            foreach (instruction n in line)
                if (n.time == time)
                    return true;
            return false;
        }
        public Note GetNoteLine(Line line, int time)
        {
            if (line == Line.Upper)
            {
                foreach (Note n in LineTop)
                    if (n.time == time) return n;
            }
            if (line == Line.Middle)
            {
                foreach (Note n in LineMiddle)
                    if (n.time == time) return n;
            }
            if (line == Line.Lower)
            {
                foreach (Note n in LineBottom)
                    if (n.time == time) return n;
            }
            return null;
        }
        public int GetNotePosLine(Line line, int time)
        {
            if (line == Line.Upper)
            {
                for (int i = 0; i < LineTop.Count; i++)
                    if (LineTop[i].time == time) return i;
            }
            if (line == Line.Middle)
            {
                for (int i = 0; i < LineMiddle.Count; i++)
                    if (LineMiddle[i].time == time) return i;
            }
            if (line == Line.Lower)
            {
                for (int i = 0; i < LineBottom.Count; i++)
                    if (LineBottom[i].time == time) return i;
            }
            return -1;
        }
    }
}