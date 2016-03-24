using System.IO;

namespace Redcat.Amqp.Serializers
{
    public class AmqpFrameSerializer
    {
        private IPayloadSerializer payloadSerializer;
        private MemoryStream buffer;
        private Stream stream;

        public AmqpFrameSerializer(Stream stream, IPayloadSerializer payloadSerializer)
        {
            buffer = new MemoryStream();
            this.payloadSerializer = payloadSerializer;
            this.stream = stream;
        }

        public void Serialize(AmqpFrame frame)
        {            
            uint size = 8;

            if (CanSerializePayload(frame.Payload)) SerializePayload(buffer, frame.Payload);
            size += (uint)buffer.Length;
            
            SerializeHeader(stream, size, 2, frame.Channel);
            buffer.WriteTo(stream);
            buffer.Seek(0, SeekOrigin.Begin);
        }

        private bool CanSerializePayload(object payload)
        {
            return payload != null;
        }

        private void SerializePayload(Stream stream, object payload)
        {
            payloadSerializer.Serialize(stream, payload);
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
