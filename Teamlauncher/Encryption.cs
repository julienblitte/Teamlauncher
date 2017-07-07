using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Teamlauncher
{
    delegate string getPassword();
    delegate bool checkPassword(string hash);

    class Encryption : IDisposable
    {
        private RijndaelManaged rijndael;

        private byte[] iv = null;
        private byte[] key = null;

        public Encryption(string password)
        {
            Rfc2898DeriveBytes rfcDb;

            rijndael = new RijndaelManaged();
            rijndael.Mode = CipherMode.CBC;

            rfcDb = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(password));
            key = rfcDb.GetBytes(16);
            iv = rfcDb.GetBytes(16);
        }

        public string EncryptString(string data)
        {
            // Place le texte à chiffrer dans un tableau d'octets
            byte[] plainText = Encoding.UTF8.GetBytes(data);

            // Crée le chiffreur AES - Rijndael
            ICryptoTransform aesEncryptor = rijndael.CreateEncryptor(key, iv);

            MemoryStream ms = new MemoryStream();

            // Ecris les données chiffrées dans le MemoryStream
            CryptoStream cs = new CryptoStream(ms, aesEncryptor, CryptoStreamMode.Write);
            cs.Write(plainText, 0, plainText.Length);
            cs.FlushFinalBlock();

            // Place les données chiffrées dans un tableau d'octet
            byte[] CipherBytes = ms.ToArray();

            ms.Close();
            cs.Close();

            // Place les données chiffrées dans une chaine encodée en Base64
            return Convert.ToBase64String(CipherBytes);
        }

        public string DecryptString(string data)
        {
            try
            {
                // Place le texte à déchiffrer dans un tableau d'octets
                byte[] cipheredData = Convert.FromBase64String(data);

                // Ecris les données déchiffrées dans le MemoryStream
                ICryptoTransform decryptor = rijndael.CreateDecryptor(key, iv);
                MemoryStream ms = new MemoryStream(cipheredData);
                CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);

                // Place les données déchiffrées dans un tableau d'octet
                byte[] plainTextData = new byte[cipheredData.Length];

                int decryptedByteCount = cs.Read(plainTextData, 0, plainTextData.Length);

                ms.Close();
                cs.Close();

                return Encoding.UTF8.GetString(plainTextData, 0, decryptedByteCount);
            }
            catch(Exception)
            {
                return null;
            }
        }

        ~Encryption()
        {
            Dispose();
        }

        public void Dispose()
        {
            rijndael?.Dispose();
            iv = null;
            key = null;
        }
    }
}
