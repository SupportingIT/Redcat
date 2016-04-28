using System;
using Redcat.Core;
using Redcat.Core.Serialization;

namespace Redcat.Amqp.Serialization
{
    public class AmqpDeserializer : ReactiveDeserializerBase<AmqpFrame>
    {
        const int DefaultBufferSize = 1024;
        const int MinFrameSize = 8;
        const int MinDoffValue = 2;
                
        private IByteBufferReader reader;

        public AmqpDeserializer(IByteBufferReader reader) : base(DefaultBufferSize)
        {
            this.reader = reader;
        }

        protected override void OnBufferUpdated()
        {
            while (Buffer.Count >= MinFrameSize)
            {
                if (IsProtocolHeader(Buffer)) DeserializeProtocolHeader(Buffer);
                else DeserializeFrame(Buffer);
            }
        }

        private bool IsProtocolHeader(ByteBuffer buffer)
        {            
            if (buffer[0] != (byte)'A' && buffer[1] != (byte)'M' && 
                buffer[2] != (byte)'Q' && buffer[3] != (byte)'P') return false;
            return true;
        }

        private void DeserializeProtocolHeader(ByteBuffer buffer)
        {
            var header = new ProtocolHeader((ProtocolType)buffer[4], new Version(buffer[5], buffer[6], buffer[7]));
            buffer.Discard(8);
            OnProtocolHeaderDeserialized(header);
        }

        protected void OnProtocolHeaderDeserialized(ProtocolHeader header)
        {
            ProtocolHeaderDeserialized?.Invoke(header);
        }

        private void DeserializeFrame(ByteBuffer buffer)
        {
            uint size = buffer.ReadUInt32();            
            byte doff = buffer.ReadByte();            
            byte type = buffer.ReadByte();

            if (size < 8 || doff < 2) throw new FormatException();
            if (type == 0) DeserializeAmqpFrame(buffer, size - (uint)(doff * 4));
            if (type == 1) DeserializeSaslFrame(buffer);
        }

        private void DeserializeAmqpFrame(ByteBuffer buffer, uint payloadLength)
        {
            ushort channel = buffer.ReadUInt16();
            object payload = reader.Read(buffer);
            
            AmqpFrame frame = new AmqpFrame(payload, channel);            
            OnDeserialized(frame);
        }

        private void DeserializeSaslFrame(ByteBuffer buffer)
        {            
            throw new NotImplementedException();
        }

        public event Action<ProtocolHeader> ProtocolHeaderDeserialized;
    }
}
