namespace Teamlauncher
{
    partial class Teamlauncher
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            trayIcon.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Teamlauncher));
            this.serverTreeview = new System.Windows.Forms.TreeView();
            this.allMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.remoteAccessToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.folderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.editConfigurationFileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.reloadConfigurationFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.connectToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.newToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.remoteAccessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.folderToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reloadConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.changeMasterPasswordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.remoteMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.connectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.editToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.folderMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.remoteAccessToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.folderToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripSeparator();
            this.pasteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.allMenuStrip.SuspendLayout();
            this.windowMenu.SuspendLayout();
            this.remoteMenuStrip.SuspendLayout();
            this.folderMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // serverTreeview
            // 
            this.serverTreeview.AllowDrop = true;
            this.serverTreeview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.serverTreeview.ContextMenuStrip = this.allMenuStrip;
            this.serverTreeview.Location = new System.Drawing.Point(0, 27);
            this.serverTreeview.Name = "serverTreeview";
            this.serverTreeview.Size = new System.Drawing.Size(214, 408);
            this.serverTreeview.TabIndex = 0;
            this.serverTreeview.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.serverTreeview_AfterSelect);
            this.serverTreeview.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.serverTreeview_NodeMouseClick);
            this.serverTreeview.DoubleClick += new System.EventHandler(this.connect);
            this.serverTreeview.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.serverTreeview_KeyPress);
            // 
            // allMenuStrip
            // 
            this.allMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.toolStripMenuItem1,
            this.editConfigurationFileToolStripMenuItem1,
            this.reloadConfigurationFileToolStripMenuItem});
            this.allMenuStrip.Name = "contextMenuStrip1";
            this.allMenuStrip.Size = new System.Drawing.Size(176, 76);
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.remoteAccessToolStripMenuItem1,
            this.folderToolStripMenuItem});
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.newToolStripMenuItem.Text = "&New";
            // 
            // remoteAccessToolStripMenuItem1
            // 
            this.remoteAccessToolStripMenuItem1.Name = "remoteAccessToolStripMenuItem1";
            this.remoteAccessToolStripMenuItem1.Size = new System.Drawing.Size(146, 22);
            this.remoteAccessToolStripMenuItem1.Text = "Remote &access";
            this.remoteAccessToolStripMenuItem1.Click += new System.EventHandler(this.newRemoteAccess);
            // 
            // folderToolStripMenuItem
            // 
            this.folderToolStripMenuItem.Name = "folderToolStripMenuItem";
            this.folderToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.folderToolStripMenuItem.Text = "&Folder";
            this.folderToolStripMenuItem.Click += new System.EventHandler(this.newFolder);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(172, 6);
            // 
            // editConfigurationFileToolStripMenuItem1
            // 
            this.editConfigurationFileToolStripMenuItem1.Name = "editConfigurationFileToolStripMenuItem1";
            this.editConfigurationFileToolStripMenuItem1.Size = new System.Drawing.Size(175, 22);
            this.editConfigurationFileToolStripMenuItem1.Text = "&Edit configuration file";
            this.editConfigurationFileToolStripMenuItem1.Click += new System.EventHandler(this.editConfiguration);
            // 
            // reloadConfigurationFileToolStripMenuItem
            // 
            this.reloadConfigurationFileToolStripMenuItem.Name = "reloadConfigurationFileToolStripMenuItem";
            this.reloadConfigurationFileToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.reloadConfigurationFileToolStripMenuItem.Text = "&Refresh";
            this.reloadConfigurationFileToolStripMenuItem.Click += new System.EventHandler(this.reloadConfiguration);
            // 
            // windowMenu
            // 
            this.windowMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem1,
            this.editToolStripMenuItem,
            this.helpToolStripMenuItem1});
            this.windowMenu.Location = new System.Drawing.Point(0, 0);
            this.windowMenu.Name = "windowMenu";
            this.windowMenu.Size = new System.Drawing.Size(214, 24);
            this.windowMenu.TabIndex = 7;
            this.windowMenu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem1
            // 
            this.fileToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToolStripMenuItem1,
            this.toolStripMenuItem3,
            this.newToolStripMenuItem1,
            this.toolStripMenuItem6,
            this.exitToolStripMenuItem1});
            this.fileToolStripMenuItem1.Name = "fileToolStripMenuItem1";
            this.fileToolStripMenuItem1.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem1.Text = "File";
            // 
            // connectToolStripMenuItem1
            // 
            this.connectToolStripMenuItem1.Name = "connectToolStripMenuItem1";
            this.connectToolStripMenuItem1.Size = new System.Drawing.Size(132, 22);
            this.connectToolStripMenuItem1.Text = "&Connect";
            this.connectToolStripMenuItem1.Click += new System.EventHandler(this.connect);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(129, 6);
            // 
            // newToolStripMenuItem1
            // 
            this.newToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.remoteAccessToolStripMenuItem,
            this.folderToolStripMenuItem1});
            this.newToolStripMenuItem1.Name = "newToolStripMenuItem1";
            this.newToolStripMenuItem1.Size = new System.Drawing.Size(132, 22);
            this.newToolStripMenuItem1.Text = "New";
            // 
            // remoteAccessToolStripMenuItem
            // 
            this.remoteAccessToolStripMenuItem.Name = "remoteAccessToolStripMenuItem";
            this.remoteAccessToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.remoteAccessToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.remoteAccessToolStripMenuItem.Text = "Remote &access";
            this.remoteAccessToolStripMenuItem.Click += new System.EventHandler(this.newRemoteAccess);
            // 
            // folderToolStripMenuItem1
            // 
            this.folderToolStripMenuItem1.Name = "folderToolStripMenuItem1";
            this.folderToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.N)));
            this.folderToolStripMenuItem1.Size = new System.Drawing.Size(185, 22);
            this.folderToolStripMenuItem1.Text = "&Folder";
            this.folderToolStripMenuItem1.Click += new System.EventHandler(this.newFolder);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(129, 6);
            // 
            // exitToolStripMenuItem1
            // 
            this.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            this.exitToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.exitToolStripMenuItem1.Size = new System.Drawing.Size(132, 22);
            this.exitToolStripMenuItem1.Text = "E&xit";
            this.exitToolStripMenuItem1.Click += new System.EventHandler(this.OnExit);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configurationToolStripMenuItem,
            this.reloadConfigurationToolStripMenuItem,
            this.toolStripMenuItem7,
            this.changeMasterPasswordToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // configurationToolStripMenuItem
            // 
            this.configurationToolStripMenuItem.Name = "configurationToolStripMenuItem";
            this.configurationToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12;
            this.configurationToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.configurationToolStripMenuItem.Text = "&Configuration file...";
            this.configurationToolStripMenuItem.Click += new System.EventHandler(this.editConfiguration);
            // 
            // reloadConfigurationToolStripMenuItem
            // 
            this.reloadConfigurationToolStripMenuItem.Name = "reloadConfigurationToolStripMenuItem";
            this.reloadConfigurationToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.reloadConfigurationToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.reloadConfigurationToolStripMenuItem.Text = "&Reload configuration";
            this.reloadConfigurationToolStripMenuItem.Click += new System.EventHandler(this.reloadConfiguration);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(193, 6);
            // 
            // changeMasterPasswordToolStripMenuItem
            // 
            this.changeMasterPasswordToolStripMenuItem.Name = "changeMasterPasswordToolStripMenuItem";
            this.changeMasterPasswordToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.changeMasterPasswordToolStripMenuItem.Text = "Change master password";
            this.changeMasterPasswordToolStripMenuItem.Click += new System.EventHandler(this.changeMasterPasswordToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem1
            // 
            this.helpToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem1});
            this.helpToolStripMenuItem1.Name = "helpToolStripMenuItem1";
            this.helpToolStripMenuItem1.Size = new System.Drawing.Size(40, 20);
            this.helpToolStripMenuItem1.Text = "Help";
            // 
            // aboutToolStripMenuItem1
            // 
            this.aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
            this.aboutToolStripMenuItem1.Size = new System.Drawing.Size(103, 22);
            this.aboutToolStripMenuItem1.Text = "&About";
            this.aboutToolStripMenuItem1.Click += new System.EventHandler(this.aboutToolStripMenuItem1_Click);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.OnExit);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            // 
            // remoteMenuStrip
            // 
            this.remoteMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToolStripMenuItem,
            this.toolStripMenuItem2,
            this.editToolStripMenuItem1,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.remoteMenuStrip.Name = "remoteMenuStrip";
            this.remoteMenuStrip.Size = new System.Drawing.Size(140, 120);
            // 
            // connectToolStripMenuItem
            // 
            this.connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            this.connectToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.connectToolStripMenuItem.Text = "Connect";
            this.connectToolStripMenuItem.Click += new System.EventHandler(this.connect);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(136, 6);
            // 
            // editToolStripMenuItem1
            // 
            this.editToolStripMenuItem1.Name = "editToolStripMenuItem1";
            this.editToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.editToolStripMenuItem1.Size = new System.Drawing.Size(139, 22);
            this.editToolStripMenuItem1.Text = "&Edit";
            this.editToolStripMenuItem1.Click += new System.EventHandler(this.editItem);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.copyToolStripMenuItem.Text = "&Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyItem);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.pasteToolStripMenuItem.Text = "&Paste";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.paste);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.deleteToolStripMenuItem.Text = "&Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.delete);
            // 
            // folderMenuStrip
            // 
            this.folderMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem2,
            this.toolStripMenuItem8,
            this.toolStripMenuItem4,
            this.toolStripMenuItem5,
            this.toolStripMenuItem9,
            this.pasteToolStripMenuItem1});
            this.folderMenuStrip.Name = "remoteMenuStrip";
            this.folderMenuStrip.Size = new System.Drawing.Size(152, 104);
            // 
            // newToolStripMenuItem2
            // 
            this.newToolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.remoteAccessToolStripMenuItem2,
            this.folderToolStripMenuItem2});
            this.newToolStripMenuItem2.Name = "newToolStripMenuItem2";
            this.newToolStripMenuItem2.Size = new System.Drawing.Size(151, 22);
            this.newToolStripMenuItem2.Text = "&New";
            // 
            // remoteAccessToolStripMenuItem2
            // 
            this.remoteAccessToolStripMenuItem2.Name = "remoteAccessToolStripMenuItem2";
            this.remoteAccessToolStripMenuItem2.Size = new System.Drawing.Size(146, 22);
            this.remoteAccessToolStripMenuItem2.Text = "Remote &access";
            this.remoteAccessToolStripMenuItem2.Click += new System.EventHandler(this.newRemoteAccess);
            // 
            // folderToolStripMenuItem2
            // 
            this.folderToolStripMenuItem2.Name = "folderToolStripMenuItem2";
            this.folderToolStripMenuItem2.Size = new System.Drawing.Size(146, 22);
            this.folderToolStripMenuItem2.Text = "&Folder";
            this.folderToolStripMenuItem2.Click += new System.EventHandler(this.newFolder);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(148, 6);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.toolStripMenuItem4.Size = new System.Drawing.Size(151, 22);
            this.toolStripMenuItem4.Text = "&Rename";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.rename);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.toolStripMenuItem5.Size = new System.Drawing.Size(151, 22);
            this.toolStripMenuItem5.Text = "&Delete";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.delete);
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(148, 6);
            // 
            // pasteToolStripMenuItem1
            // 
            this.pasteToolStripMenuItem1.Name = "pasteToolStripMenuItem1";
            this.pasteToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.pasteToolStripMenuItem1.Size = new System.Drawing.Size(151, 22);
            this.pasteToolStripMenuItem1.Text = "&Paste";
            this.pasteToolStripMenuItem1.Click += new System.EventHandler(this.paste);
            // 
            // Teamlauncher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(214, 435);
            this.Controls.Add(this.serverTreeview);
            this.Controls.Add(this.windowMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.windowMenu;
            this.Name = "Teamlauncher";
            this.Text = "Teamlauncher";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Teamlauncher_FormClosing);
            this.Load += new System.EventHandler(this.Teamlauncher_Load);
            this.Shown += new System.EventHandler(this.Teamlauncher_Shown);
            this.allMenuStrip.ResumeLayout(false);
            this.windowMenu.ResumeLayout(false);
            this.windowMenu.PerformLayout();
            this.remoteMenuStrip.ResumeLayout(false);
            this.folderMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView serverTreeview;
        private System.Windows.Forms.MenuStrip windowMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeMasterPasswordToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip allMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem folderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem remoteAccessToolStripMenuItem1;
        private System.Windows.Forms.ContextMenuStrip remoteMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem editConfigurationFileToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem reloadConfigurationFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem connectToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ContextMenuStrip folderMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem configurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem connectToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem remoteAccessToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem folderToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem reloadConfigurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem remoteAccessToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem folderToolStripMenuItem2;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem8;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem9;
    }
}

