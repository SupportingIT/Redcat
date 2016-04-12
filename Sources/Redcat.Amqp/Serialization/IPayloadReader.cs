using Redcat.Core;

namespace Redcat.Amqp.Serialization
{
    public interface IPayloadReader
    {
        object Deserialize(AmqpDataReader reader);
    }
}
