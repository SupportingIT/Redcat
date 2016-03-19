using System;
using System.Collections.Generic;
using System.IO;

namespace Redcat.Amqp.Serializers
{
    public class AmqpFrameSerializer
    {        
        private Stream stream;

        private IDictionary<Type, PayloadSerializer> payloadSerializers;

        public AmqpFrameSerializer(Stream stream)
        {
            payloadSerializers = new Dictionary<Type, PayloadSerializer>();
            this.stream = stream;
        }

        public void AddPayloadSerializer(Type payloadType, PayloadSerializer serializer)
        {
            payloadSerializers.Add(payloadType, serializer);
        }

        public void Serialize(AmqpFrame frame)
        {
            var buffer = new MemoryStream();
            uint size = 8;

            if (CanSerializePayload(frame.Payload)) SerializePayload(buffer, frame.Payload);
            size += (uint)buffer.Length;
            
            SerializeHeader(stream, size, 2, frame.Channel);
            buffer.Seek(0, SeekOrigin.Begin);
            buffer.CopyTo(stream);
            buffer.Flush();
        }

        private bool CanSerializePayload(object payload)
        {
            return payload != null && payloadSerializers.ContainsKey(payload.GetType());
        }

        private void SerializePayload(Stream stream, object payload)
        {
            var serializer = payloadSerializers[payload.GetType()];
            serializer.Invoke(stream, payload);
        }

        private void SerializeHeader(Stream stream, uint size, byte offset, ushort channel)
        {
            stream.Write(size);
            stream.WriteByte(offset);
            stream.WriteByte(AmqpFrame.AmqpFrameType);
            stream.Write(channel);
        }
    }

    public delegate void PayloadSerializer(Stream stream, object payload);
}
