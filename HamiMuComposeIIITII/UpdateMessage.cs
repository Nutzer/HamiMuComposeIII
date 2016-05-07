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
    public partial class UpdateMessage : Form
    {
        public UpdateMessage()
        {
            InitializeComponent();
        }
        public UpdateMessage(string v)
        {
            InitializeComponent();
            version = v;
        }

        string version;

        private void UpdateMessage_Load(object sender, EventArgs e)
        {
            label1.Text = "Update Notices for Version " + version;

            
        }
    }
}
