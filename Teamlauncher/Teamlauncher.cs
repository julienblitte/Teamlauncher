using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using Teamlauncher.Protocol;

namespace Teamlauncher
{
    public partial class Teamlauncher : Form
    {
        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;
        private MenuItem connectMenu;
        protected string databaseFile;
        protected string editor;
        protected ImageList iconList;
        protected Dictionary<string, ProtocolType> protocols;
        protected EditRemoteAccess editDialog;

        private const int STYLE_EFFECT_BORDER = 16;

        public enum networkMode { single, client, server };
        private networkMode currentMode;
        private string serverAddress;
        protected int serverPort;

        protected bool debug;
        private WebServer webserver;

        private const int MAX_BACKUP_KEEY_DAYS = 7;

        private string foldingState;

        public Teamlauncher(bool visible = true)
        {
            Trace.WriteLine("Teamlauncher.Teamlauncher()");

            InitializeComponent();
            createTrayIcon();

            iconList = new ImageList();
            iconList.Images.Add(Properties.Resources.group);

            // load configuration from registry
            using (RegistryConfig reg = new RegistryConfig())
            {
                debug = reg.readBool(RegistryConfig.REGISTRY_KEY_DEBUG);

                editor = reg.readString(RegistryConfig.REGISTRY_KEY_EDITOR);
                if (editor == "")
                {
                    // try to get better editor than notepad (notpead++ only for now)
                    editor = "notepad.exe";
                    RegistryKey npp = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\notepad++.exe");
                    if (npp != null)
                    {
                        editor = "notepad++.exe";
                    }
                    reg.writeString(RegistryConfig.REGISTRY_KEY_EDITOR, editor);
                }

                databaseFile = reg.readString(RegistryConfig.REGISTRY_KEY_DATABASE);
                // make sure we actually have a database file
                if (databaseFile == "")
                {
                    // should be in roaming app data 
                    databaseFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "teamlauncher.xml");

                    var programFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "teamlauncher.xml");

                    if (File.Exists(databaseFile))
                    {
                        reg.writeString(RegistryConfig.REGISTRY_KEY_DATABASE, databaseFile);
                    }
                    else if (File.Exists(programFile))
                    {
                        Trace.WriteLine(String.Format("Teamlauncher(): Trying to migrate {0} to {1}", programFile, databaseFile));
                        // if no database, move the one in application folder
                        try
                        {
                            File.Copy(programFile, databaseFile);
                            if (File.Exists(databaseFile))
                            {
                                reg.writeString(RegistryConfig.REGISTRY_KEY_DATABASE, databaseFile);
                            }
                            else
                            {
                                Trace.WriteLine(String.Format("Teamlauncher(): Error migrating file, rolling back to {0}.", programFile));
                                databaseFile = programFile;
                            }
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine(String.Format("Teamlauncher(): Error happened moving database file from {0} to {1} failed.", programFile, databaseFile));
                            MessageBox.Show(ex.ToString());
                        }
                    }
                }

                currentMode = (networkMode)reg.readInteger(RegistryConfig.REGISTRY_KEY_MODE);

                serverAddress = reg.readString(RegistryConfig.REGISTRY_KEY_SERVER);
                serverPort = reg.readInteger(RegistryConfig.REGISTRY_KEY_PORT);
                if (serverPort == 0)
                    serverPort = 0x544C; // 21580
            }

            protocols = new Dictionary<string, ProtocolType>();

            registerProtocol(new ProtoTeamviewer());
            registerProtocol(new ProtoSSH());
            registerProtocol(new ProtoSCP());
            registerProtocol(new ProtoHTTP());
            registerProtocol(new ProtoHTTP(false));
            registerProtocol(new ProtoFTP());
            registerProtocol(new ProtoFTP(false));
            registerProtocol(new ProtoRDP());
            registerProtocol(new ProtoVNC());
            registerProtocol(new ProtocolTelnet());
            registerProtocol(new ProtoAnyDesk());
            registerProtocol(new ProtoSerial());
            registerProtocol(new ProtoRTSP());

            if (debug)
            {
                Trace.WriteLine("Debug mode is on ");
                registerProtocol(new ProtoDebug());
            }

            webserver = new WebServer(IPAddress.Parse("0.0.0.0"), serverPort, databaseFile);
            if (currentMode == networkMode.server)
            {                
                webserver.start();
            }

            MasterPassword.getInstance().onCacheChanged += OnPasswordInCache;

            Visible = visible;
        }

        private void createTrayIcon()
        {
            connectMenu = new MenuItem("Connect to");

            // Create a simple tray menu with only one item.
            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Show", onTrayMenuShow);
            trayMenu.MenuItems.Add("Exit", onExit);

            // Create a tray icon. 
            trayIcon = new NotifyIcon();
            trayIcon.Text = "Teamlauncher";
            trayIcon.Icon = Properties.Resources.icon;

            // Add menu to tray icon and show it.
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Click += onTrayIconClick;
            trayIcon.Visible = true;
        }

        private void onTrayMenuShow(object sender, EventArgs e)
        {
            if (Visible)
            {
                Teamlauncher_VisibleChanged(sender, e);
            }
            else
            {
                Visible = true;
            }
        }

        private void onTrayIconClick(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;

            if (me.Button == MouseButtons.Left)
            {
                Visible = !Visible;
            }
        }

        public Action<bool> OnPasswordInCache(bool passwordCached)
        {
            Text = Text.Replace("*", "");
            if (passwordCached)
            {
                this.Text += "*";
            }

            return null;
        }

        private void onExit(object sender, EventArgs e)
        {
            exit();
        }

        public void exit()
        {
            if (Visible)
            {
                using (RegistryConfig reg = new RegistryConfig())
                {
                    reg.writeInteger(RegistryConfig.REGISTRY_KEY_LOCATION_X, this.Location.X);
                    reg.writeInteger(RegistryConfig.REGISTRY_KEY_LOCATION_Y, this.Location.Y);
                    reg.writeInteger(RegistryConfig.REGISTRY_KEY_WIN_WIDTH, this.Width);
                    reg.writeInteger(RegistryConfig.REGISTRY_KEY_WIN_HEIGHT, this.Height);

                    foldingState = serializeFoldingState();
                    if (foldingState != "")
                    {
                        reg.writeString(RegistryConfig.REGISTRY_KEY_FOLDING, foldingState);
                    }
                }
            }

            trayIcon.Visible = false; // must be there, else trayicon stay
            trayIcon.Dispose();
            Environment.Exit(0);
        }

        private int onMonitor(Point pt)
        {
            for (int i = 0; i < Screen.AllScreens.Length; i++)
            {
                Rectangle monitor;
                
                monitor = Screen.AllScreens[i].Bounds;

                if ((monitor.Left <= pt.X) && (pt.X <= monitor.Right) &&
                    (monitor.Top <= pt.Y) && (pt.Y <= monitor.Bottom))
                {
                    return i + 1;
                }
            }
            return 0;
        }

        private void Teamlauncher_Load(object sender, EventArgs e)
        {
            int x, y, h, w;

            Trace.WriteLine("Teamlauncher.Teamlauncher_Load()");

            // load configuration from registry
            using (RegistryConfig reg = new RegistryConfig())
            {
                x = reg.readInteger(RegistryConfig.REGISTRY_KEY_LOCATION_X);
                y = reg.readInteger(RegistryConfig.REGISTRY_KEY_LOCATION_Y);
                h = reg.readInteger(RegistryConfig.REGISTRY_KEY_WIN_HEIGHT);
                w = reg.readInteger(RegistryConfig.REGISTRY_KEY_WIN_WIDTH);

                foldingState = reg.readString(RegistryConfig.REGISTRY_KEY_FOLDING);
            }

            reloadDatabase();
            if (foldingState != "")
            {
                restoreFoldingState(foldingState);
            }

            trayMenu.MenuItems.Add(0, connectMenu);

            // try to restore window position
            if (h > STYLE_EFFECT_BORDER && w > STYLE_EFFECT_BORDER)
            {
                Point topLeft, bottomRight;
                int monitor1, monitor2;

                Trace.WriteLine(String.Format("Teamlauncher_Load(): trying restore window position to [({0},{1}),({2},{3})]",
                    x, y, x + w, y + h));


                topLeft = new Point(x + STYLE_EFFECT_BORDER, y + STYLE_EFFECT_BORDER);
                bottomRight = new Point(x + w -STYLE_EFFECT_BORDER, y + h -STYLE_EFFECT_BORDER);

                monitor1 = onMonitor(topLeft);
                monitor2 = onMonitor(bottomRight);

                Trace.WriteLine(String.Format("Teamlauncher_Load(): window top left ({0},{1}) is on monitor #{2}", x, y, monitor1));
                Trace.WriteLine(String.Format("Teamlauncher_Load(): window bottom right ({0},{1}) is on monitor #{2}", x + w, y + h, monitor2));

                if ((monitor1 == monitor2) && (monitor1 != 0))
                {
                    this.Location = new Point(x, y);
                    this.Width = w;
                    this.Height = h;
                }
            }
        }

        private void registerProtocol(ProtocolType rp)
        {
            // populate icon list
            iconList.Images.Add(rp.icon);
            // inform protocol back about icon id
            rp.id = iconList.Images.Count - 1;

            // add protocol to protocol name list
            protocols.Add(rp.name, rp);
        }

        public static string SendResponse(HttpListenerRequest request)
        {
            return string.Format("<HTML><BODY>My web page.<br>{0}</BODY></HTML>", DateTime.Now);
        }

        private void reloadDatabase()
        {
            string foldingBackup;

            Trace.WriteLine("Teamlauncher.reloadDatabase()");

            foldingBackup = foldingState;

            // Assign the ImageList to the TreeView.
            serverTreeview.ImageList = iconList;

            // if client mode, download remote file
            if (currentMode == networkMode.client)
            {
                string downloadFile;
                long downloadSize;

                using (var client = new WebClient())
                {

                    downloadFile = databaseFile + ".tmp";
                    //client.Credentials = new NetworkCredential("teamlauncher", password);
                    try
                    {
                        client.DownloadFile(String.Format("http://{0}:{1}/", serverAddress, serverPort), downloadFile);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        if (File.Exists(downloadFile))
                        {
                            File.Delete(downloadFile);
                        }
                    }
                }

                if (File.Exists(downloadFile))
                {
                    FileInfo fi;

                    fi = new FileInfo(downloadFile);
                    downloadSize = (fi != null ? fi.Length : 0);

                    if (downloadSize == 0)
                    {
                        File.Delete(downloadFile);
                    }
                    else
                    {
                        backupDatabase();
                        if (File.Exists(databaseFile))
                        {
                            File.Delete(databaseFile);
                        }
                        File.Move(downloadFile, databaseFile);
                    }
                }
            }

            if (File.Exists(databaseFile))
            {
                XmlDocument xmldoc;
                XmlNode xmlnode;
                FileStream fs;

                // open XML
                fs = new FileStream(databaseFile, FileMode.Open, FileAccess.Read);
                xmldoc = new XmlDocument();
                xmldoc.Load(fs);

                // root item
                xmlnode = xmldoc.ChildNodes[1];
                serverTreeview.Nodes.Clear();
                serverTreeview.Nodes.Add(new TreeNodeAccess(
                     (xmldoc.DocumentElement.Attributes["name"] != null) ?
                     xmldoc.DocumentElement.Attributes["name"].Value :
                     "Teamlauncher"
                    ));
                if (xmldoc.DocumentElement.Attributes["hash"] != null)
                {
                    MasterPassword.getInstance().hash = xmldoc.DocumentElement.Attributes["hash"].Value;
                }

                // launch recursive process
                TreeNodeAccess tNode;
                tNode = (TreeNodeAccess)serverTreeview.Nodes[0];
                addXmlNode(xmlnode, tNode);
                serverTreeview.ExpandAll();
                fs.Close();

                // build connect menu
                connectMenu.MenuItems.Clear();
                populateConnectMenu(connectMenu, tNode);
            }
            else
            {
                Trace.WriteLine("reloadDatabase(): Configuration file " + databaseFile + " does not exists, will try to create new one.");
                try
                {
                    File.WriteAllText(databaseFile, "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n<folder name=\"servers\">\n</folder>");
                    Trace.WriteLine("reloadDatabase(): Configuration file " + databaseFile + " created.");
                }
                catch(Exception ex)
                {
                    Trace.WriteLine("reloadDatabase(): Error creating configuration file: " + ex.ToString());
                }
            }

            foldingState = foldingBackup;
        }

        private void addXmlNode(XmlNode inXmlNode, TreeNodeAccess inTreeNode)
        {
            XmlNode currentXMLNode;
            TreeNodeAccess currentTreeNode;
            XmlNodeList nodeList;
            int i = 0;

            if (inXmlNode.HasChildNodes)
            {
                TreeNodeAccess nodeToAdd;

                nodeList = inXmlNode.ChildNodes;
                for (i = 0; i <= nodeList.Count - 1; i++)
                {
                    currentXMLNode = inXmlNode.ChildNodes[i];
                    if (currentXMLNode.Attributes != null && currentXMLNode.Attributes["name"] != null)
                    {
                        nodeToAdd = new TreeNodeAccess(currentXMLNode.Attributes["name"].Value);
                    }
                    else
                    {
                        nodeToAdd = new TreeNodeAccess("New folder");
                    }
                    nodeToAdd.ContextMenuStrip = folderMenuStrip;
                    inTreeNode.Nodes.Add(nodeToAdd);
                    currentTreeNode = (TreeNodeAccess)inTreeNode.Nodes[i];
                    addXmlNode(currentXMLNode, currentTreeNode);
                }
            }
            else if (inXmlNode.Name == "remote")
            {
                if (inXmlNode.Attributes != null &&
                inXmlNode.Attributes["name"] != null &&
                inXmlNode.Attributes["protocol"] != null)
                {
                    ProtocolType rp;

                    inTreeNode.Text = inXmlNode.Attributes["name"].Value;
                    inTreeNode.ContextMenuStrip = remoteMenuStrip;
                    try
                    {
                        if (inXmlNode.Attributes["protocol"].Value == "sftp") // renamed protocol backward compatibility
                        {
                            rp = protocols["ftps"];
                        }
                        else
                        {
                            rp = protocols[inXmlNode.Attributes["protocol"].Value];
                        }
                        inTreeNode.ImageIndex = rp.id;

                        if (inXmlNode.Attributes["host"] != null)
                        {
                            /* Do we need to use WebUtility.HtmlDecode to decode? */
                            RemoteAccess access = new RemoteAccess();
                            access.login = inXmlNode.Attributes["login"]?.Value;
                            access.host = inXmlNode.Attributes["host"]?.Value;

                            if (!Int32.TryParse(inXmlNode.Attributes["port"]?.Value, out access.port))
                            {
                                access.port = rp.defaultPort;
                            }
                            access.password = inXmlNode.Attributes["password"]?.Value;
                            access.protocol = rp;
                            access.resource = inXmlNode.Attributes["resource"]?.Value;

                            // TODO: find better way using direclty the constructor of TreeNodeAccess
                            inTreeNode.remoteAccess = access;
                        }
                    }
                    catch (KeyNotFoundException)
                    {
                        Trace.WriteLine("addXmlNode(): Unrecognized protocol '" + inXmlNode.Attributes["protocol"].Value + "'");
                    }
                }
                else
                {
                    inTreeNode.Text = "invalid host";
                }
            }
        }

        private string serializeFoldingState()
        {
            string result;

            Trace.WriteLine("Teamlauncher.serializeFoldingState()");

            if (serverTreeview.Nodes.Count == 0)
            {
                Trace.WriteLine("serializeFoldingState(): empty database");
                return "";
            }

            result = "";
            foreach (TreeNodeAccess ta in (TreeNodeAccess)serverTreeview.Nodes[0])
            {
                if (ta.isFolder())
                {
                    result += (ta.IsExpanded ? "+" : "-");
                }
            }

            Trace.WriteLine("serializeFoldingState(): folding state is " + result);
            return result;
        }

        private void restoreFoldingState(string state)
        {
            int i;

            Trace.WriteLine("restoreFoldingState(): restoring folding " + state);

            if (serverTreeview.Nodes.Count == 0)
            {
                Trace.WriteLine("restoreFoldingState(): empty database");
                return;
            }

            i = 0;
            foreach (TreeNodeAccess ta in (TreeNodeAccess)serverTreeview.Nodes[0])
            {
                if (ta.isFolder())
                {
                    if (i >= state.Length)
                    {
                        break;
                    }

                    if (state[i] == '+')
                    {
                        ta.Expand();
                    }
                    else
                    {
                        ta.Collapse();
                    }
                    i++;
                }
            }
        }

        private bool connectFromNode(TreeNodeAccess node, bool avoidPassword = false)
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
            paramSet = ProtocolType.ParamNone;

            if ((ra.login != null) && (ra.login != "")) paramSet |= ProtocolType.ParamLogin;
            if ((ra.host != null) && (ra.host != "")) paramSet |= ProtocolType.ParamHost;
            if (ra.port != ra.protocol.defaultPort) paramSet |= ProtocolType.ParamPort;
            if ((ra.resource != null) && (ra.resource != "")) paramSet |= ProtocolType.ParamResource;

            localPassword = "";
            if ((ra.password != null) && (ra.password != "") && !avoidPassword)
            {
                masterPassword = MasterPassword.getInstance().master;
                if (masterPassword == MasterPassword.NO_MASTER_ENTERED)
                {
                    masterPassword = null;
                    return true; // cancelled by user
                }
                else if (masterPassword == MasterPassword.NO_MASTER_ENABLED)
                {
                    localPassword = Encoding.UTF8.GetString(Convert.FromBase64String(ra.password));
                    paramSet |= ProtocolType.ParamPassword;
                }
                else
                {
                    using (Encryption enc = new Encryption(masterPassword))
                    {
                        localPassword = enc.DecryptString(ra.password);
                        if (localPassword == null)
                        {
                            MessageBox.Show("Error decrypting data, your configuration is probably file corrupted?", "Invalid Password", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return true;
                        }
                        else
                        {
                            paramSet |= ProtocolType.ParamPassword;
                        }
                    }
                }

                masterPassword = null;
            }

            try
            {
                ra.protocol.run(paramSet, ra.login, localPassword, ra.host, ra.port, ra.resource);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error running program", "Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return true;
        }

        private void generateConnectMenu()
        {
            if (serverTreeview.Nodes.Count > 0)
            {
                // build connect menu
                connectMenu.MenuItems.Clear();
                populateConnectMenu(connectMenu, (TreeNodeAccess)serverTreeview.Nodes[0]);
            }
        }

        private void populateConnectMenu(MenuItem currentMenu, TreeNodeAccess currentNode)
        {
            if (!currentNode.isFolder())
            {
                currentMenu.MenuItems.Add(currentNode.Text, (object sender, EventArgs e) => { connectFromNode(currentNode); });
            }
            else
            {
                int count;

                count = currentMenu.MenuItems.Count;
                if (count > 0)
                {
                    if (currentMenu.MenuItems[count-1].Text != "-")
                    {
                        currentMenu.MenuItems.Add("-");
                        count++;
                    }
                }
                currentMenu.MenuItems.Add(currentNode.Text);
                count++;
                currentMenu.MenuItems[count-1].Enabled = false;
                foreach (TreeNodeAccess subNode in currentNode.Nodes)
                {
                    populateConnectMenu(currentMenu, subNode);
                }
            }
        }

        private void Teamlauncher_FormClosing(object sender, FormClosingEventArgs e)
        {
            using (RegistryConfig reg = new RegistryConfig())
            {
                if (WindowState != FormWindowState.Minimized)
                {
                    reg.writeInteger(RegistryConfig.REGISTRY_KEY_LOCATION_X, this.Location.X);
                    reg.writeInteger(RegistryConfig.REGISTRY_KEY_LOCATION_Y, this.Location.Y);
                    reg.writeInteger(RegistryConfig.REGISTRY_KEY_WIN_WIDTH, this.Width);
                    reg.writeInteger(RegistryConfig.REGISTRY_KEY_WIN_HEIGHT, this.Height);
                }

                foldingState = serializeFoldingState();
                reg.writeString(RegistryConfig.REGISTRY_KEY_FOLDING, foldingState);

                Visible = false;
                e.Cancel = true;

                if (!reg.readBool(RegistryConfig.REGISTRY_KEY_CLOSETIP))
                {
                    trayIcon.BalloonTipText = "Teamlauncher is still working...";
                    trayIcon.BalloonTipTitle = "Teamlauncher";
                    trayIcon.BalloonTipIcon = ToolTipIcon.Info;
                    trayIcon.ShowBalloonTip(300);
                    reg.writeBool(RegistryConfig.REGISTRY_KEY_CLOSETIP, true);
                }
            }
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
            foreach (KeyValuePair<string, ProtocolType> item in protocols)
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

            if (newNode.ContextMenuStrip == null)
            {
                newNode.ContextMenuStrip = (newNode.remoteAccess == null ? folderMenuStrip : remoteMenuStrip);
            }

            serverTreeview.SelectedNode = newNode;
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
                result += String.Format("<remote name=\"{0}\" protocol=\"{1}\"", WebUtility.HtmlEncode(currentNode.Text), ra.protocol.name);
                if ((ra.login != null) && (ra.login != ""))
                {
                    result += String.Format(" login=\"{0}\"", WebUtility.HtmlEncode(ra.login));
                }
                if ((ra.password != null) && (ra.password != ""))
                {
                    result += String.Format(" password=\"{0}\"", WebUtility.HtmlEncode(ra.password));
                }
                if ((ra.host != null) && (ra.host != ""))
                {
                    result += String.Format(" host=\"{0}\"", WebUtility.HtmlEncode(ra.host));
                }
                if (ra.port != ra.protocol.defaultPort)
                {
                    result += String.Format(" port=\"{0}\"", ra.port);
                }
                if ((ra.resource != null) && (ra.resource != ""))
                {
                    result += String.Format(" resource=\"{0}\"", WebUtility.HtmlEncode(ra.resource));
                }
                result += " />\n";
            }
            else
            {
                // root, try to save hash
                if ((level == 0) && (MasterPassword.getInstance().hash != null))
                {
                    result = indent + String.Format("<folder name=\"{0}\" hash=\"{1}\">\n",
                        WebUtility.HtmlEncode(currentNode.Text), MasterPassword.getInstance().hash);
                }
                else
                {
                    result = indent + String.Format("<folder name=\"{0}\">\n", WebUtility.HtmlEncode(currentNode.Text));
                }

                foreach (TreeNodeAccess subNode in currentNode.Nodes)
                {
                    result += saveDatabaseSub(subNode, level + 1);
                }
                result += indent + "</folder>\n";
            }

            return result;
        }

        private void backupDatabase()
        {
            string backupConfigFile;
            string[] backups;
            Regex regBackupFile;
            Match matchBackupFile;
            DateTime dateBackupFile;
            double ageBackupFile;

            // create local backup
            backupConfigFile = databaseFile + "." + DateTime.Now.ToString("yyyy-MM-dd");
            if (!File.Exists(backupConfigFile))
            {
                File.Copy(databaseFile, backupConfigFile);
            }

            // delete old Backup
            backups = Directory.GetFiles(Path.GetDirectoryName(databaseFile), "teamlauncher.xml.????-??-??");
            regBackupFile = new Regex("(2[0-9]{3})-([0-9]{2})-([0-9]{2})$");
            dateBackupFile = new DateTime();
            foreach (string oldBackupConfigFile in backups)
            {
                matchBackupFile = regBackupFile.Match(oldBackupConfigFile);
                if (matchBackupFile.Success)
                {
                    dateBackupFile = new DateTime(
                        Int32.Parse(matchBackupFile.Groups[1].ToString()),
                        Int32.Parse(matchBackupFile.Groups[2].ToString()),
                        Int32.Parse(matchBackupFile.Groups[3].ToString()));

                    ageBackupFile = (DateTime.Now - dateBackupFile).TotalDays;
                    if (ageBackupFile > MAX_BACKUP_KEEY_DAYS)
                    {
                        File.Delete(oldBackupConfigFile);
                    }
                }
            }
        }

        private void saveDatabase()
        {
            string content;

            backupDatabase();

            content = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n" +
                saveDatabaseSub((TreeNodeAccess)serverTreeview.Nodes[0]);

            File.WriteAllText(databaseFile, content);
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

        private void errorClientMode()
        {
            MessageBox.Show("You are in client mode, you must change configuration on the server", "Mode client",
                MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
        }

        private void changeMasterPasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string currentMaster;
            string newMaster;
            ChangePassword changeMaster;

            if (currentMode == networkMode.client)
            {
                errorClientMode();
                return;
            }

            currentMaster = MasterPassword.getInstance().master;
            if (currentMaster == MasterPassword.NO_MASTER_ENTERED)
            {
                return;
            }

            changeMaster = new ChangePassword();
            if (changeMaster.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            newMaster = MasterPassword.checkPasswordConformity(changeMaster.newPassword);
            if (newMaster == null)
            {
                return;
            }

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

        private void newRemoteAccess(object sender, EventArgs e)
        {
            if (currentMode == networkMode.client)
            {
                errorClientMode();
                return;
            }

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

        private void editConfigurationFile(object sender, EventArgs e)
        {
            ProcessStartInfo psi;
            String backupConfigFile;

            if (currentMode == networkMode.client)
            {
                if (MessageBox.Show("You are in client mode, configuration file is a local copy from the server\n" +
                    "Your changes will be lost after program restart or configuration refresh, do you wish to continue?", "Mode client",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                {
                    return;
                }
            }

            backupConfigFile = databaseFile + "." + DateTime.Now.ToString("yyyy-MM-dd");
            if (!File.Exists(backupConfigFile))
            {
                File.Copy(databaseFile, backupConfigFile);
            }

            psi = new ProcessStartInfo(editor, String.Format("\"{0}\"", databaseFile));
            psi.UseShellExecute = true;
            psi.Verb = "runas";
            Process.Start(psi);
        }

        private void editNodeItem(object sender, EventArgs e)
        {
            TreeNodeAccess node;

            if (currentMode == networkMode.client)
            {
                errorClientMode();
                return;
            }

            node = (TreeNodeAccess)serverTreeview.SelectedNode;

            if (node == null)
                return;

            if (node.isFolder())
            {
                rename(sender, e);
                return;
            }

            if (editDialog == null)
            {
                editDialog = new EditRemoteAccess(protocols);
            }
            if (editDialog.ShowDialog(node.Text, node.remoteAccess) == DialogResult.OK)
            {
                node.Text = editDialog.RemoteName;
                node.remoteAccess = editDialog.RemoteDetail;
                saveDatabase();
                //here
            }
        }

        private void delete(object sender, EventArgs e)
        {
            TreeNodeAccess node;

            if (currentMode == networkMode.client)
            {
                errorClientMode();
                return;
            }

            node = (TreeNodeAccess)serverTreeview.SelectedNode;
            if (node == null)
                return;

            if (node.isRoot()) /* must not be root item */
                return;

            if (MessageBox.Show("Do you really want to delete item \"" + node.Text + "\"?", "Confirm delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                if ((node.isFolder()) && (node.Nodes.Count != 0)) /* handle folder subitems */
                {
                    int pos;
                    TreeNode parentNode;
                    DialogResult keepSubItems;

                    keepSubItems = MessageBox.Show("This folder is not empty!\nDo you want to keep subitems?", "Confirm delete subitems",
                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);

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

        private void connect(object sender, EventArgs e)
        {
            TreeNodeAccess node;

            node = (TreeNodeAccess)serverTreeview.SelectedNode;
            if (node == null)
                return;

            connectFromNode(node);
        }

        private void serverTreeview_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // to support right click
            serverTreeview.SelectedNode = e.Node;
        }

        private void newFolder(object sender, EventArgs e)
        {
            TreeNodeAccess newNode;
            ItemNameDialog diag;

            if (currentMode == networkMode.client)
            {
                errorClientMode();
                return;
            }

            diag = new ItemNameDialog("New folder");
            if (diag.ShowDialog() == DialogResult.OK)
            {
                if (diag.givenName != "")
                {
                    newNode = new TreeNodeAccess(diag.givenName);
                }
                else
                {
                    newNode = new TreeNodeAccess("New folder");
                }

                addNode(newNode);
                saveDatabase();
            }
        }

        private void reloadConfiguration(object sender, EventArgs e)
        {
            reloadDatabase();
        }

        private void serverTreeview_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNodeAccess node;
            bool remoteAccessSelected, folderSelected;

            node = (TreeNodeAccess)serverTreeview.SelectedNode;

            if (node == null)
            {
                remoteAccessSelected = false;
                folderSelected = false;
            }
            else
            {
                remoteAccessSelected = !node.isFolder();
                folderSelected = node.isFolder();
            }
            connectToolStripMenuItem1.Enabled = remoteAccessSelected;
            copyToolStripMenuItem1.Enabled = remoteAccessSelected;
            pasteToolStripMenuItem2.Enabled = remoteAccessSelected;

            editToolStripMenuItem2.Enabled = remoteAccessSelected || folderSelected;
            deleteToolStripMenuItem1.Enabled = remoteAccessSelected || folderSelected;
        }

        private void rename(object sender, EventArgs e)
        {
            ItemNameDialog diag;
            TreeNodeAccess node;

            Trace.WriteLine("Teamlauncher.rename()");

            if (currentMode == networkMode.client)
            {
                errorClientMode();
                return;
            }

            node = (TreeNodeAccess)serverTreeview.SelectedNode;
            if (node == null)
                return;

            diag = new ItemNameDialog(node.Text);
            if (diag.ShowDialog() == DialogResult.OK)
            {
                if (diag.givenName != "")
                {
                    node.Text = diag.givenName;
                }
                else
                {
                    node.Text = "Unamed folder";
                }
            }

            saveDatabase();
        }

        private void copyItem(object sender, EventArgs e)
        {
            TreeNodeAccess node;

            node = (TreeNodeAccess)serverTreeview.SelectedNode;
            if (node == null)
                return;

            Clipboard.SetText(Uri.EscapeDataString(node.Text)+":="+node.remoteAccess.ToString());
        }

        private void paste(object sender, EventArgs e)
        {
            string c;
            TreeNodeAccess newNode;
            string masterPassword;

            RemoteAccess ra;
            string name;

            Trace.WriteLine("Teamlauncher.paste()");

            if (currentMode == networkMode.client)
            {
                errorClientMode();
                return;
            }

            c = Clipboard.GetText();
            if (c == null)
                return;

            try
            {
                ra = new RemoteAccess(c, protocols, out name);

                // external paste
                if (name == null)
                {
                    // generate a name
                    name = String.Format("New {0} access", ra.protocol.name);
                    // cipher password
                    if ((ra.password != null) && (ra.password != ""))
                    {
                        masterPassword = MasterPassword.getInstance().master;
                        if (masterPassword == MasterPassword.NO_MASTER_ENTERED)
                        {
                            // no master entered, delete imported password
                            ra.password = null;
                        }
                        else if (masterPassword == MasterPassword.NO_MASTER_ENABLED)
                        {
                            // no master password exists, simply encode in base64
                            ra.password = Convert.ToBase64String(Encoding.UTF8.GetBytes(ra.password));
                        }
                        else
                        {
                            using (Encryption enc = new Encryption(masterPassword))
                            {
                                ra.password = enc.EncryptString(ra.password);
                            }
                        }
                    }
                }
                newNode = new TreeNodeAccess(ra, name);
                addNode(newNode);
                saveDatabase();
            }
            catch (Exception)
            {
                Trace.WriteLine("Teamlauncher.Paste(): Url decoding error:\n"+c);
                return;
            }
        }

        private void autoStartupController(object sender, EventArgs e)
        {
            RegistryKey Keyrun;

            Trace.WriteLine("Teamlauncher.autoStartupController()");

            using(Keyrun = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                if (sender == null) // only set menu check state
                {
                    startupAutomaticallyToolStripMenuItem.Checked = (Keyrun.GetValue("Teamlauncher") != null);
                }
                else if (startupAutomaticallyToolStripMenuItem.Checked) // click to uncheck
                {
                    Keyrun.DeleteValue("Teamlauncher");
                    startupAutomaticallyToolStripMenuItem.Checked = false;
                }
                else // click to check
                {
                    Keyrun.SetValue("Teamlauncher", "\"" + Assembly.GetEntryAssembly().Location + "\" /startup");
                    startupAutomaticallyToolStripMenuItem.Checked = true;
                }
            }
        }

        private void Teamlauncher_VisibleChanged(object sender, EventArgs e)
        {
            Trace.WriteLine("Teamlauncher.Teamlauncher_VisibleChanged()");

            if (!Visible)
            {
                return;
            }
            else
            {
                autoStartupController(null, new EventArgs());
                staysOnTopController(null, new EventArgs());

                if (WindowState == FormWindowState.Minimized)
                {
                    WindowState = FormWindowState.Normal;
                }
                BringToFront();
                Activate();
                serverTreeview.Focus();

                restoreFoldingState(foldingState);
            }
        }

        private void staysOnTopController(object sender, EventArgs e)
        {
            RegistryConfig reg;

            Trace.WriteLine("Teamlauncher.staysOnTopController()");

            using (reg = new RegistryConfig())
            {
                if (sender == null) // only set menu check state
                {
                    staysOntopToolStripMenuItem.Checked = reg.readBool(RegistryConfig.REGISTRY_KEY_WIN_TOPMOST);
                }
                else
                {
                    foldingState = serializeFoldingState();

                    if (staysOntopToolStripMenuItem.Checked) // click to uncheck
                    {
                        reg.writeBool(RegistryConfig.REGISTRY_KEY_WIN_TOPMOST, false);
                        staysOntopToolStripMenuItem.Checked = false;
                    }
                    else // click to check
                    {
                        reg.writeBool(RegistryConfig.REGISTRY_KEY_WIN_TOPMOST, true);
                        staysOntopToolStripMenuItem.Checked = true;
                    }

                    reg.writeString(RegistryConfig.REGISTRY_KEY_FOLDING, foldingState);
                }
            }

            TopMost = staysOntopToolStripMenuItem.Checked;
            ShowInTaskbar = !TopMost;

            if (sender != null)
            {
                restoreFoldingState(foldingState);
            }
        }

        void onDatabaseUpdate()
        {
            saveDatabase();
        }

        private void modeMenuItem_Click(object sender, EventArgs e)
        {
            ChangeMode cm;

            cm = new ChangeMode(currentMode, serverAddress);
            var dr = cm.ShowDialog();
            if (dr == DialogResult.OK)
            {
                currentMode = cm.resultMode;
                serverAddress = cm.resultServer;

                if (currentMode != networkMode.server)
                {
                    webserver.stop();
                }
                else if (currentMode == networkMode.server)
                {
                    webserver.start();
                }

                using (RegistryConfig reg = new RegistryConfig())
                {
                    reg.writeInteger(RegistryConfig.REGISTRY_KEY_MODE, (int)currentMode);
                    if (serverAddress != "")
                    {
                        reg.writeString(RegistryConfig.REGISTRY_KEY_SERVER, serverAddress);
                    }
                }
            }
        }

        public void ImportValidated(TreeNodeAccess node)
        {
            backupDatabase();
            serverTreeview.Nodes[0].Nodes.Add(node);
            saveDatabase();
        }

        private void remoteTeamlauncherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportWizard import;
            RemoteAccessEntry[] items;

            items = new RemoteAccessEntry[0];
            import = new ImportWizard(items);
            import.OnImport = ImportValidated;
            import.Show();
        }

        private void noPassowdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNodeAccess node;

            node = (TreeNodeAccess)serverTreeview.SelectedNode;
            if (node == null)
                return;

            connectFromNode(node, true);
        }

        private void fromPuTTYToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportWizard import;
            List<RemoteAccessEntry> ra_list;

            if (currentMode == networkMode.client)
            {
                errorClientMode();
                return;
            }

            ra_list = new List<RemoteAccessEntry>();
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\SimonTatham\PuTTY\Sessions"))
                {
                    string[] sessions;

                    sessions = key.GetSubKeyNames();
                    foreach (var sessionName in sessions)
                    {
                        using (RegistryKey subkey = key.OpenSubKey(sessionName))
                        {
                            string protocol;
                            RemoteAccess newAccess;
                            RemoteAccessEntry newAccessEntry;

                            protocol = (string)subkey.GetValue("Protocol");
                            if (protocol == "ssh")
                            {
                                string hostname, username;
                                int port;
                                hostname = (string)subkey.GetValue("HostName");
                                port = (int)subkey.GetValue("PortNumber");
                                username = (string)subkey.GetValue("UserName");

                                newAccess = new RemoteAccess();

                                newAccess.protocol = this.protocols["ssh"];
                                newAccess.host = hostname;
                                newAccess.port = port;
                                newAccess.login = username;

                                newAccessEntry.name = sessionName;
                                newAccessEntry.access = newAccess;
                                ra_list.Add(newAccessEntry);
                            }
                            else if (protocol == "telnet")
                            {
                                string hostname, username;
                                int port;

                                hostname = (string)subkey.GetValue("HostName");
                                port = (int)subkey.GetValue("PortNumber");
                                username = (string)subkey.GetValue("UserName");

                                newAccess = new RemoteAccess();
                                newAccess.protocol = this.protocols["telnet"];
                                newAccess.host = hostname;
                                newAccess.port = port;
                                newAccess.login = username;

                                newAccessEntry.name = sessionName;
                                newAccessEntry.access = newAccess;
                                ra_list.Add(newAccessEntry);
                            }
                            else if (protocol == "serial")
                            {
                                string hostname;

                                hostname = (string)subkey.GetValue("SerialLine");

                                newAccess = new RemoteAccess();
                                newAccess.protocol = this.protocols["serial"];
                                newAccess.host = hostname;

                                newAccessEntry.name = sessionName;
                                newAccessEntry.access = newAccess;
                                ra_list.Add(newAccessEntry);
                            }
                        }
                    }
                }

                import = new ImportWizard(ra_list.ToArray(), "Imported from Putty");
                import.OnImport = ImportValidated;
                import.Show();
            }
            catch(Exception ex)
            {
                MessageBox.Show(this, "Error while importing remote access from Putty:\n" + ex.Message, "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        byte winscp_char_decode(ref string Str)
        {
            int value;
            const byte PWALG_SIMPLE_MAGIC = 0xA3;

            if (Str.Length >= 2)
            {
                value = Byte.Parse(Str.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);

                byte Result = (byte)~(value ^ PWALG_SIMPLE_MAGIC);
                Str = Str.Substring(2);
                return Result;
            }
            else
                return 0x00;
        }

        private string winscp_decode(string password, string username, string hostname)
        {
            List<byte> buffer;
            int i;
            byte Length, Flag, byPass;
            string key;
            const byte PWALG_SIMPLE_FLAG = 0xFF;

            if ((password == null) || (password == ""))
                return "";

            key = username + hostname;

            Flag = winscp_char_decode(ref password);
            if (Flag == PWALG_SIMPLE_FLAG)
            {
                /* Dummy = */
                winscp_char_decode(ref password);
                Length = winscp_char_decode(ref password);
            }
            else
                Length = Flag;

            byPass = winscp_char_decode(ref password);

            password = password.Substring(byPass * 2);

            buffer = new List<byte>();
            for (i = 0; i < Length; i++)
            {
                buffer.Add(winscp_char_decode(ref password));
            }
            if (Flag == PWALG_SIMPLE_FLAG)
            {
                if (Encoding.UTF8.GetString(buffer.ToArray(), 0, key.Length) != key)
                    buffer.Clear();
                else
                    buffer.RemoveRange(0, key.Length);
            }
            return Encoding.UTF8.GetString(buffer.ToArray());
        }

        private void fromWinSCPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportWizard import;
            List<RemoteAccessEntry> ra_list;

            if (currentMode == networkMode.client)
            {
                errorClientMode();
                return;
            }

            ra_list = new List<RemoteAccessEntry>();
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Martin Prikryl\WinSCP 2\Sessions"))
                {
                    string[] sessions;

                    sessions = key.GetSubKeyNames();
                    foreach (var sessionName in sessions)
                    {
                        using (RegistryKey subkey = key.OpenSubKey(sessionName))
                        {
                            RemoteAccess newAccess;
                            RemoteAccessEntry newAccessEntry;
                            string hostname, username, password, resource;
                            int protocol, port;

                            try
                            {
                                hostname = (string)subkey.GetValue("HostName");
                            }
                            catch (Exception)
                            {
                                continue;
                            }

                            try
                            {
                                protocol = (int)subkey.GetValue("FSProtocol");
                            }
                            catch (Exception)
                            {
                                protocol = 0;
                            }
                            try
                            {
                                port = (int)subkey.GetValue("PortNumber");
                            }
                            catch (Exception)
                            {
                                port = 0;
                            }
                            try
                            {
                                username = (string)subkey.GetValue("UserName");
                            }
                            catch (Exception)
                            {
                                username = "";
                            }
                            try
                            {
                                password = (string)subkey.GetValue("Password");
                            }
                            catch (Exception)
                            {
                                password = "";
                            }
                            try
                            {
                                resource = (string)subkey.GetValue("RemoteDirectory");
                            }
                            catch (Exception)
                            {
                                resource = "";
                            }
 
                            switch (protocol)
                            {
                                case 0:
                                    newAccess = new RemoteAccess();

                                    newAccess.protocol = this.protocols["scp"];
                                    newAccess.host = hostname;
                                    newAccess.port = port;
                                    newAccess.login = username;
                                    newAccess.password = winscp_decode(password, username, hostname);
                                    newAccess.resource = resource;

                                    newAccessEntry.name = sessionName;
                                    newAccessEntry.access = newAccess;
                                    ra_list.Add(newAccessEntry);
                                    break;
                                case 5:
                                    newAccess = new RemoteAccess();
                                    newAccess.protocol = this.protocols["ftp"];
                                    newAccess.host = hostname;
                                    newAccess.port = port;
                                    newAccess.login = username;
                                    newAccess.password = winscp_decode(password, username, hostname);
                                    newAccess.resource = resource;

                                    newAccessEntry.name = sessionName;
                                    newAccessEntry.access = newAccess;
                                    ra_list.Add(newAccessEntry);
                                    break;
                            }
                        }
                    }
                }

                import = new ImportWizard(ra_list.ToArray(), "Imported from WinSCP");
                import.OnImport = ImportValidated;
                import.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error while importing remote access from WinSCP:\n" + ex.Message, "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void filezillaXmlImport(XmlNode xmlnode, ref List<RemoteAccessEntry> ra_list)
        {
            XmlNode child;

            if (xmlnode.Name == "Server")
            {
                RemoteAccess newAccess;
                RemoteAccessEntry newAccessEntry;
                string name, hostname, username, resource, password;
                int port, protocol;
                int i;

                hostname = "";
                port = 0;
                username = "";
                resource = "";
                name = "";
                password = "";

                for (i=0; i < xmlnode.ChildNodes.Count; i++)
                {
                    child = xmlnode.ChildNodes[i];
                    if (child.Name == "Host")
                    {
                        hostname = child.InnerText;
                    }
                    else if (child.Name == "Port")
                    {
                        Int32.TryParse(child.InnerText, out port);
                    }
                    else if (child.Name == "Protocol")
                    {
                        Int32.TryParse(child.InnerText, out protocol);
                    }
                    else if (child.Name == "User")
                    {
                        username = child.InnerText;
                    }
                    else if (child.Name == "Name")
                    {
                        name = child.InnerText;
                    }
                    else if (child.Name == "Pass" && child.Attributes[0].Value == "base64")
                    {
                        password = Encoding.UTF8.GetString(Convert.FromBase64String(child.InnerText));
                    }
                    else if (child.Name == "RemoteDir")
                    {
                        resource = child.InnerText;
                    }                     
                }

                newAccess = new RemoteAccess();

                newAccess.protocol = this.protocols["ftp"];
                newAccess.host = hostname;
                newAccess.port = port;
                newAccess.login = username;
                newAccess.password = password;
                newAccess.resource = resource;

                xmlnode.SelectNodes("Host");

                newAccessEntry.name = name;
                newAccessEntry.access = newAccess;
                ra_list.Add(newAccessEntry);

            }
            //else if (xmlnode.Name == "Folder")
            else
            {
                int i;
                for (i=0; i < xmlnode.ChildNodes.Count; i++)
                {
                    filezillaXmlImport(xmlnode.ChildNodes[i], ref ra_list);
                }
            }
        }

        private void fromFilezillaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<RemoteAccessEntry> ra_list;
            ImportWizard import;
            XmlDocument xmldoc;
            FileStream fs;
            string fz_xml;

            if (currentMode == networkMode.client)
            {
                errorClientMode();
                return;
            }

            fz_xml = Path.Combine(Environment.GetEnvironmentVariable("appdata"), @"FileZilla\sitemanager.xml");

            if (!File.Exists(fz_xml))
            {
                MessageBox.Show("FileZilla settings not found:\n"+fz_xml, "Filezilla import error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ra_list = new List<RemoteAccessEntry>();

            // open XML
            fs = new FileStream(fz_xml, FileMode.Open, FileAccess.Read);
            xmldoc = new XmlDocument();
            xmldoc.Load(fs);

            // do to: make function that browse the file
            // generate for each folder a RemoteAccess
            // generate for each server a RemoteAccess
            filezillaXmlImport(xmldoc.ChildNodes[1], ref ra_list);

            import = new ImportWizard(ra_list.ToArray(), "Imported from FileZilla");
            import.OnImport = ImportValidated;
            import.Show();
        }
    }
    
}
