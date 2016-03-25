using System.IO;

namespace Redcat.Amqp.Serializers
{
    public interface IPayloadSerializer
    {
        void Serialize(Stream stream, object payload);
    }
}
