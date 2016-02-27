using Redcat.Core.Channels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Redcat.Core
{
    public class Communicator : ICommunicator, IDisposable
    {
        private IChannelFactory channelFactory;
        private IMessageDispatcher dispatcher;        

        public Communicator(IChannelFactory channelFactory, IMessageDispatcher dispatcher)
        {
            this.channelFactory = channelFactory;
            this.dispatcher = dispatcher;
        }

        public void Connect(ConnectionSettings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));            
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }        

        public async Task ConnectAsync(ConnectionSettings settings)
        {
            await Task.Run(() => Connect(settings));
        }        

        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public void Send<T>(T message) where T : class
        {
            dispatcher.Dispatch(message);
        }

        public void Dispose()
        { }
    }
}
