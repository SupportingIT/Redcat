using System.IO;

namespace Redcat.Core.Channels
{
    public interface ISerializer<T>
    {
        void Serialize(Stream stream, T message);
    }
}
