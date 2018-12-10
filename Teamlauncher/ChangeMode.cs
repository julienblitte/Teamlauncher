using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Teamlauncher
{
    public partial class ChangeMode : Form
    {
        public Teamlauncher.networkMode resultMode;
        public string resultServer;

        public ChangeMode(Teamlauncher.networkMode mode, string server)
        {
            InitializeComponent();

            switch (mode)
            {
                case Teamlauncher.networkMode.single:
                    singleMode.Checked = true;
                    serverMode.Checked = false;
                    clientMode.Checked = false;
                    clientBox.Enabled = false;
                    break;
                case Teamlauncher.networkMode.server:
                    singleMode.Checked = false;
                    serverMode.Checked = true;
                    clientMode.Checked = false;
                    clientBox.Enabled = false;
                    break;
                case Teamlauncher.networkMode.client:
                    singleMode.Checked = false;
                    serverMode.Checked = false;
                    clientMode.Checked = true;
                    clientBox.Enabled = true;
                    break;
            }

            clientServer.Text = server;

            resultMode = mode;
            resultServer = server;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (singleMode.Checked)
            {
                resultMode = Teamlauncher.networkMode.single;
            }
            else if (serverMode.Checked)
            {
                resultMode = Teamlauncher.networkMode.server;
            }
            else if (clientMode.Checked)
            {
                resultMode = Teamlauncher.networkMode.client;
            }

            resultServer = clientServer.Text;

            DialogResult = DialogResult.OK;
            Visible = false;
        }

        private void modeChecked(object sender, EventArgs e)
        {
            clientBox.Enabled = clientMode.Checked;
        }
    }
}
