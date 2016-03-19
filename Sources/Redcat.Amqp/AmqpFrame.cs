namespace Redcat.Amqp
{
    public class AmqpFrame : Frame
    {
        public const byte AmqpFrameType = 0x00;

        public AmqpFrame(object payload) : base(AmqpFrameType)
        {
            Payload = payload;
        }

        public ushort Channel { get; set; }        

        public object Payload { get; }
    }
}
