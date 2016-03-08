namespace Redcat.Amqp
{
    public class AmqpFrame : Frame
    {
        public const byte AmqpFrameType = 0x00;

        public AmqpFrame(string performative) : base(AmqpFrameType)
        {
            Performative = performative;
        }

        public ushort Channel { get; set; }

        public string Performative { get; }

        public object Payload { get; set; }
    }
}
