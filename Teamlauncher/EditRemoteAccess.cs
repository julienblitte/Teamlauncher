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
    public partial class EditRemoteAccess : Form
    {
        private Dictionary<string, RemoteProtocol> protocolList;
        public string RemoteName;
        public RemoteAccess RemoteDetail;

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

        private void button1_Click(object sender, EventArgs e)
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
                if (master != null)
                {
                    using (Encryption enc = new Encryption(master))
                    {
                        RemoteDetail.password = enc.EncryptString(password.Text);
                    }
                }
                else
                {
                    name.Text = "";
                    password.Text = "";
                    DialogResult = DialogResult.Abort;
                    return;
                }
            }
            if (host.Text != "")
            {
                RemoteDetail.host = host.Text;
            }
            if (login.Text != "")
            {
                RemoteDetail.login = login.Text;
            }
            if (port.Value != 0)
            {
                RemoteDetail.port = (int)port.Value;
            }
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
            BackColor = (port.Value == 0 ? SystemColors.ButtonFace:SystemColors.Window);
        }
    }
}
