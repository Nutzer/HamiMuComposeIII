using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace HamiMuComposeIIITII
{
    public enum SPType { none, start, stop };
    public enum Notes { a = 1, b = 2, x = 0, y = 3, A = 9, B = 10, X = 8, Y = 11, S = 12 };
    public enum Line { Upper, Middle, Lower };

    public class Note
    {
        public int orgNote;
        public int[] orgArgs;
        public int time, orgtime;
        public int note;
        public bool SPLine, hold;
        public Line Position;
        public SPType SPNote;
        public int LineNr, unkn1, unkn2;
        public Note()
        {

        }
        public Note(Note n)
        {
            time = n.time;
            orgtime = n.orgtime;
            note = n.note;
            SPLine = n.SPLine;
            hold = n.hold;
            Position = n.Position;
            SPNote = n.SPNote;
            LineNr = n.LineNr;
            unkn1 = n.unkn1;
            unkn2 = n.unkn2;
        }
        public Note(int[] args)
        {       //Argument Structure for Type 06: Time, 6, Button, Line, SPNote, SPLine, Linenr, unkn1, unkn2, (1 or 6)
                //Argument Structure for Type 19: Time, 19, unkn1, unkn2, unkn3, unkn4, unkn5, unkn6, 
                //                                          Button, Line, SPNote, SPLine, Linenr, unkn1, unkn2, (1 or 6)
            time = args[0];
            orgtime = -1;
            orgNote = -1;
            int add = 0;
            if (args[1] == 19)
            {
                add = 6;
                orgNote = 19;
            }
            else if (args[1] == 14)
            {
                add = 3;
                orgNote = 14;
            }
            if (args[2 + add] <= (int)Notes.y || args[2 + add] == (int)Notes.S)
                note = args[2 + add];
            else
            {
                note = args[2 + add] - (int)Notes.X;
                hold = true;
            }
            orgArgs = new int[add];
            for (int i = 0; i < add; i++)
            {
                orgArgs[i] = args[2 + i];
            }
            if (args[3 + add] == 1000) Position = Line.Upper;
            else if (args[3 + add] == -1000) Position = Line.Lower;
            else Position = Line.Middle;

            if (args[4 + add] == 1000) SPNote = SPType.start;
            else if (args[4 + add] == 2000) SPNote = SPType.stop;
            else SPNote = SPType.none;

            SPLine = args[5 + add] != 0;
            unkn1 = args[6 + add];
            unkn2 = args[7 + add];
            LineNr = args[8 + add] / 1000;
        }
        public void AddOrgNote(int note, int[] args)
        {
            orgNote = note;
            orgArgs = args;
        }
        public instruction ToInstruction()
        {
            instruction ins = new instruction();
            ins.time = time;
            ins.args = new List<int>();
            if (orgNote == -1)
            ins.type = 6;
            else
            {
                ins.type = orgNote;
                ins.args.AddRange(orgArgs);
            }
            if (hold)
                ins.args.Add((int)note + (int)Notes.X);

            else ins.args.Add((int)note);
            if (Position == Line.Upper)
                ins.args.Add(1000);
            if (Position == Line.Middle)
                ins.args.Add(0);
            else ins.args.Add(-1000);

            if (SPNote == SPType.start)
                ins.args.Add(1000);
            if (SPNote == SPType.stop)
                ins.args.Add(2000);
            else ins.args.Add(0);

            ins.args.Add(Convert.ToInt32(SPLine) * 1000);
            ins.args.Add(unkn1);
            ins.args.Add(unkn2);
            ins.args.Add(LineNr * 1000);

            ins.args.Add(1);

            return ins;
        }
        public void MakeOffset(int offset)
        {
            orgtime = time;
            time /= offset;
        }
        public void RemoveOffset(int offset, List<Note> rel)
        {
            if (orgtime != -1)
                time = orgtime;
            else
            {
                int ln = -1;
                int lt = 1000;
                for(int i = 0; i < rel.Count; i++)
                {
                    if(rel[i].orgtime != -1 && time - rel[i].time > 0 && time - rel[i].time < lt)
                    {
                        lt = time - rel[i].time;
                        ln = rel[i].orgtime;
                    }
                }
                if (ln == -1)
                    time *= offset;
                else
                    time = ln + (lt * offset);
            }
        }
    }

    public class instruction
    {
        public int type, time, orgtime;
        public List<int> args;
        public instruction() { }
        public instruction(int Type, int Time, List<int> Args) { orgtime = Time; type = Type; time = Time; args = Args; }
        public void Write(BinaryWriter bw)
        {
            bw.Write(time);
            bw.Write(type);
            foreach (int i in args)
                bw.Write(i);
        }
    }

    public class InstructionInterpreterTypeExtra
    {
        public int argNr, argIf, eargL;
        public string[] argName;
        public bool isSpecial;
        public bool isIf(List<int> args)
        {
            if (args.Count > argNr)
                if (args[argNr] == argIf || isSpecial)
                    return true;
            return false;
        }
    }
    public class InstructionInterpreterType
    {
        public int typeNr;
        public string typeName;
        public int argNr;
        public string[] argName;
        public InstructionInterpreterTypeExtra[] extra;
        public bool isMyType(int type) { return type == typeNr; }
        public instruction parse(BinaryReader br, int time, InstructionInterpreterType[] types)
        {
            instruction ins = new instruction(typeNr, time, new List<int>());

            for (int i = 0; i < argNr; i++)
                ins.args.Add(br.ReadInt32());

            foreach (InstructionInterpreterTypeExtra e in extra)
                if (e.isIf(ins.args))
                {
                    if (e.isSpecial)
                    {
                        instruction tmp = new instruction(0, 0, new List<int>());
                        foreach (InstructionInterpreterType t in types)
                            if (t.isMyType(ins.args[e.argNr]))
                                tmp = t.parse(br, time, types);
                        ins.args.AddRange(tmp.args);
                    }
                    else
                    {
                        for (int i = 0; i < e.eargL; i++)
                            ins.args.Add(br.ReadInt32());
                    }
                }

            return ins;
        }
    }
}
