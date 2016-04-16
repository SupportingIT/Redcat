namespace Redcat.Amqp
{
    public class AmqpFrame
    {
        public const byte AmqpFrameType = 0x00;

        public AmqpFrame(object payload, ushort channel = 0)
        {
            Payload = payload;
            Channel = channel;
        }

        public ushort Channel { get; set; }        

        public object Payload { get; }
    }
}
