using System;
using System.Collections.Generic;

namespace Redcat.Core
{
    public class ConnectionSettings
    {
        private IDictionary<string, object> settings;

        public ConnectionSettings()
        {
            settings = new Dictionary<string, object>();
        }

        public string Domain
        {
            get { return GetString("Domain"); }
            set { Set("Domain", value); }
        }

        public string Host
        {
            get { return GetString("Host"); }
            set { Set("Host", value); }
        }

        public string Username
        {
            get { return GetString("Username"); }
            set { Set("Username", value); }
        }

        public string Password
        {
            get { return GetString("Password"); }
            set { Set("Password", value); }
        }

        public int Port
        {
            get { return GetInt32("Port"); }
            set { Set("Port", value); }
        }

        public object this[string setting]
        {
            get { return settings[setting]; }
        }

        public T Get<T>(string setting)
        {
            if (string.IsNullOrEmpty(setting)) throw new ArgumentNullException("setting");
            if (!settings.ContainsKey(setting)) return default(T);
            return (T) settings[setting];
        }

        public int GetInt32(string setting)
        {
            return Get<int>(setting);
        }

        public string GetString(string setting)
        {
            return Get<string>(setting);
        }

        public void Set(string setting, object value)
        {
            if (string.IsNullOrEmpty(setting)) throw new ArgumentNullException("setting");
            settings[setting] = value;
        }
    }
}
