using Redcat.Core.Channels;
using Redcat.Core;
using Redcat.Amqp.Serialization;
using System;

namespace Redcat.Amqp.Channels
{
    public class AmqpChannel : ReactiveMessageChannel<AmqpFrame, AmqpSerializer, AmqpDeserializer>, IAmqpChannel
    {
        public AmqpChannel(IReactiveStreamChannel streamChannel, ConnectionSettings settings) : base(streamChannel, settings)
        { }

        protected override void OnOpen()
        {
            base.OnOpen();
            Send(Protocols.AmqpV100);
        }

        protected override AmqpDeserializer CreateDeserializer()
        {
            OpenPerformativeReader reader = new OpenPerformativeReader();
            var deserializer = new AmqpDeserializer(reader);
            deserializer.ProtocolHeaderDeserialized += h => OnProtocolHeaderReceived(h);
            return deserializer;
        }

        protected override AmqpSerializer CreateSerializer()
        {
            IPayloadSerializer payloadSerializer = new PayloadSerializer();
            return new AmqpSerializer(payloadSerializer);
        }

        protected void OnProtocolHeaderReceived(ProtocolHeader header)
        {
            ProtocolHeaderReceived?.Invoke(this, header);
        }

        public void Send(ProtocolHeader header)
        {
            Serializer.Serialize(Stream, header);
        }

        public event EventHandler<ProtocolHeader> ProtocolHeaderReceived;
    }
}
