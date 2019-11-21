namespace Teamlauncher
{
    partial class ChangeMode
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
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.singleMode = new System.Windows.Forms.RadioButton();
            this.serverMode = new System.Windows.Forms.RadioButton();
            this.clientMode = new System.Windows.Forms.RadioButton();
            this.clientServer = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.clientBox = new System.Windows.Forms.GroupBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.passwordBox = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.passwordServer = new System.Windows.Forms.MaskedTextBox();
            this.clientBox.SuspendLayout();
            this.passwordBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // singleMode
            // 
            this.singleMode.AutoSize = true;
            this.singleMode.Location = new System.Drawing.Point(12, 12);
            this.singleMode.Name = "singleMode";
            this.singleMode.Size = new System.Drawing.Size(83, 17);
            this.singleMode.TabIndex = 0;
            this.singleMode.TabStop = true;
            this.singleMode.Text = "Single mode";
            this.singleMode.UseVisualStyleBackColor = true;
            this.singleMode.CheckedChanged += new System.EventHandler(this.modeChecked);
            // 
            // serverMode
            // 
            this.serverMode.AutoSize = true;
            this.serverMode.Location = new System.Drawing.Point(12, 35);
            this.serverMode.Name = "serverMode";
            this.serverMode.Size = new System.Drawing.Size(85, 17);
            this.serverMode.TabIndex = 1;
            this.serverMode.TabStop = true;
            this.serverMode.Text = "Server mode";
            this.serverMode.UseVisualStyleBackColor = true;
            this.serverMode.CheckedChanged += new System.EventHandler(this.modeChecked);
            // 
            // clientMode
            // 
            this.clientMode.AutoSize = true;
            this.clientMode.Location = new System.Drawing.Point(12, 56);
            this.clientMode.Name = "clientMode";
            this.clientMode.Size = new System.Drawing.Size(80, 17);
            this.clientMode.TabIndex = 2;
            this.clientMode.TabStop = true;
            this.clientMode.Text = "Client mode";
            this.clientMode.UseVisualStyleBackColor = true;
            this.clientMode.CheckedChanged += new System.EventHandler(this.modeChecked);
            // 
            // clientServer
            // 
            this.clientServer.Location = new System.Drawing.Point(9, 32);
            this.clientServer.Name = "clientServer";
            this.clientServer.Size = new System.Drawing.Size(148, 20);
            this.clientServer.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Server address:";
            // 
            // clientBox
            // 
            this.clientBox.Controls.Add(this.clientServer);
            this.clientBox.Controls.Add(this.label1);
            this.clientBox.Location = new System.Drawing.Point(117, 74);
            this.clientBox.Name = "clientBox";
            this.clientBox.Size = new System.Drawing.Size(174, 68);
            this.clientBox.TabIndex = 5;
            this.clientBox.TabStop = false;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(12, 90);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 6;
            this.okButton.Text = "Ok";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(12, 119);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 7;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // passwordBox
            // 
            this.passwordBox.Controls.Add(this.passwordServer);
            this.passwordBox.Controls.Add(this.label2);
            this.passwordBox.Location = new System.Drawing.Point(117, 7);
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.Size = new System.Drawing.Size(173, 66);
            this.passwordBox.TabIndex = 10;
            this.passwordBox.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Server Password:";
            // 
            // passwordServer
            // 
            this.passwordServer.Location = new System.Drawing.Point(9, 32);
            this.passwordServer.Name = "passwordServer";
            this.passwordServer.PasswordChar = '*';
            this.passwordServer.Size = new System.Drawing.Size(147, 20);
            this.passwordServer.TabIndex = 12;
            // 
            // ChangeMode
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(307, 158);
            this.Controls.Add(this.passwordBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.clientBox);
            this.Controls.Add(this.clientMode);
            this.Controls.Add(this.serverMode);
            this.Controls.Add(this.singleMode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChangeMode";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ChangeMode";
            this.TopMost = true;
            this.clientBox.ResumeLayout(false);
            this.clientBox.PerformLayout();
            this.passwordBox.ResumeLayout(false);
            this.passwordBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton singleMode;
        private System.Windows.Forms.RadioButton serverMode;
        private System.Windows.Forms.RadioButton clientMode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox clientBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.TextBox clientServer;
        private System.Windows.Forms.GroupBox passwordBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MaskedTextBox passwordServer;
    }
}