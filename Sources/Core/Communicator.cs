using Redcat.Core.Channels;
using System;
using System.Collections.Generic;

namespace Redcat.Core
{
    public class Communicator : ICommunicator, IDisposable
    {
        private IChannelFactory channelFactory;
        private ICollection<Connection> activeConnections;

        public Communicator(IChannelFactory channelFactory)
        {
            this.channelFactory = channelFactory;
            activeConnections = new List<Connection>();
        }

        public IEnumerable<Connection> ActiveConnections
        {
            get { return activeConnections; }
        }

        public void Connect(ConnectionSettings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            IChannel channel = channelFactory.CreateChannel(settings);
            channel.Open();
            Connection conn = new Connection(settings.ConnectionName, channel);            
            activeConnections.Add(conn);
        }

        public void Send<T>(T message)
        {
            throw new NotImplementedException();
        }

        public void Send(Message message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            throw new NotImplementedException();
        }

        public void Dispose()
        { }
    }
}
