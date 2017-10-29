using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Teamlauncher
{
    public partial class EditRemoteAccess : Form
    {
        private Dictionary<string, RemoteProtocol> protocolList;
        public string RemoteName;
        public RemoteAccess RemoteDetail;
        public int defaultPort
        {
            get
            {
                try
                {
                    return protocolList[protocol.Text].defaultPort;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        public EditRemoteAccess(Dictionary<string, RemoteProtocol> protocolList)
        {
            InitializeComponent();

            this.protocolList = protocolList;
            protocol.Items.AddRange(protocolList.Keys.ToArray<string>());
        }

        private void EditRemoteAccess_FormClosed(object sender, FormClosedEventArgs e)
        {
            password.Text = "";
        }

        private void ok_Click(object sender, EventArgs e)
        {
            string master;

            if ((name.Text == "") || (host.Text == "") || (protocol.SelectedIndex < 0))
            {
                MessageBox.Show("Name, host or protocol cannot be empty", "Error editing remote access", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            RemoteName = name.Text;
            RemoteDetail = new RemoteAccess();
            RemoteDetail.protocol = protocolList[protocol.Text];
            if (password.Text != "")
            {
                master = MasterPassword.getInstance().master;
                if (master == MasterPassword.NO_MASTER_ENABLED)
                {
                    RemoteDetail.password = Convert.ToBase64String(Encoding.UTF8.GetBytes(password.Text));
                }
                else if (master != MasterPassword.NO_MASTER_ENTERED)
                {
                    using (Encryption enc = new Encryption(master))
                    {
                        RemoteDetail.password = enc.EncryptString(password.Text);
                    }
                }
                else // not entered
                {
                    name.Text = "";
                    password.Text = "";
                    DialogResult = DialogResult.Abort;
                    return;
                }
            }
 
            RemoteDetail.host = (host.Text != "" ? host.Text : null);
            RemoteDetail.login = (login.Text != "" ? login.Text :null);
            RemoteDetail.port = (int)port.Value;

            name.Text = "";
            password.Text = "";
            DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        public DialogResult ShowDialog(string inName, RemoteAccess ra)
        {
            name.Text = inName;
            host.Text = ra.host;
            if (ra.login != null)
            {
                login.Text = ra.login;
            }
            port.Value = ra.port;
            portCustom.Checked = (ra.port != ra.protocol.defaultPort);
            protocol.Text = ra.protocol.name;

            if (ra.password != null)
            {
                string master;

                master = MasterPassword.getInstance().master;
                if (master == MasterPassword.NO_MASTER_ENTERED)
                {
                    return DialogResult.Cancel;
                }
                else if (master == MasterPassword.NO_MASTER_ENABLED)
                {
                    password.Text = Encoding.UTF8.GetString(Convert.FromBase64String(ra.password));
                }
                else
                {
                    using (Encryption enc = new Encryption(master))
                    {
                        password.Text = enc.DecryptString(ra.password);
                    }
                }
            }

            return this.ShowDialog();
        }

        private void port_ValueChanged(object sender, EventArgs e)
        {
            if (port.Value == defaultPort)
            {
                portCustom.Checked = false;
                port.ForeColor = Color.Gray;
            }
            else
            {
                portCustom.Checked = true;
                port.ForeColor = Color.Black;
            }
        }

        private void portNotDefault_CheckedChanged(object sender, EventArgs e)
        {
            if (!portCustom.Checked)
            {
                port.Value = defaultPort;
            }
        }

        private void protocol_SelectedIndexChanged(object sender, EventArgs e)
        {
            port.Value = defaultPort;
            // needed because sometimes port do not get changed
            portCustom.Checked = false;
        }

        private void password_Enter(object sender, EventArgs e)
        {
            password.Text = "";
        }
    }
}
