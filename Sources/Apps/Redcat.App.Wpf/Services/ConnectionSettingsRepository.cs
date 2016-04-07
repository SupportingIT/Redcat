using System;
using Redcat.App.Services;
using Redcat.Core;
using Newtonsoft.Json;
using System.IO;

namespace Redcat.App.Wpf.Services
{
    public class ConnectionSettingsRepository : IConnectionSettingsRepository
    {
        private string fileName = "ConnectionSettings.json";

        public ConnectionSettings Get()
        {
            if (!File.Exists(fileName)) return new ConnectionSettings();
            return JsonConvert.DeserializeObject<ConnectionSettings>(File.ReadAllText(fileName));
        }

        public void Save(ConnectionSettings settings)
        {
            string data = JsonConvert.SerializeObject(settings);
            File.WriteAllText(fileName, data);
        }
    }
}
