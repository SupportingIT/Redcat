using Redcat.Core.Channels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Redcat.Core
{
    public class Communicator : ICommunicator, IDisposable, IObserver<ConnectionCommand>
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
            AddConnection(settings, channel);
        }

        public async Task ConnectAsync(ConnectionSettings settings)
        {
            IChannel channel = channelFactory.CreateChannel(settings);
            IAsyncChannel asyncChannel = channel as IAsyncChannel;
            if (asyncChannel != null) await asyncChannel.OpenAsync();
            else await Task.Run(() => channel.Open());
            AddConnection(settings, channel);
        }

        private void AddConnection(ConnectionSettings settings, IChannel channel)
        {
            Connection conn = new ChannelConnection(settings.ConnectionName, channel);
            conn.Closing += OnClosingConnection;
            activeConnections.Add(conn);
        }

        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public void OnNext(ConnectionCommand command)
        {
            if (command.CommandType == ConnectionCommandType.Open) Connect(command.Settings);
            if (command.CommandType == ConnectionCommandType.Close) CloseConnection(command.ConnectionName);
        }

        private void CloseConnection(string connectionName)
        {
            var connection = ActiveConnections.FirstOrDefault(c => c.Name == connectionName);
            if (connection == null) return;
            CloseConnection(connection);
        }

        private void CloseConnection(Connection connection)
        {
            connection.Closing -= OnClosingConnection;
            activeConnections.Remove(connection);
        }

        private void OnClosingConnection(object sender, EventArgs args) => CloseConnection((Connection)sender);        

        public void Dispose()
        { }
    }
}
