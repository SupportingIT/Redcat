using System;

namespace Redcat.Core
{
    public class ConnectionCommand
    {
        private ConnectionCommand(string name, ConnectionCommandType commandType, ConnectionSettings settings)
        {
            ConnectionName = name;
            CommandType = commandType;
            Settings = settings;
        }

        internal string ConnectionName { get; }

        internal ConnectionCommandType CommandType { get; }

        internal ConnectionSettings Settings { get; }

        public static ConnectionCommand Open(ConnectionSettings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            return new ConnectionCommand(null, ConnectionCommandType.Open, settings);
        }

        public static ConnectionCommand Close(string connectionName)
        {
            return new ConnectionCommand(connectionName, ConnectionCommandType.Close, null);
        }
    }

    internal enum ConnectionCommandType { Open, Close }
}
