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
        private IMessageDispatcher dispatcher;
        private ICollection<Connection> activeConnections;

        public Communicator(IChannelFactory channelFactory, IMessageDispatcher dispatcher)
        {
            this.channelFactory = channelFactory;
            this.dispatcher = dispatcher;
            activeConnections = new List<Connection>();
        }

        public IEnumerable<Connection> ActiveConnections
        {
            get { return activeConnections; }
        }

        public void Connect(ConnectionSettings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            Connection connection = OpenConnection(settings);
            activeConnections.Add(connection);
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        private Connection OpenConnection(ConnectionSettings settings)
        {
            IChannel channel = channelFactory.CreateChannel(settings);
            channel.Open();
            Connection connection = CreateConnection(settings.ConnectionName, channel);
            connection.Closing += OnClosingConnection;
            return connection;
        }

        private Connection CreateConnection(string name, IChannel channel)
        {
            return new ChannelConnection(name, channel);
        }

        public async Task ConnectAsync(ConnectionSettings settings)
        {
            await Task.Run(() => Connect(settings));
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

        public void Send<T>(T message) where T : class
        {
            dispatcher.Dispatch(message);
        }

        public void Dispose()
        { }
    }
}
