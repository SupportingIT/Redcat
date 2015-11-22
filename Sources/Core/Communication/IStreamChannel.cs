using System.IO;

namespace Redcat.Core.Communication
{
    public interface IStreamChannel
    {
        Stream GetStream();
    }

    public interface ISecureStreamChannel
    {
        Stream GetSecureStream();
    }
}
