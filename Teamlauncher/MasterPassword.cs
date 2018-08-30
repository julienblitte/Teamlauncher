#define ALLOW_SHORT_PASSWORD

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
    public delegate Action<bool> cacheChangeDelegate(bool passwordCached);

    public partial class MasterPassword : Form
    {
        private static MasterPassword instance = null;
        private string _master;
        public cacheChangeDelegate onCacheChanged;

        public string master
        {
            get
            {
                if (hash == null)
                {
                    return NO_MASTER_ENABLED;
                }

                if (_master == null)
                {
                    this.ShowDialog();
                }
                expirationTimer.Stop();
                expirationTimer.Start();
                notifyDelegates();

                return this._master;
            }

            set
            {
                Hash hasher;
                hasher = new Hash(HASH_SEED);

                hash = hasher.compute(value);
                this._master = value;
                expirationTimer.Start();
                notifyDelegates();
            }
        }
        public string hash { get; set; }

        private const string HASH_SEED = "Teamlauncher";

        public const string NO_MASTER_ENTERED = null;
        public const string NO_MASTER_ENABLED = "";

        private MasterPassword()
        {
            InitializeComponent();
            _master = NO_MASTER_ENTERED;
            hash = null;
        }

        public static MasterPassword getInstance()
        {
            if (instance == null)
            {
                instance = new MasterPassword();
            }
            return MasterPassword.instance;
        }

        public void clear()
        {
            this._master = NO_MASTER_ENTERED;
            expirationTimer.Stop();

            notifyDelegates();
        }

        public void notifyDelegates()
        {
			if (_master == NO_MASTER_ENABLED)
				return;

			onCacheChanged?.Invoke(_master != NO_MASTER_ENTERED);
        }


        public static string checkPasswordConformity(string localPassword)
        {
#if ALLOW_SHORT_PASSWORD
            if (localPassword.Length < 4)
            {
                MessageBox.Show("Password too short!\nMake your password length 4 or more characters.", "Invalid Password", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
            if (localPassword.Length < 8)
            {
                Hash passGen;

                // ! inverse mode: generate HASH_SEED checksum with pass as seed
                passGen = new Hash(localPassword);
                localPassword = passGen.compute(HASH_SEED);
            }   
#else
            if (password.Text.Length < 8)
            {
                MessageBox.Show("Password too short!\nMake your password length 8 or more characters.", "Invalid Password", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
#endif
            return localPassword;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            Hash hasher;
            string conformPassword;
            hasher = new Hash(HASH_SEED);

            conformPassword = checkPasswordConformity(password.Text);
            if (conformPassword == null)
            {
                return;
            }

            if (hash == null)
            {
                hash = hasher.compute(conformPassword);
            }
            else if (hasher.compute(conformPassword) != this.hash)
            {
                MessageBox.Show("Password hash does not match!\nIs your password correct?", "Invalid Password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this._master = conformPassword;
            password.Text = "";
            expirationTimer.Start();
            notifyDelegates();
            this.DialogResult = DialogResult.OK;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            _master = NO_MASTER_ENTERED;
            password.Text = "";
            this.DialogResult = DialogResult.Cancel;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.clear();
        }

        private void MasterPassword_Shown(object sender, EventArgs e)
        {
            password.Focus();
        }
    }
}
