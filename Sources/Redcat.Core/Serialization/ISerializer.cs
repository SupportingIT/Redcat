using System.IO;

namespace Redcat.Core.Serialization
{
    public interface ISerializer<T>
    {
        void Serialize(Stream stream, T message);
    }
}
