using Redcat.Core.Channels;
using System.IO;
using System;

namespace Redcat.Amqp.Serialization
{
    public class AmqpSerializer : ISerializer<AmqpFrame>, ISerializer<ProtocolHeader>
    {        
        private IPayloadSerializer payloadSerializer;
        private MemoryStream buffer;        
        private AmqpDataWriter writer;

        public AmqpSerializer(IPayloadSerializer payloadSerializer)
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

        public void Serialize(Stream stream, ProtocolHeader header)
        {
            stream.WriteByte((byte)'A');
            stream.WriteByte((byte)'M');
            stream.WriteByte((byte)'Q');
            stream.WriteByte((byte)'P');

            stream.WriteByte((byte)header.ProtocolType);
            SerializeProtocolVersion(stream, header.Version);
        }

        private void SerializeProtocolVersion(Stream stream, Version version)
        {
            stream.WriteByte((byte)version.Major);
            stream.WriteByte((byte)version.Minor);
            stream.WriteByte((byte)version.Build);
        }
    }    
}
