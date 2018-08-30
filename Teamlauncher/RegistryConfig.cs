using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teamlauncher
{
    class RegistryConfig : IDisposable
    {
        protected const string REGISTRY_CONFIG_PATH = @"Software\Teamlauncher";
        private RegistryKey configKey;

        public RegistryConfig()
        {
            configKey = Registry.CurrentUser.OpenSubKey(REGISTRY_CONFIG_PATH, true);
            if (configKey == null)
            {
                Registry.CurrentUser.CreateSubKey(REGISTRY_CONFIG_PATH);
            }
        }

        public string readString(string variable)
        {
            object result;

            result = configKey.GetValue(variable);
            if (result == null || result.GetType() != typeof(String))
                return "";

            return (string)result;
        }
        public void writeString(string variable, string value)
        {
            configKey.SetValue(variable, value, RegistryValueKind.String);
        }

        public byte[] readBinary(string variable)
        {
            object result;

            result = configKey.GetValue(variable);
            if (result == null || result.GetType() != typeof(byte[]))
                return new byte[0];

            return (byte[])result;
        }
        public void writeBinary(string variable, byte[] value)
        {
            configKey.SetValue(variable, value, RegistryValueKind.Binary);
        }

        public int readInteger(string variable)
        {
            object result;

            result = configKey.GetValue(variable);
            if (result == null || result.GetType() != typeof(Int32))
                return 0;

            return (int)result;
        }
        public void writeInteger(string variable, int value)
        {
            configKey.SetValue(variable, value, RegistryValueKind.DWord);
        }

        public bool readBool(string variable)
        {
            object result;

            result = configKey.GetValue(variable);
            if (result == null || result.GetType() != typeof(Int32))
                return false;

            return ((int)result) != 0;
        }
        public void writeBool(string variable, bool value)
        {
            configKey.SetValue(variable, (value ? 1 : 0), RegistryValueKind.DWord);
        }

        public long readLong(string variable)
        {
            object result;

            result = configKey.GetValue(variable);
            if (result == null || result.GetType() != typeof(Int64))
                return 0;

            return (long)result;
        }
        public void writeLong(string variable, long value)
        {
            configKey.SetValue(variable, value, RegistryValueKind.QWord);
        }

        public string[] readText(string variable)
        {
            object result;

            result = configKey.GetValue(variable);
            if (result == null || result.GetType() != typeof(String[]))
                return new string[0];

            return (string[])result;
        }
        public void writeText(string variable, string[] value)
        {
            configKey.SetValue(variable, value, RegistryValueKind.MultiString);
        }

        public void Dispose()
        {
            configKey?.Dispose();
        }
    }
}
