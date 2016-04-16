using Redcat.Core;

namespace Redcat.Amqp.Serialization
{
    public interface IPayloadReader
    {
        object Read(AmqpDataReader reader);
    }
}
