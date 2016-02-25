using Redcat.Core.Channels;
using System;

namespace Redcat.Core.Net
{
    public class TcpChannelFactory : IChannelFactory<IStreamChannel>
    {
        private const int DefaultBufferSize = 1024;

        public IStreamChannel CreateChannel(ConnectionSettings settings)
        {
            var channel = new TcpChannel(DefaultBufferSize, settings) { AcceptAllCertificates = true };
            if (ChannelCreated != null) ChannelCreated(this, channel);
            return channel;
        }

        public event EventHandler<TcpChannel> ChannelCreated;
    }
}
