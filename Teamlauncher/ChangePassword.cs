﻿using System;
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
    public partial class ChangePassword : Form
    {
        private string _newPassword;
        private int minimumLength;

        public string newPassword
        {
            get
            {
                string result = _newPassword;
                _newPassword = null;
                return result;
            }
        }

        public ChangePassword()
        {
            InitializeComponent();
            minimumLength = 0;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (password.Text != confirm.Text)
            {
                MessageBox.Show("The both password does not match!", "Passwords mistmatch", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password.Text.Length >= minimumLength)
            {
                _newPassword = password.Text;
            }

            password.Text = "";
            confirm.Text = "";
            DialogResult = DialogResult.OK;
        }
    }
}
