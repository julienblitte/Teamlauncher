using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Teamlauncher
{
    public partial class Teamlauncher : Form
    {
        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;
        protected string configFile;
        protected string editor;
        protected ImageList iconList;
        protected Dictionary<string, RemoteProtocol> protocols;
        protected EditRemoteAccess editDialog;

        public Teamlauncher(bool visible=true)
        {
            InitializeComponent();
            createTrayIcon();

            iconList = new ImageList();
            iconList.Images.Add(Properties.Resources.group);

            protocols = new Dictionary<string, RemoteProtocol>();

            registerProtocol(new Teamviewer());
            registerProtocol(new SSH());
            registerProtocol(new SCP());
            registerProtocol(new HTTP());
            registerProtocol(new HTTP(false));
            registerProtocol(new FTP());
            registerProtocol(new FTP(false));
            registerProtocol(new RDP());
            registerProtocol(new VNC());
            registerProtocol(new Telnet());

            setVisible(visible);
        }

        private void createTrayIcon()
        {
            // Create a simple tray menu with only one item.
            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Exit", OnExit);

            // Create a tray icon. 
            trayIcon = new NotifyIcon();
            trayIcon.Text = "Teamlauncher";
            trayIcon.Icon = Properties.Resources.icon;

            // Add menu to tray icon and show it.
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Click += onClick;
            trayIcon.Visible = true;
        }

        private bool setVisible(bool state)
        {
            ShowInTaskbar = Visible = state;
            if (Visible)
            {
                if (WindowState == FormWindowState.Minimized)
                {
                    WindowState = FormWindowState.Normal;
                }
                Teamlauncher_Shown(this, new EventArgs());
            }
            return Visible;
        }

        private void onClick(object sender, EventArgs e)
        {
            setVisible(!Visible);
        }

        private void OnExit(object sender, EventArgs e)
        {
            Properties.Settings.Default.LocationX = this.Location.X;
            Properties.Settings.Default.LocationY = this.Location.Y;
            Properties.Settings.Default.WindowWidth = this.Width;
            Properties.Settings.Default.WindowHeight = this.Height;
            Properties.Settings.Default.Save();

            setVisible(false);
            trayIcon.Dispose();
            Application.Exit();
        }

        private void loadConfig()
        {
            string appFolder;
            configFile = "teamlauncher.xml";

            appFolder = AppDomain.CurrentDomain.BaseDirectory;
            if (!appFolder.EndsWith("\\"))
            {
                appFolder += "\\";
            }
            configFile = appFolder + "teamlauncher.xml";

            editor = "notepad.exe";
            RegistryKey npp = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\notepad++.exe");
            if (npp != null)
            {
                editor = "notepad++.exe";
            }
        }

        private void Teamlauncher_Load(object sender, EventArgs e)
        {
            loadConfig();
            reloadDatabase();

            if (Properties.Settings.Default.LocationX != 0 || Properties.Settings.Default.LocationY != 0)
                this.Location = new Point(Properties.Settings.Default.LocationX, Properties.Settings.Default.LocationY);

            if (Properties.Settings.Default.WindowWidth != 0 || Properties.Settings.Default.WindowHeight != 0)
            {
                this.Width = Properties.Settings.Default.WindowWidth;
                this.Height = Properties.Settings.Default.WindowHeight;
            }
        }

        private void Teamlauncher_Shown(object sender, EventArgs e)
        {
            serverTreeview.ExpandAll();
            serverTreeview.Focus();
            BringToFront();
            Activate();
        }

        private void registerProtocol(RemoteProtocol rp)
        {
            // populate icon list
            iconList.Images.Add(rp.icon);
            // inform protocol back about icon id
            rp.id = iconList.Images.Count - 1;

            // add protocol to protocol name list
            protocols.Add(rp.name, rp);
        }

        private void reloadDatabase()
        {
            // Assign the ImageList to the TreeView.
            serverTreeview.ImageList = iconList;

            if (File.Exists(configFile))
            {
                XmlDocument xmldoc;
                XmlNode xmlnode;
                FileStream fs;
                    
                // open XML
                fs = new FileStream(configFile, FileMode.Open, FileAccess.Read);
                xmldoc = new XmlDocument();
                xmldoc.Load(fs);

                // root item
                xmlnode = xmldoc.ChildNodes[1];
                serverTreeview.Nodes.Clear();
                serverTreeview.Nodes.Add(new TreeNodeAccess(
                     (xmldoc.DocumentElement.Attributes["name"] != null)?
                     xmldoc.DocumentElement.Attributes["name"].Value:
                     "Teamlauncher"
                    ));
                if (xmldoc.DocumentElement.Attributes["hash"] != null)
                {
                    MasterPassword.getInstance().hash = xmldoc.DocumentElement.Attributes["hash"].Value;
                }

                // launch recursive process
                TreeNodeAccess tNode;
                tNode = (TreeNodeAccess)serverTreeview.Nodes[0];
                AddXmlNode(xmlnode, tNode);
                serverTreeview.ExpandAll();
                fs.Close();
            }
            else
            {
                using (File.Create(configFile))
                {
                    MessageBox.Show(this, "Configuration file " + configFile + " does not exists, created.", "Configuration", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

        }

        private void AddXmlNode(XmlNode inXmlNode, TreeNodeAccess inTreeNode)
        {
            XmlNode currentXMLNode;
            TreeNodeAccess currentTreeNode;
            XmlNodeList nodeList;
            int i = 0;

            if (inXmlNode.HasChildNodes)
            {
                nodeList = inXmlNode.ChildNodes;
                for (i = 0; i <= nodeList.Count - 1; i++)
                {
                    currentXMLNode = inXmlNode.ChildNodes[i];
                    if (currentXMLNode.Attributes != null && currentXMLNode.Attributes["name"] != null)
                    {
                        inTreeNode.Nodes.Add(new TreeNodeAccess(currentXMLNode.Attributes["name"].Value));
                    }
                    else
                    {
                        inTreeNode.Nodes.Add(new TreeNodeAccess("New folder"));
                    }
                    currentTreeNode = (TreeNodeAccess)inTreeNode.Nodes[i];
                    AddXmlNode(currentXMLNode, currentTreeNode);
                }
            }
            else if (inXmlNode.Name == "remote")
            {
                if (inXmlNode.Attributes != null &&
                inXmlNode.Attributes["name"] != null &&
                inXmlNode.Attributes["protocol"] != null)
                {
                    RemoteProtocol rp;

                    inTreeNode.Text = inXmlNode.Attributes["name"].Value;
                    try
                    {
                        rp = protocols[inXmlNode.Attributes["protocol"].Value];
                        inTreeNode.ImageIndex = rp.id;

                        if (inXmlNode.Attributes["host"] != null)
                        {
                            RemoteAccess access = new RemoteAccess();
                            access.login = inXmlNode.Attributes["login"]?.Value;
                            access.host = inXmlNode.Attributes["host"]?.Value;

                            if (!Int32.TryParse(inXmlNode.Attributes["port"]?.Value, out access.port))
                            {
                                access.port = 0;
                            }
                            access.password = inXmlNode.Attributes["password"]?.Value;
                            access.protocol = rp;
                            // TODO: find better way using direclty the constructor of TreeNodeAccess
                            inTreeNode.remoteAccess = access;
                        }
                    }
                    catch (KeyNotFoundException) {
                        MessageBox.Show("Unrecognized protocol '" + inXmlNode.Attributes["protocol"].Value + "'");
                    }
                }
                else
                {
                    inTreeNode.Text = "invalid host";
                }
            }
        }

        private bool connectFromNode(TreeNodeAccess node)
        {
            int paramSet;
            string masterPassword;
            string localPassword;

            RemoteAccess ra;

            if (node.isFolder())
            {
                return false;
            }

            ra = node.remoteAccess;
            paramSet = RemoteProtocol.ParamNone;

            if ((ra.login != null) && (ra.login != "")) paramSet |= RemoteProtocol.ParamLogin;
            if ((ra.host != null) && (ra.host != "")) paramSet |= RemoteProtocol.ParamHost;
            if (ra.port != 0) paramSet |= RemoteProtocol.ParamPort;

            localPassword = "";
            if ((ra.password != null) && (ra.password != ""))
            {
                masterPassword = MasterPassword.getInstance().master;
                if (masterPassword == MasterPassword.NO_MASTER_ENTERED)
                {
                    masterPassword = null;
                    if (MessageBox.Show("No password entered, connect without password?", "No password", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) != DialogResult.Yes)
                    {
                        return true;
                    }
                }
                else if (masterPassword == MasterPassword.NO_MASTER_ENABLED)
                {
                    localPassword = Encoding.UTF8.GetString(Convert.FromBase64String(ra.password));
                }
                else
                {
                    using (Encryption enc = new Encryption(masterPassword))
                    {
                        localPassword = enc.DecryptString(ra.password);
                        if (localPassword == null)
                        {
                            MessageBox.Show("Error decrypting data!\nIs your configuration file corrupted?", "Invalid Password", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return true;
                        }
                        else
                        {
                            paramSet |= RemoteProtocol.ParamPassword;
                        }
                    }
                }

                masterPassword = null;
            }

            try
            {
                ra.protocol.run(ra.login, localPassword, ra.host, ra.port, paramSet);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error running program", "Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return true;
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            TreeNodeAccess node;

            node = (TreeNodeAccess)serverTreeview.SelectedNode;
            if (node == null)
                return;

            connectFromNode(node);
        }

        private void Teamlauncher_FormClosing(object sender, FormClosingEventArgs e)
        {
            setVisible(false);
            e.Cancel = true;

            trayIcon.BalloonTipText ="Teamlauncher is still working...";
            trayIcon.BalloonTipTitle = "Teamlauncher";
            trayIcon.BalloonTipIcon = ToolTipIcon.Info;
            trayIcon.ShowBalloonTip(300);
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
            string[] versionDetail;
            string version, build;

            string protocolList;
            int i;

            protocolList = "";
            i = 0;
            foreach (KeyValuePair<string, RemoteProtocol> item in protocols)
            {
                if (i != 0)
                   protocolList += ",";
                protocolList += item.Key;
                i++;
            }

            versionDetail = this.ProductVersion.Split('.');
            if (versionDetail.Length == 4) // there 4 level of version
            {
                version = versionDetail[0] + "." + versionDetail[1];
                if (versionDetail[2].Length == 4 && versionDetail[3].Length == 4) // subversion looks a build date
                {
                    build = versionDetail[2] + "-" + versionDetail[3].Substring(0, 2) + "-" + versionDetail[3].Substring(2);
                }
                else
                {
                    build = versionDetail[2] + "." + versionDetail[3];
                }
            }
            else
            {
                version = this.ProductVersion;
                build = "unknown";
            }

            MessageBox.Show(this,
                String.Format("{0} {1}.{2}, {3:0000}-{4:00}-{5:00}\n\n{6}\nAll rights reserved.\n\nLoaded protocol modules:\n{7}",
                    versionInfo.ProductName, versionInfo.FileMajorPart, versionInfo.FileMinorPart,
                    versionInfo.ProductBuildPart, (versionInfo.ProductPrivatePart / 100), (versionInfo.ProductPrivatePart % 100),
                    versionInfo.LegalCopyright, protocolList), "About");
        }

        private void configurationFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessStartInfo psi;
            String backupConfigFile;

            backupConfigFile = configFile + "." + DateTime.Now.ToString("yyyy-MM-dd");
            if (!File.Exists(backupConfigFile))
            {
                File.Copy(configFile, backupConfigFile);
            }

            psi = new ProcessStartInfo(editor, String.Format("\"{0}\"", configFile));
            psi.UseShellExecute = true;
            psi.Verb = "runas";
            Process.Start(psi);
        }

        private void addNode(TreeNodeAccess newNode)
        {
            TreeNodeAccess node;

            node = (TreeNodeAccess)serverTreeview.SelectedNode;

            if (node == null) /* no node selected */
            {
                serverTreeview.Nodes[0].Nodes.Add(newNode);
            }
            else if (node.isFolder())  /* folder selected */
            {
                node.Nodes.Add(newNode);
            }
            else
            {
                int pos;

                pos = node.Index + 1;
                node.Parent.Nodes.Insert(pos, newNode);
            }

            serverTreeview.SelectedNode = newNode;
        }

        private void AddNewAccess_Click(object sender, EventArgs e)
        {
            if (editDialog == null)
            {
                editDialog = new EditRemoteAccess(protocols);
            }
            if (editDialog.ShowDialog() == DialogResult.OK)
            {
                TreeNodeAccess newNode;

                newNode = new TreeNodeAccess(editDialog.RemoteDetail, editDialog.RemoteName);
                addNode(newNode);
                saveDatabase();
            }
        }

        private string saveDatabaseSub(TreeNodeAccess currentNode, int level=0)
        {
            RemoteAccess ra;
            String result, indent;

            indent = new String('\t', level);

            if (!currentNode.isFolder())
            {
                ra = currentNode.remoteAccess;
                result = indent;
                result += String.Format("<remote name=\"{0}\" protocol=\"{1}\"", currentNode.Text, ra.protocol.name);
                if ((ra.login != null) && (ra.login != ""))
                {
                    result += String.Format(" login=\"{0}\"", ra.login);
                }
                if ((ra.password != null) && (ra.password != ""))
                {
                    result += String.Format(" password=\"{0}\"", ra.password);
                }
                if ((ra.host != null) && (ra.host != ""))
                {
                    result += String.Format(" host=\"{0}\"", ra.host);
                }
                if (ra.port != 0)
                {
                    result += String.Format(" port=\"{0}\"", ra.port);
                }
                result += " />\n";
            }
            else
            {
                // root, try to save hash
                if ((level == 0) && (MasterPassword.getInstance().hash != null))
                {
                    result = indent + String.Format("<folder name=\"{0}\" hash=\"{1}\">\n",
                        currentNode.Text, MasterPassword.getInstance().hash);
                }
                else
                {
                    result = indent + String.Format("<folder name=\"{0}\">\n", currentNode.Text);
                }

                foreach (TreeNodeAccess subNode in currentNode.Nodes)
                {
                    result += saveDatabaseSub(subNode, level + 1);
                }
                result += indent + "</folder>\n";
            }

            return result;
        }

        private void saveDatabase()
        {
            string content;
            String backupConfigFile;

            backupConfigFile = configFile + "." + DateTime.Now.ToString("yyyy-MM-dd");
            if (!File.Exists(backupConfigFile))
            {
                File.Copy(configFile, backupConfigFile);
            }

            content = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n" +
                saveDatabaseSub((TreeNodeAccess)serverTreeview.Nodes[0]);

            File.WriteAllText(configFile, content);
        }

        private void reloadConfigurationFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            reloadDatabase();
        }

        private void editAccessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNodeAccess node;

            node = (TreeNodeAccess)serverTreeview.SelectedNode;

            if (node == null)
                return;

            if (node.isFolder())
                return;

            if (editDialog == null)
            {
                editDialog = new EditRemoteAccess(protocols);
            }
            if (editDialog.ShowDialog(node.Text, node.remoteAccess) == DialogResult.OK)
            {
                node.Text = editDialog.RemoteName;
                node.remoteAccess = editDialog.RemoteDetail;
                saveDatabase();
            }
        }

        private void serverTreeview_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                TreeNodeAccess node = (TreeNodeAccess)serverTreeview.SelectedNode;
                if (node != null)
                {
                    e.Handled = connectFromNode(node);
                }
            }
        }

        private void addNewFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNodeAccess newNode;

            newNode = new TreeNodeAccess("New folder");

            addNode(newNode);
            saveDatabase();
        }

        private void deleteFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNodeAccess node;

            node = (TreeNodeAccess)serverTreeview.SelectedNode;
            if (node == null)
                return;

            if (node.isRoot()) /* must not be root item */
                return;

            if (MessageBox.Show("Do you really want to delete item \"" + node.Text + "\"?", "Confirm delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if ((node.isFolder()) && (node.Nodes.Count != 0)) /* handle folder subitems */
                {
                    int pos;
                    TreeNode parentNode;
                    DialogResult keepSubItems;

                    keepSubItems = MessageBox.Show("This folder is not empty!\nDo you want to keep subitems?", "Confirm delete subitems", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                    if (keepSubItems == DialogResult.Cancel)
                    {
                        return;
                    }
                    if (keepSubItems == DialogResult.Yes)
                    {
                        pos = node.Index + 1;
                        parentNode = node.Parent;
                        foreach (TreeNode childNode in node.Nodes)
                        {
                            parentNode.Nodes.Insert(pos, (TreeNodeAccess)childNode.Clone());
                            pos++;
                        }
                    }
                }
                node.Remove();
                saveDatabase();
            }
        }

        private void changePassword(TreeNodeAccess currentNode, string oldMaster, string newMaster)
        {
            if (currentNode.isFolder())
            {
                foreach(TreeNodeAccess childNode in currentNode.Nodes)
                {
                    changePassword(childNode, oldMaster, newMaster);
                }
            }
            else
            {
                string localPassword;
                RemoteAccess ra;

                ra = currentNode.remoteAccess;
                if ((ra.password != null) && (ra.password != ""))
                {
                    try
                    {
                        if (oldMaster == null)
                        {
                            localPassword = Encoding.UTF8.GetString(Convert.FromBase64String(ra.password));
                        }
                        else
                        {
                            using (Encryption enc = new Encryption(oldMaster))
                            {
                                localPassword = enc.DecryptString(ra.password);
                                if (localPassword == null)
                                {
                                    ra.password = null;
                                    return;
                                }
                            }
                        }

                        using (Encryption enc = new Encryption(newMaster))
                        {
                            ra.password = enc.EncryptString(localPassword);
                        }
                    }
                    catch (Exception)
                    {
                        ra.password = null;
                    }
                }
            }
        }

        private void changeMasterPasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string currentMaster;
            string newMaster;

            currentMaster = MasterPassword.getInstance().master;
            if (currentMaster == MasterPassword.NO_MASTER_ENTERED)
            {
                return;
            }

            ChangeMaster changeMaster;
            changeMaster = new ChangeMaster();
            if (changeMaster.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            newMaster = changeMaster.newPassword;

            if (currentMaster == MasterPassword.NO_MASTER_ENABLED)
            {
                changePassword((TreeNodeAccess)serverTreeview.Nodes[0], null, newMaster);
                MasterPassword.getInstance().master = newMaster;
                saveDatabase();
            }
            else
            {
                changePassword((TreeNodeAccess)serverTreeview.Nodes[0], currentMaster, newMaster);
                MasterPassword.getInstance().master = newMaster;
                saveDatabase();
            }
        }
    }
}
