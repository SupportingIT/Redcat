using System.IO;

namespace Redcat.Core.Net
{
    public interface INetworkStreamFactory
    {
        Stream CreateStream(ConnectionSettings settings);
        Stream CreateSecuredStream(Stream stream, ConnectionSettings settings);
    }
}
