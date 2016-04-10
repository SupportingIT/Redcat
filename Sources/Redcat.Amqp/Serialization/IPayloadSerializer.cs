using System.IO;

namespace Redcat.Amqp.Serialization
{
    public interface IPayloadSerializer
    {
        void Serialize(AmqpDataWriter writer, object payload);
    }
}
