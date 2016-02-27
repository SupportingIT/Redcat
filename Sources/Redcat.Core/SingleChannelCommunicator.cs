using Redcat.Core.Channels;
using System;

namespace Redcat.Core
{
    public class SingleChannelCommunicator : DisposableObject, ICommunicator
    {
        private IChannelFactory channelFactory;
        private IChannel channel;

        public SingleChannelCommunicator(IChannelFactory channelFactory)
        {
            this.channelFactory = channelFactory;
        }

        public void Connect(ConnectionSettings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            channel = channelFactory.CreateChannel(settings);            
            if (channel == null) throw new InvalidOperationException();
            channel.Open();
        }

        public void Disconnect()
        {
            if (channel == null) return;
            channel.Close();
        }

        public void Send<T>(T message) where T : class
        {
            ThrowIfDisposed(nameof(SingleChannelCommunicator));
            if (message == null) throw new ArgumentNullException(nameof(message));
            var outputChannel = channel as IOutputChannel<T>;
            if (outputChannel == null) throw new InvalidOperationException();
            outputChannel.Send(message);
        }

        public T Receive<T>()
        {
            var inChannel = channel as IInputChannel<T>;
            if (inChannel == null) throw new InvalidOperationException();
            return inChannel.Receive();
        }

        protected override void DisposeManagedResources()
        {
            base.DisposeManagedResources();
            channel.DisposeIfDisposable();
        }
    }
}
