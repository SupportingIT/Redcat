using Redcat.Core.Channels;
using System;

namespace Redcat.Core
{
    public abstract class CommunicatorBase<T> : DisposableObject, ICommunicator where T : IChannel
    {
        private IChannelFactory<T> channelFactory;

        protected CommunicatorBase(IChannelFactory<T> channelFactory)
        {
            if (channelFactory == null) throw new ArgumentNullException(nameof(channelFactory));
            this.channelFactory = channelFactory;
        }

        protected IChannelFactory<T> ChannelFactory => channelFactory;

        public void Connect(ConnectionSettings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));            
            OnConnecting(settings);
            T channel = channelFactory.CreateChannel(settings);
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

        protected virtual void OnChannelCreated(T channel)
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

        public event EventHandler<T> ChannelCreated;
    }
}
