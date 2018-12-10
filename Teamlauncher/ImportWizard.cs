using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Teamlauncher
{
    public struct RemoteAccessEntry
    {
        public string name;
        public RemoteAccess access;

        public override string ToString()
        {
            return name;
        }

    }


    public delegate void ImportValidated(TreeNodeAccess nodes);

    public partial class ImportWizard : Form
    {
        private RemoteAccessEntry[] _accesslist;
        public ImportValidated OnImport;
        string folderName;

        // Delegate to list items to import
        public ImportWizard(RemoteAccessEntry[] accesslist, string importFolder = null)
        {
            Trace.WriteLine("Teamlauncher.ImportWizard()");
            InitializeComponent();

            _accesslist = accesslist;
            folderName = (importFolder != null ? importFolder : "New import");
        }

        private void Import_Click(object sender, EventArgs e)
        {
            int i;
            TreeNodeAccess import;
            RemoteAccess ra;
            string name;
            string master;

            master = MasterPassword.getInstance().master;
            if (master == MasterPassword.NO_MASTER_ENTERED)
            {
                Close();
                return;
            }

            import = new TreeNodeAccess(directoryName.Text);
            for (i=0; i < checkedListBox1.CheckedItems.Count; i++)
            {
                ra = ((RemoteAccessEntry)checkedListBox1.CheckedItems[i]).access;

                // cipher collected passwords
                if (master == MasterPassword.NO_MASTER_ENABLED)
                {
                    ra.password = Convert.ToBase64String(Encoding.UTF8.GetBytes(ra.password));
                }
                else
                {
                    using (Encryption enc = new Encryption(master))
                    {
                        ra.password = enc.EncryptString(ra.password);
                    }
                }
                name = ((RemoteAccessEntry)checkedListBox1.CheckedItems[i]).name;
                import.Nodes.Add(new TreeNodeAccess(ra, name));
            }

            if (OnImport != null && i != 0)
            {
                OnImport(import);
            }

            Close();
        }

        private void ImportWizard_Load(object sender, EventArgs e)
        {
            int i;

            checkedListBox1.Items.Clear();
            for (i=0; i < _accesslist.Length; i++)
            {
                checkedListBox1.Items.Add(_accesslist[i], true);
            }

            directoryName.Text = folderName;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
