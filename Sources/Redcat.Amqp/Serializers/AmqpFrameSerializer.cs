using Redcat.Core.Channels;
using System.IO;

namespace Redcat.Amqp.Serializers
{
    public class AmqpFrameSerializer : ISerializer<AmqpFrame>
    {        
        private IPayloadSerializer payloadSerializer;
        private MemoryStream buffer;        
        private AmqpDataWriter writer;

        public AmqpFrameSerializer(IPayloadSerializer payloadSerializer)
        {
            buffer = new MemoryStream();
            this.payloadSerializer = payloadSerializer;            
            writer = new AmqpDataWriter(buffer);
        }

        public void Serialize(Stream stream, AmqpFrame frame)
        {            
            uint size = 8;

            if (CanSerializePayload(frame.Payload)) SerializePayload(frame.Payload);
            size += (uint)buffer.Length;
            
            SerializeHeader(stream, size, 2, frame.Channel);
            buffer.WriteTo(stream);
            buffer.Seek(0, SeekOrigin.Begin);
        }

        private bool CanSerializePayload(object payload)
        {
            return payload != null;
        }

        private void SerializePayload(object payload)
        {
            payloadSerializer.Serialize(writer, payload);
        }

        private void SerializeHeader(Stream stream, uint size, byte offset, ushort channel)
        {
            stream.Write(size);
            stream.WriteByte(offset);
            stream.WriteByte(AmqpFrame.AmqpFrameType);
            stream.Write(channel);
        }
    }    
}
