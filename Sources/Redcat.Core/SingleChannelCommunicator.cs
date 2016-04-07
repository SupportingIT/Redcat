using Redcat.Core.Channels;
using System;

namespace Redcat.Core
{
    public class SingleChannelCommunicator<T> : CommunicatorBase<T> where T : IChannel
    {
        public SingleChannelCommunicator(IChannelFactory<T> channelFactory) : base(channelFactory)
        { }

        protected T Channel { get; private set; }

        protected override void OnChannelCreated(T channel)
        {
            base.OnChannelCreated(channel);            
            Channel = channel;
            Channel.Open();
        }

        protected override void OnConnecting(ConnectionSettings settings)
        {
            if (IsConnected) throw new InvalidOperationException();
            base.OnConnecting(settings);
        }

        protected override void OnDisconnecting()
        {
            base.OnDisconnecting();
            Channel.Close();
        }

        protected override void OnDisconnected()
        {
            base.OnDisconnected();
            IsConnected = false;
        }

        protected override void DisposeManagedResources()
        {
            base.DisposeManagedResources();
            Channel.DisposeIfDisposable();
        }
    }
}
