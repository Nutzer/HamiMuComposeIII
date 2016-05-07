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
            ins.orgtime = orgtime;
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
        public int[] getArgs()
        {
            List<int> rargs = new List<int>();
            if (type == 6)
            {
                rargs.Add(args[4]);
                rargs.Add(args[5]);
                rargs.Add(args[6]);
                if (isDouble())
                {
                    rargs.Add(args[11]);
                    rargs.Add(args[12]);
                    rargs.Add(args[13]);
                }
            }
            if (type == 19)
            {
                rargs.Add(args[10]);
                rargs.Add(args[11]);
                rargs.Add(args[12]);
                if (isDouble())
                {
                    rargs.Add(args[17]);
                    rargs.Add(args[18]);
                    rargs.Add(args[19]);
                }
            }
            return rargs.ToArray();
        }
        public void setArgs(int[] tp)
        {
            if (type == 6)
            {
                args[4] = tp[0];
                args[5] = tp[1];
                args[6] = tp[2];
                if (isDouble() && tp.Length > 3)
                {
                    args[11] = tp[3];
                    args[12] = tp[4];
                    args[13] = tp[5];
                }
            }
            if (type == 19)
            {
                args[10] = tp[0];
                args[11] = tp[1];
                args[12] = tp[2];
                if (isDouble() && tp.Length > 3)
                {
                    args[17] = tp[3];
                    args[18] = tp[4];
                    args[19] = tp[5];
                }
            }
        }
        public bool isArgs()
        {
            if (type == 6) return (args[4] != 0) || (args[5] != 0) || (args[6] != 0);
            if (type == 19) return (args[10] != 0) || (args[11] != 0) || (args[12] != 0);
            return false;
        }
        public int getLStart()
        {
            if (type == 6) return args[6] / 1000;
            if (type == 19) return args[12] / 1000;
            return 0;
        }
        public int getLn()
        {
            if (type == 6) return args[1];
            if (type == 19) return args[7];
            return 0;
        }
        public int getSp()
        {
            if (type == 6) return args[2];
            if (type == 19) return args[8];
            return 0;
        }
        public int getSpln()
        {
            if (type == 6) return args[3];
            if (type == 19) return args[9];
            return 0;
        }
        public bool isDouble()
        {
            if (type == 6) return args.Count > 9;
            if (type == 19) return args.Count > 12;
            return false;
        }
        public void setLn(int val)
        {
            if (type == 6) args[1] = val;
            if (type == 19) args[7] = val;
        }
        public void setSp(int val)
        {
            if (type == 6) args[2] = val;
            if (type == 19) args[8] = val;
        }
        public void setSpln(int val)
        {
            if (type == 6) args[3] = val;
            if (type == 19) args[9] = val;
        }
        public int getTime()
        {
            return time;
        }
        public int getNote()
        {
            if (type == 6) return args[0];
            return args[6];
        }
        public int getSecondNote()
        {
            if (!isDouble()) return -1;
            if (type == 6) return args[8];
            return args[14];
        }
        public void setNote(int nt)
        {
            if (type == 6) args[0] = nt;
            else args[6] = nt;
        }
        public void setSecondNote(int nt)
        {
            if (!isDouble()) return;
            if (type == 6) args[8] = nt;
            else args[14] = nt;
        }
        public int getPos()
        {
            if (type == 6) return args[1];
            return args[7];
        }
        public void setPos(int nr)
        {
            if (type == 6) args[1] = nr;
            else args[7] = nr;
        }
        public bool isSingleTop()
        {
            return !isDouble() && getPos() != 0;
        }
        public bool IsSingleBottom()
        {
            return !isDouble() && getPos() == 0;
        }
        public void ConvertToSingle(bool topper)
        {
            if (topper && isDouble())
            {
                for (int i = 0; i < 6; i++)
                    args[i] = args[i + 8];
                setPos(1000);
            }
            List<int> tmp = new List<int>(args);
            args = new List<int>();
            for (int i = 0; i < 7; i++)
                args.Add(tmp[i]);
            args.Add(1);
        }
        public void ConvertToDouble(int newNote)
        {
            args[7] = 6;
            if (isSingleTop())
            {
                for (int i = 0; i < 7; i++)
                {
                    args.Add(args[i]);
                    args[i] = 0;
                }
                setNote(newNote);
            }
            else
            {
                for (int i = 0; i < 7; i++)
                {
                    args.Add(0);
                }
                setSecondNote(newNote);
            }
        }
        public void ConvertTo6()
        {
            List<int> buf = new List<int>(args);
            args = new List<int>();
            for (int i = 0; i < buf.Count + 6; i++)
                args.Add(buf[i + 6]);
            type = 6;
        }
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
    public class OldParser
    {
        public string retO;

        public instruction parse_ins(BinaryReader br)
        {
            instruction ins = new instruction();
            ins.time = br.ReadInt32();
            ins.type = br.ReadInt32();
            ins.orgtime = ins.time;
            ins.args = new List<int>();
            int[] recTypes = { 5, 6, 14, 18, 19, 20, 22, 24, 32, 52 };

            switch (ins.type)
            {
                case 5:
                    for (int i = 0; i < 3; i++)
                        ins.args.Add(br.ReadInt32());
                    break;
                case 6:
                    for (int i = 0; i < 8; i++)
                        ins.args.Add(br.ReadInt32());
                    if (ins.args[7] == 6)
                        for (int i = 0; i < 8; i++)
                            ins.args.Add(br.ReadInt32());
                    break;
                case 14:
                    for (int i = 0; i < 3; i++)
                        ins.args.Add(br.ReadInt32());
                    switch (ins.args[2])
                    {
                        case 4:
                            ins.args.Add(br.ReadInt32()); //TODO
                            break;
                        case 5:
                            for (int i = 0; i < 3; i++)
                                ins.args.Add(br.ReadInt32());
                            if (ins.args[5] == 22)
                            {
                                for (int i = 0; i < 3; i++)
                                    ins.args.Add(br.ReadInt32());
                                if (ins.args[8] == 4)
                                    for (int i = 0; i < 2; i++)
                                        ins.args.Add(br.ReadInt32());
                            }
                            break;
                        case 18:
                            for (int i = 0; i < 4; i++)
                                ins.args.Add(br.ReadInt32());
                            if (ins.args[6] != 1)
                                for (int i = 0; i < 6; i++)
                                    ins.args.Add(br.ReadInt32());
                            break;
                        case 22:
                            for (int i = 0; i < 3; i++)
                                ins.args.Add(br.ReadInt32());
                            if (ins.args[4] == 4)
                                for (int i = 0; i < 2; i++)
                                    ins.args.Add(br.ReadInt32());
                            break;
                        case 24:
                            for (int i = 0; i < 3; i++)
                                ins.args.Add(br.ReadInt32());
                            break;
                        case 32:
                            ins.args.Add(br.ReadInt32());
                            break;
                        case 52:
                            for (int i = 0; i < 7; i++)
                                ins.args.Add(br.ReadInt32());
                            break;
                    }
                    break;
                case 18:
                    for (int i = 0; i < 4; i++)
                        ins.args.Add(br.ReadInt32());
                    if (ins.args[3] != 1)
                        for (int i = 0; i < 6; i++)
                            ins.args.Add(br.ReadInt32());
                    break;
                case 19:
                    for (int i = 0; i < 6; i++)
                        ins.args.Add(br.ReadInt32());
                    if (ins.args[5] == 6)
                    {
                        for (int i = 0; i < 8; i++)
                            ins.args.Add(br.ReadInt32());
                        if (ins.args[13] == 6)
                            for (int i = 0; i < 8; i++)
                                ins.args.Add(br.ReadInt32());
                    }
                    break;
                case 20:
                    for (int i = 0; i < 4; i++)
                        ins.args.Add(br.ReadInt32());

                    break;
                case 22:
                    for (int i = 0; i < 3; i++)
                        ins.args.Add(br.ReadInt32());
                    if (ins.args[1] == 4)
                        for (int i = 0; i < 2; i++)
                            ins.args.Add(br.ReadInt32());
                    break;
                case 24:
                    for (int i = 0; i < 3; i++)
                        ins.args.Add(br.ReadInt32());
                    break;
                case 32:
                    ins.args.Add(br.ReadInt32());
                    break;
                case 52:
                    for (int i = 0; i < 7; i++)
                        ins.args.Add(br.ReadInt32());
                    break;
            }
            return ins;
        }

    }
}
