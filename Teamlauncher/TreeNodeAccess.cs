using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Teamlauncher
{
    class TreeNodeAccess : TreeNode
    {
        protected ProtoRemoteAccess _remoteAccess;
        public ProtoRemoteAccess remoteAccess
        {
            set {
                _remoteAccess = value;
                this.SelectedImageIndex = value.protocol.id;
                this.ImageIndex = value.protocol.id;
            }
            get { return _remoteAccess; }
        }

        public TreeNodeAccess(string text) : base(text)
        {
            _remoteAccess = null;
            this.SelectedImageIndex = 0;
            this.ImageIndex = 0;
        }

        public TreeNodeAccess(ProtoRemoteAccess remoteAccess, string text) : base(text)
        {
            this._remoteAccess = remoteAccess;
            this.SelectedImageIndex = remoteAccess.protocol.id;
            this.ImageIndex = _remoteAccess.protocol.id;
        }

        public bool isFolder()
        {
            return (_remoteAccess == null);
        }

        public bool isRoot()
        {
            return (Parent == null);
        }

        public override object Clone()
        {
            TreeNodeAccess result;

            if (isFolder())
            {
                TreeNodeAccess clone;

                result = new TreeNodeAccess(this.Text);
                foreach (TreeNodeAccess childNode in Nodes)
                {
                    clone = (TreeNodeAccess)childNode.Clone();
                    result.Nodes.Add(clone);
                }
            }
            else
            {
                result = new TreeNodeAccess(_remoteAccess, this.Text);
            }

            return result;
        }
    }
}
