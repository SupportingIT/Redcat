using Redcat.Core.Channels;
using System;

namespace Redcat.Core
{
    public class SingleChannelCommunicator : CommunicatorBase
    {
        public SingleChannelCommunicator(IChannelFactory channelFactory) : base(channelFactory)
        { }

        protected IChannel Channel { get; private set; }

        protected override void OnChannelCreated(IChannel channel)
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
