using Redcat.Core.Channels;
using System.Linq;
using System;
using Redcat.Core;
using Redcat.Amqp.Serializers;

namespace Redcat.Amqp.Channels
{
    public class AmqpChannel : ReactiveChannelBase<AmqpFrame>, IAmqpChannel
    {        
        private readonly byte[] amqpHeader = { (byte)'A', (byte)'M', (byte)'Q', (byte)'P', 0, 1, 0, 0 };

        public AmqpChannel(IReactiveStreamChannel streamChannel, ConnectionSettings settings) : base(streamChannel, settings)
        { }

        private void InitializeChannel()
        {
            byte[] response = new byte[8];
            Stream.Write(amqpHeader, 0, amqpHeader.Length);
            Stream.Read(response, 0, 8);
            if (!IsValidAmqpHeader(response)) throw new InvalidOperationException();
        }

        private bool IsValidAmqpHeader(byte[] header)
        {
            return amqpHeader.SequenceEqual(header);
        }

        protected override IReactiveDeserializer<AmqpFrame> CreateDeserializer()
        {
            return new AmqpFrameDeserializer();
        }

        protected override ISerializer<AmqpFrame> CreateSerializer()
        {
            IPayloadSerializer payloadSerializer = new PayloadSerializer();
            return new AmqpFrameSerializer(payloadSerializer);
        }
    }
}
