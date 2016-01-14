using Redcat.Core.Channels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Redcat.Core
{
    public class MessageDispatcher// : IMessageDispatcher
    {
        public Connection DefaultConnection { get; set; }

        public IEnumerable<Connection>ActiveConnections { get; set; }

        public void Dispatch<T>(T message)
        {
            if (ActiveConnections.Count() == 0) throw new InvalidOperationException("No active connections");            
            var connection = SelectConnection(ActiveConnections, message);
            if (connection == null) throw new InvalidOperationException("No connections to send message " + message);
            SendMessage(connection, message);
        }

        private Connection SelectConnection<T>(IEnumerable<Connection> connections, T message)
        {
            return null;
        }

        private void SendMessage<T>(Connection connection, T message)
        {
            throw new NotImplementedException();
        }
    }
}
