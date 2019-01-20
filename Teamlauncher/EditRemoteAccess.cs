using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Teamlauncher
{
    public partial class EditRemoteAccess : Form
    {
        private Dictionary<string, ProtocolType> protocolList;
        public string RemoteName;
        public RemoteAccess RemoteDetail;
        private int defaultPort;
        private string password;

        public EditRemoteAccess(Dictionary<string, ProtocolType> protocolList)
        {
            InitializeComponent();

            this.protocolList = protocolList;
            protocol.Items.AddRange(protocolList.Keys.ToArray<string>());
        }

        private void EditRemoteAccess_FormClosed(object sender, FormClosedEventArgs e)
        {
            Clear();
        }

        private void ok_Click(object sender, EventArgs e)
        {
            if ((name.Text == "") || (host.Text == "") || (protocol.SelectedIndex < 0))
            {
                MessageBox.Show("Name, host or protocol cannot be empty", "Error editing remote access", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            RemoteName = name.Text;
            RemoteDetail = new RemoteAccess();
            RemoteDetail.protocol = protocolList[protocol.Text];

            RemoteDetail.host = (host.Text != "" ? host.Text : null);
            RemoteDetail.login = (login.Text != "" ? login.Text : null);
            RemoteDetail.password = (password != "" ? password : null);
            RemoteDetail.port = (int)port.Value;
            RemoteDetail.resource = (resource.Text != "" ? resource.Text : null);

            DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        public DialogResult ShowDialog(string inName, RemoteAccess ra)
        {
            protocol.Text = ra.protocol.name;

            name.Text = inName;
            host.Text = (ra.host != null ? ra.host : "");
            login.Text = (ra.login != null ? ra.login : "");
            port.Value = ra.port;
            resource.Text = (ra.resource != null ? ra.resource : "");

            if (ra.password != null)
            {
                password = ra.password;
            }

            return this.ShowDialog();
        }

        private void port_ValueChanged(object sender, EventArgs e)
        {
            if (port.Value == defaultPort)
            {
                port.ForeColor = SystemColors.ControlDark;
            }
            else
            {
                port.ForeColor = SystemColors.WindowText;
            }
        }

        private void protocol_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProtocolType p;

            if (protocol.SelectedIndex < 0)
            {
                return;
            }
            p = protocolList[protocol.Text];

            defaultPort = p.defaultPort;
            port.Value = defaultPort;
            port.ForeColor = SystemColors.ControlDark;

            host.Enabled = ((p.AllowedParameters & ProtocolType.ParamHost) != 0);
            login.Enabled = ((p.AllowedParameters & ProtocolType.ParamLogin) != 0);
            port.Enabled = ((p.AllowedParameters & ProtocolType.ParamPort) != 0);
            port.BackColor = (port.Enabled ? SystemColors.Window : SystemColors.Control);

            passwordButton.Enabled = ((p.AllowedParameters & ProtocolType.ParamPassword) != 0);
            resource.Enabled = ((p.AllowedParameters & ProtocolType.ParamResource) != 0);
        }


        private void EditRemoteAccess_Shown(object sender, EventArgs e)
        {
            cancelButton.Focus();
        }

        private void password_Click(object sender, EventArgs e)
        {
            ChangePassword cp;
            string master;

            master = MasterPassword.getInstance().master;
            if (master == MasterPassword.NO_MASTER_ENTERED)
            {
                return;
            }

            cp = new ChangePassword();
            if (cp.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            if (master == MasterPassword.NO_MASTER_ENABLED)
            {
                password = Convert.ToBase64String(Encoding.UTF8.GetBytes(cp.newPassword));
            }
            else
            {
                using (Encryption enc = new Encryption(master))
                {
                    password = enc.EncryptString(cp.newPassword);
                }
            }
        }

        public void Clear()
        {
            name.Text = "";
            password = null;

            host.Text = "";
            login.Text = "";
            port.Value = 0;
            resource.Text = "";

            protocol.SelectedIndex = -1;
        }
    }
}
