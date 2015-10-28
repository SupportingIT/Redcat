using System.IO;

namespace Redcat.Core.Net
{
    public interface INetworkStreamFactory
    {
        Stream CreateSocketStream(ISocket socket, ConnectionSettings settings);
        Stream CreateTlsStream(Stream stream, ConnectionSettings settings);
    }
}
