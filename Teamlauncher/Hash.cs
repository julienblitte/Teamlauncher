using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Teamlauncher
{
    public class Hash
    {
        protected SHA1 sha;
        protected string seed;
        public Hash()
        {
            this.sha = new SHA1CryptoServiceProvider();
            this.seed = "";
        }

        public Hash(string seed)
        {
            this.sha = new SHA1CryptoServiceProvider();
            this.seed = seed+":";
        }

        public string compute(string content)
        {
            return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(seed+content)));
        }
    }
}
