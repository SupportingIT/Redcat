using Redcat.Core.Channels;
using Redcat.Core;
using Redcat.Amqp.Serialization;
using System;

namespace Redcat.Amqp.Channels
{
    public class AmqpChannel : ReactiveChannelBase<AmqpFrame>, IAmqpChannel
    {
        public AmqpChannel(IReactiveStreamChannel streamChannel, ConnectionSettings settings) : base(streamChannel, settings)
        { }        

        protected override IReactiveDeserializer<AmqpFrame> CreateDeserializer()
        {
            var deserializer = new AmqpDeserializer();
            deserializer.ProtocolHeaderDeserialized += h => OnProtocolHeaderReceived(h);
            return deserializer;
        }

        protected override ISerializer<AmqpFrame> CreateSerializer()
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
            throw new NotImplementedException();
        }

        public event EventHandler<ProtocolHeader> ProtocolHeaderReceived;
    }
}
