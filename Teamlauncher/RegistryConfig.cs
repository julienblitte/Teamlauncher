using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teamlauncher
{
    class RegistryConfig : IDisposable
    {
        protected const string REGISTRY_PATH = @"Software\Teamlauncher";
        private RegistryKey configKey;

        public const string REGISTRY_KEY_LOCATION_X = "LocationX";
        public const string REGISTRY_KEY_LOCATION_Y = "LocationY";
        public const string REGISTRY_KEY_WIN_HEIGHT = "WindowHeight";
        public const string REGISTRY_KEY_WIN_WIDTH = "WindowWidth";
        public const string REGISTRY_KEY_FOLDING = "Folding";
        public const string REGISTRY_KEY_EDITOR = "Editor";
        public const string REGISTRY_KEY_SERVER_ADDRESS = "server";
        public const string REGISTRY_KEY_SERVER_PASSWORD = "password";
        public const string REGISTRY_KEY_PORT = "port";
        public const string REGISTRY_KEY_PASSWORD = "password";
        public const string REGISTRY_KEY_DATABASE = "Database";
        public const string REGISTRY_KEY_CLOSETIP = "closeTip";
        public const string REGISTRY_KEY_WIN_TOPMOST = "stayOnTop";
        public const string REGISTRY_KEY_DEBUG = "debug";
        public const string REGISTRY_KEY_MODE = "workingMode";
        public const string REGISTRY_KEY_WIN_LOCKITEMS = "lockItems";
        public const string REGISTRY_KEY_WIN_AUTOREDUCE = "autoReduce";
        public const string REGISTRY_KEY_WEB_LOCALOPAQUE = "localOpaque";

        public RegistryConfig()
        {
            configKey = null;

            if (openConfigKey() == null)
            {
                Trace.WriteLine("Registry: unable to access to '" + REGISTRY_PATH + "'");
            }
        }

        private RegistryKey openConfigKey()
        {
            if (configKey != null)
                return configKey;

            configKey = Registry.CurrentUser.OpenSubKey(REGISTRY_PATH, true);
            if (configKey == null)
            {
                configKey = Registry.CurrentUser.CreateSubKey(REGISTRY_PATH);
            }

            return configKey;
        }

        public string readString(string variable)
        {
            object result;

            if (openConfigKey() == null)
                return "";

            result = configKey.GetValue(variable);
            if (result == null || result.GetType() != typeof(String))
                return "";

            return (string)result;
        }
        public void writeString(string variable, string value)
        {
            if (openConfigKey() == null)
                return;

            configKey.SetValue(variable, value, RegistryValueKind.String);
        }

        public byte[] readBinary(string variable)
        {
            object result;

            if (openConfigKey() == null)
                return new byte[0];

            result = configKey.GetValue(variable);
            if (result == null || result.GetType() != typeof(byte[]))
                return new byte[0];

            return (byte[])result;
        }
        public void writeBinary(string variable, byte[] value)
        {
            if (openConfigKey() == null)
                return;

            configKey.SetValue(variable, value, RegistryValueKind.Binary);
        }

        public int readInteger(string variable)
        {
            object result;

            if (openConfigKey() == null)
                return 0;

            result = configKey.GetValue(variable);
            if (result == null || result.GetType() != typeof(Int32))
                return 0;

            return (int)result;
        }
        public void writeInteger(string variable, int value)
        {
            if (openConfigKey() == null)
                return;

            configKey.SetValue(variable, value, RegistryValueKind.DWord);
        }

        public bool readBool(string variable)
        {
            object result;

            if (openConfigKey() == null)
                return false;

            result = configKey.GetValue(variable);
            if (result == null || result.GetType() != typeof(Int32))
                return false;

            return ((int)result) != 0;
        }
        public void writeBool(string variable, bool value)
        {
            if (openConfigKey() == null)
                return;

            configKey.SetValue(variable, (value ? 1 : 0), RegistryValueKind.DWord);
        }

        public long readLong(string variable)
        {
            object result;

            if (openConfigKey() == null)
                return 0;

            result = configKey.GetValue(variable);
            if (result == null || result.GetType() != typeof(Int64))
                return 0;

            return (long)result;
        }
        public void writeLong(string variable, long value)
        {
            if (openConfigKey() == null)
                return;

            configKey.SetValue(variable, value, RegistryValueKind.QWord);
        }

        public string[] readText(string variable)
        {
            object result;

            if (openConfigKey() == null)
                return new string[0];

            result = configKey.GetValue(variable);
            if (result == null || result.GetType() != typeof(String[]))
                return new string[0];

            return (string[])result;
        }
        public void writeText(string variable, string[] value)
        {
            if (openConfigKey() == null)
                return;

            configKey.SetValue(variable, value, RegistryValueKind.MultiString);
        }

        public void Dispose()
        {
            configKey?.Dispose();
        }
    }
}
