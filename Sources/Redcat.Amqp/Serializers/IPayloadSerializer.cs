using System.IO;

namespace Redcat.Amqp.Serializers
{
    public interface IPayloadSerializer
    {
        void Serialize(AmqpDataWriter writer, object payload);
    }
}
