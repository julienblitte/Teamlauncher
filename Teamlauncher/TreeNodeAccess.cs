using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Teamlauncher
{
    class TreeNodeAccess : TreeNode, IEnumerable
    {
        protected RemoteAccess _remoteAccess;
        public RemoteAccess remoteAccess
        {
            set {
                _remoteAccess = value;
                this.SelectedImageIndex = value.protocol.id;
                this.ImageIndex = value.protocol.id;
                this.ToolTipText = String.Format("{0} ({1})", value.host, value.protocol.ToString());
                /*
                this.ToolTipText = (value.login != null ? String.Format("{0} ({1}) - {3}",
                    value.host, value.protocol.ToString(), value.login)
                    :
                    String.Format("{0} ({1})",
                        value.host, value.protocol.ToString())
                );
                */
            }
            get { return _remoteAccess; }
        }

        public TreeNodeAccess(string text) : base(text)
        {
            _remoteAccess = null;
            this.SelectedImageIndex = 0;
            this.ImageIndex = 0;
        }

        public TreeNodeAccess(RemoteAccess remoteAccess, string text) : base(text)
        {
            this._remoteAccess = remoteAccess;
            this.SelectedImageIndex = remoteAccess.protocol.id;
            this.ImageIndex = _remoteAccess.protocol.id;
            this.ToolTipText = String.Format("$1 ($2) - $3",
                remoteAccess.host, remoteAccess.protocol.ToString(), remoteAccess.login);
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
                result.ContextMenuStrip = ContextMenuStrip;
                foreach (TreeNodeAccess childNode in Nodes)
                {
                    clone = (TreeNodeAccess)childNode.Clone();
                    result.Nodes.Add(clone);
                }
            }
            else
            {
                result = new TreeNodeAccess(_remoteAccess, this.Text);
                result.ContextMenuStrip = ContextMenuStrip;
            }

            return result;
        }

        public IEnumerator GetEnumerator()
        {
            return new TreeNodeAccessEnumerator(this);
        }
    }


    class TreeNodeAccessEnumerator : IEnumerator<TreeNodeAccess>
    {
        private TreeNodeAccess _current;
        private bool started;

        object IEnumerator.Current { get { return _current; } }
        public TreeNodeAccess Current { get { return _current; } }

        public TreeNodeAccessEnumerator(TreeNodeAccess current)
        {
            _current = current;
            started = false;
        }

        bool IEnumerator.MoveNext()
        {
            TreeNodeAccess parent;
            TreeNodeAccess next;
            int index;

            parent = (TreeNodeAccess)_current.Parent;
            next = (TreeNodeAccess)_current.NextNode;
            index = _current.Index;

            if (!started)
            {
                started = true;
                return true;
            }

            // there is child(s), goes into recursely
            if (_current.Nodes.Count != 0)
            {
                _current = (TreeNodeAccess)_current.Nodes[0];
                return true;
            }

            // try to get next
            if (next != null)
            {
                _current = next;
                return true;
            }

            // end of the list, go up
            while (!_current.isRoot())
            {
                _current = (TreeNodeAccess)_current.Parent;
                next = (TreeNodeAccess)_current.NextNode;
                if (next != null)
                {
                    _current = next;
                    return true;
                }
            }

            return false;
        }

        void IEnumerator.Reset()
        {
            TreeNodeAccess prev;

            // go to top level
            while (!_current.isRoot())
            {
                _current = (TreeNodeAccess)_current.Parent;
            }

            // select first item
            prev = (TreeNodeAccess)_current.PrevNode;
            while(prev != null)
            {
                _current = prev;
                prev = (TreeNodeAccess)_current.PrevNode;
            }

            started = false;
        }

        public void Dispose()
        {

        }
    }
}
