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
    public partial class ItemNameDialog : Form
    {
        private string defaultName;

        public string givenName
        {
            set { nameInput.Text = givenName; if (defaultName == "") defaultName = givenName; }
            get { return nameInput.Text; }

        }

        public ItemNameDialog()
        {
            InitializeComponent();
            defaultName = "";
        }

        public ItemNameDialog(string myDefaultName)
        {
            InitializeComponent();
            defaultName = myDefaultName;
            nameInput.Text = myDefaultName;
        }

        private void ok_Click(object sender, EventArgs e)
        {
            if (nameInput.Text == "")
                nameInput.Text = defaultName;

            this.DialogResult = DialogResult.OK;
        }
    }
}
