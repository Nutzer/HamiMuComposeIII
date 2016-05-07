using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HamiMuComposeIIITII
{
    public enum ChangeType { Add, Remove, Move };
    public struct Change
    {
        public ChangeType type;
        public Note n1, n2;
    }
    public  class History
    {
        List<Change> changes;
        int Position;
        public History()
        {
            changes = new List<Change>();
            Position = -1;
        }
        public void removeAlLatter()
        {
            for (int i = Position+1; i >= 0 && i < changes.Count; i++)
                changes.RemoveAt(i);
        }
        public void NoteRemoved(Note n)
        {
            removeAlLatter();
            Change c = new Change();
            c.type = ChangeType.Move;
            c.n1 = n;
            changes.Add(c);
            Position++;
        }
        public void NoteAdded(Note n)
        {
            removeAlLatter();
            if (changes.Count > 1 && changes.Count > Position && changes[Position].type == ChangeType.Move)
            {
                Change c = changes[Position];
                c.n2 = n;
                changes[Position] = c;
            }
            else
            {
                Change c = new Change();
                c.n1 = n;
                c.type = ChangeType.Add;
                changes.Add(c);
                Position++;
            }
        }
        public void NoteAbschluss()
        {
            removeAlLatter();
            //if (changes[Position].type == ChangeType.Move)
            {
                Change c = changes[Position];
                c.type = ChangeType.Remove;
                changes[Position] = c;
            }
        }
        public void undo(Project p)
        {
            if (Position < 0) return;

            Change c = changes[Position];
            switch (c.type)
            {
                case ChangeType.Add:
                    p.getDsc().DeleteNoteAtLine(c.n1.Position, c.n1.time);
                    break;
                case ChangeType.Remove:
                    p.getDsc().AddNote(c.n1);
                    break;
                case ChangeType.Move:
                    p.getDsc().DeleteNoteAtLine(c.n2.Position, c.n2.time);
                    p.getDsc().AddNote(c.n1);
                    break;
            }
            Position--;
        }
        public void redo(Project p)
        {
            if (Position >= changes.Count-1) return;
            Position++;

            Change c = changes[Position];
            switch (c.type)
            {
                case ChangeType.Remove:
                    p.getDsc().DeleteNoteAtLine(c.n1.Position, c.n1.time);
                    break;
                case ChangeType.Add:
                    p.getDsc().AddNote(c.n1);
                    break;
                case ChangeType.Move:
                    p.getDsc().DeleteNoteAtLine(c.n1.Position, c.n1.time);
                    p.getDsc().AddNote(c.n2);
                    break;
            }
        }
    }
}
