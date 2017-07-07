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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Teamlauncher));
            this.serverTreeview = new System.Windows.Forms.TreeView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.remoteAccessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addNewAccessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addNewFolderToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.editAccessToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteItemToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.configurationToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.editconfigurationFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reloadConfigurationFileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.changeMasterPasswordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // serverTreeview
            // 
            this.serverTreeview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.serverTreeview.Location = new System.Drawing.Point(0, 27);
            this.serverTreeview.Name = "serverTreeview";
            this.serverTreeview.Size = new System.Drawing.Size(214, 408);
            this.serverTreeview.TabIndex = 0;
            this.serverTreeview.DoubleClick += new System.EventHandler(this.treeView1_DoubleClick);
            this.serverTreeview.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.serverTreeview_KeyPress);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem1,
            this.editToolStripMenuItem,
            this.helpToolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(214, 24);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem1
            // 
            this.fileToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem1});
            this.fileToolStripMenuItem1.Name = "fileToolStripMenuItem1";
            this.fileToolStripMenuItem1.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem1.Text = "File";
            // 
            // exitToolStripMenuItem1
            // 
            this.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            this.exitToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.exitToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem1.Text = "E&xit";
            this.exitToolStripMenuItem1.Click += new System.EventHandler(this.OnExit);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.remoteAccessToolStripMenuItem,
            this.configurationToolStripMenuItem1,
            this.changeMasterPasswordToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // remoteAccessToolStripMenuItem
            // 
            this.remoteAccessToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNewAccessToolStripMenuItem,
            this.addNewFolderToolStripMenuItem1,
            this.editAccessToolStripMenuItem1,
            this.deleteItemToolStripMenuItem1});
            this.remoteAccessToolStripMenuItem.Name = "remoteAccessToolStripMenuItem";
            this.remoteAccessToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.remoteAccessToolStripMenuItem.Text = "&Structure";
            // 
            // addNewAccessToolStripMenuItem
            // 
            this.addNewAccessToolStripMenuItem.Name = "addNewAccessToolStripMenuItem";
            this.addNewAccessToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.addNewAccessToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.addNewAccessToolStripMenuItem.Text = "Add new &access";
            this.addNewAccessToolStripMenuItem.Click += new System.EventHandler(this.AddNewAccess_Click);
            // 
            // addNewFolderToolStripMenuItem1
            // 
            this.addNewFolderToolStripMenuItem1.Name = "addNewFolderToolStripMenuItem1";
            this.addNewFolderToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.N)));
            this.addNewFolderToolStripMenuItem1.Size = new System.Drawing.Size(216, 22);
            this.addNewFolderToolStripMenuItem1.Text = "Add new &folder";
            this.addNewFolderToolStripMenuItem1.Click += new System.EventHandler(this.addNewFolderToolStripMenuItem_Click);
            // 
            // editAccessToolStripMenuItem1
            // 
            this.editAccessToolStripMenuItem1.Name = "editAccessToolStripMenuItem1";
            this.editAccessToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.editAccessToolStripMenuItem1.Size = new System.Drawing.Size(216, 22);
            this.editAccessToolStripMenuItem1.Text = "&Edit access";
            this.editAccessToolStripMenuItem1.Click += new System.EventHandler(this.editAccessToolStripMenuItem_Click);
            // 
            // deleteItemToolStripMenuItem1
            // 
            this.deleteItemToolStripMenuItem1.Name = "deleteItemToolStripMenuItem1";
            this.deleteItemToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.deleteItemToolStripMenuItem1.Size = new System.Drawing.Size(216, 22);
            this.deleteItemToolStripMenuItem1.Text = "&Delete item";
            this.deleteItemToolStripMenuItem1.Click += new System.EventHandler(this.deleteFolderToolStripMenuItem_Click);
            // 
            // configurationToolStripMenuItem1
            // 
            this.configurationToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editconfigurationFileToolStripMenuItem,
            this.reloadConfigurationFileToolStripMenuItem1});
            this.configurationToolStripMenuItem1.Name = "configurationToolStripMenuItem1";
            this.configurationToolStripMenuItem1.Size = new System.Drawing.Size(196, 22);
            this.configurationToolStripMenuItem1.Text = "&Configuration";
            // 
            // editconfigurationFileToolStripMenuItem
            // 
            this.editconfigurationFileToolStripMenuItem.Name = "editconfigurationFileToolStripMenuItem";
            this.editconfigurationFileToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12;
            this.editconfigurationFileToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.editconfigurationFileToolStripMenuItem.Text = "&Edit file manually";
            this.editconfigurationFileToolStripMenuItem.Click += new System.EventHandler(this.configurationFileToolStripMenuItem_Click);
            // 
            // reloadConfigurationFileToolStripMenuItem1
            // 
            this.reloadConfigurationFileToolStripMenuItem1.Name = "reloadConfigurationFileToolStripMenuItem1";
            this.reloadConfigurationFileToolStripMenuItem1.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.reloadConfigurationFileToolStripMenuItem1.Size = new System.Drawing.Size(179, 22);
            this.reloadConfigurationFileToolStripMenuItem1.Text = "&Reload";
            this.reloadConfigurationFileToolStripMenuItem1.Click += new System.EventHandler(this.reloadConfigurationFileToolStripMenuItem_Click);
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
            // Teamlauncher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(214, 435);
            this.Controls.Add(this.serverTreeview);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Teamlauncher";
            this.Text = "Teamlauncher";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Teamlauncher_FormClosing);
            this.Load += new System.EventHandler(this.Teamlauncher_Load);
            this.Shown += new System.EventHandler(this.Teamlauncher_Shown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView serverTreeview;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configurationToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem editconfigurationFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reloadConfigurationFileToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem remoteAccessToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addNewAccessToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editAccessToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem deleteItemToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem changeMasterPasswordToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addNewFolderToolStripMenuItem1;
    }
}

