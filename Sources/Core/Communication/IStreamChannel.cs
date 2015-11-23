using System.IO;

namespace Redcat.Core.Communication
{
    public interface IStreamChannel : IChannel
    {
        Stream GetStream();
    }

    public interface ISecureStreamChannel : IChannel
    {
        Stream GetSecureStream();
    }
}
