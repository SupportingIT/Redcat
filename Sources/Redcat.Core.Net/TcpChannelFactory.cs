using Redcat.Core.Channels;
using System;

namespace Redcat.Core.Net
{
    public class TcpChannelFactory : IChannelFactory<IStreamChannel>
    {
        public IStreamChannel CreateChannel(ConnectionSettings settings)
        {
            var channel = new TcpChannel(settings) { AcceptAllCertificates = true };
            if (ChannelCreated != null) ChannelCreated(this, channel);
            return channel;
        }

        public event EventHandler<TcpChannel> ChannelCreated;
    }
}
