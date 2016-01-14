using System;
using System.Collections.Generic;

namespace Redcat.Core
{
    public class PropertySet
    {
        private IDictionary<string, object> properties;

        public PropertySet()
        {
            properties = new Dictionary<string, object>();
        }

        public object this[string name]
        {
            get { return properties[name]; }
        }

        public T Get<T>(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (!properties.ContainsKey(name) || !(properties[name] is T)) return default(T);
            return (T)properties[name];
        }

        public int GetInt32(string name)
        {
            return Get<int>(name);
        }

        public string GetString(string name)
        {
            return Get<string>(name);
        }

        public void Set<T>(string name, T value)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("setting");
            properties[name] = value;
        }
    }
}
