using Redcat.Core.Channels;
using System;

namespace Redcat.Core
{
    public abstract class CommunicatorBase : DisposableObject, ICommunicator
    {
        private IChannelFactory channelFactory;

        protected CommunicatorBase(IChannelFactory channelFactory)
        {
            if (channelFactory == null) throw new ArgumentNullException(nameof(channelFactory));
            this.channelFactory = channelFactory;
        }

        protected IChannelFactory ChannelFactory => channelFactory;

        public void Connect(ConnectionSettings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));            
            OnConnecting(settings);
            IChannel channel = channelFactory.CreateChannel(settings);
            if (channel == null) throw new InvalidOperationException();
            OnChannelCreated(channel);
            OnConnected();
        }

        public bool IsConnected { get; protected set; }

        protected virtual void OnConnecting(ConnectionSettings settings)
        { }

        protected virtual void OnConnected()
        {
            IsConnected = true;
        }

        protected virtual void OnChannelCreated(IChannel channel)
        {
            ChannelCreated?.Invoke(this, channel);
        }
         
        public void Disconnect()
        {
            OnDisconnecting();
            OnDisconnected();
        }

        protected virtual void OnDisconnecting()
        { }

        protected virtual void OnDisconnected()
        { }

        public event EventHandler<IChannel> ChannelCreated;
    }
}
