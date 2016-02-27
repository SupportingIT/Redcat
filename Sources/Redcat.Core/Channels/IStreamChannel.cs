using System.IO;

namespace Redcat.Core.Channels
{
    public interface IStreamChannel : IChannel
    {
        Stream GetStream();
    }

    public interface ISecureStreamChannel : IStreamChannel
    {
        Stream GetSecuredStream();
    }
}
