using Redcat.Core.Channels;

namespace Redcat.Core.Net
{
    public class TcpChannelFactory : IStreamChannelFactory
    {
        private const int DefaultBufferSize = 1024;

        public IStreamChannel CreateChannel(ConnectionSettings settings)
        {
            var channel = new TcpChannel(DefaultBufferSize, settings) { AcceptAllCertificates = true };            
            return channel;
        }
    }
}
