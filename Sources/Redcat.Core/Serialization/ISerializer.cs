using System.IO;

namespace Redcat.Core.Serializaton
{
    public interface ISerializer<T>
    {
        void Serialize(Stream stream, T message);
    }
}
