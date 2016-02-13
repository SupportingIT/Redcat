using System.Collections.Generic;

namespace Redcat.Core
{
    public interface ICommunicator
    {
        IEnumerable<Connection> ActiveConnections { get; }

        void Connect(ConnectionSettings settings);
        void Send<T>(T message) where T : class;
    }
}
