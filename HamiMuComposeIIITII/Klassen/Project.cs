using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace HamiMuComposeIIITII
{
    public struct ProjectContent
    {
        public int ProjectVersion;
        public string dscPath, projectPath, musicPath;
        public Parse dscParsed, dscBackup;
        public History history;
        public float soundOffs, soundAdd;
    }
    public class Project
    {
        ProjectContent pc;
        public bool isOpen;
        public Sound s;
        public Project()
        {
            pc = new ProjectContent();
            isOpen = false;
        }
        public void Save(string pathas)
        {
            if (pathas != "") pc.projectPath = pathas;
            using (StreamWriter file = new StreamWriter(pc.projectPath))
            {
                file.Write(JsonConvert.SerializeObject(pc));
            }
        }
        public string getPath()
        {
            return pc.projectPath;
        }
        public bool Open(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                if (sr.Peek() == '{')
                {
                    try
                    {
                        pc = JsonConvert.DeserializeObject<ProjectContent>(sr.ReadToEnd());
                        if (pc.ProjectVersion > 1)
                        {
                            MessageBox.Show("This Project Verision is not Supported. Please Update.");
                            return false;
                        }
                        if(pc.musicPath != "")
                        {
                            s = new Sound();
                            s.loadSound(pc.musicPath);
                            s.offs = pc.soundOffs;
                            s.add = pc.soundAdd;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error while Parsing Project file. Please try upgrading or a different Project.\r\n"+ex.Message);
                    }
                }
                else
                {   //PROJECT VERSION < 1
                    MessageBox.Show("This Project Verision is not Supported. Please Downgrade.");
                    return false;
                }
                isOpen = true;
                return true;
            }
        }
        public void Create(string prPath, string dscPath)
        {
            pc = new ProjectContent();
            pc.projectPath = prPath;
            pc.dscPath = dscPath;
            pc.ProjectVersion = 1;
            pc.dscParsed = new Parse(dscPath);
            pc.dscBackup = new Parse(dscPath);
            pc.history = new History();
            pc.musicPath = "";
            isOpen = true;
        }
        public void AddMusic(string path, float add, float offs)
        {
            pc.musicPath = path;
            pc.soundAdd = add;
            pc.soundOffs = offs;
            s = new Sound();
            s.loadSound(pc.musicPath);
            s.offs = pc.soundOffs;
            s.add = pc.soundAdd;
        }
        public void Export(string pathas)
        {
            if (pathas != "") pc.dscPath = pathas;
            using (BinaryWriter bw = new BinaryWriter(new StreamWriter(pc.dscPath).BaseStream))
            {
                pc.dscParsed.Write(bw);
            }
        }
        public Parse getDsc()
        {
            return pc.dscParsed;
        }
        public History getHistory()
        {
            return pc.history;
        }
    }
}
