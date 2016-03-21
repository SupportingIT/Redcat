using Redcat.Amqp.Performatives;

namespace Redcat.Amqp.Serializers
{
    public static class PayloadSerializers
    {
        public static void SerializeOpenPerformative(AmqpDataWriter writer, Open performative)
        { }

        public static void SerializeClosePerformative(AmqpDataWriter writer, Close performative)
        { }
    }
}
