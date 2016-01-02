using Redcat.Core.Channels;

namespace Redcat.Core.Net
{
    public class TcpChannelFactory : IChannelFactory<IStreamChannel>
    {
        public IStreamChannel CreateChannel(ConnectionSettings settings)
        {
            return new TcpChannel(settings);
        }
    }
}
