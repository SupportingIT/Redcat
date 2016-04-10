using System;
using Redcat.Core;
using Redcat.Core.Serializaton;

namespace Redcat.Amqp.Serialization
{
    public class AmqpDeserializer : ReactiveDeserializerBase<AmqpFrame>
    {
        const int DefaultBufferSize = 1024;

        public AmqpDeserializer() : base(DefaultBufferSize)
        { }        

        protected override void OnBufferUpdated()
        {
            if (IsProtocolHeader(Buffer)) DeserializeProtocolHeader(Buffer);
        }

        private bool IsProtocolHeader(ByteBuffer buffer)
        {
            if (buffer.Count != 8) return false;
            if (buffer[0] != (byte)'A' && buffer[1] != (byte)'M' && 
                buffer[2] != (byte)'Q' && buffer[3] != 'P') return false;
            return true;
        }

        private void DeserializeProtocolHeader(ByteBuffer buffer)
        {
            var header = new ProtocolHeader((ProtocolType)buffer[4], new Version(buffer[5], buffer[6], buffer[7]));
            OnProtocolHeaderDeserialized(header);
        }

        protected void OnProtocolHeaderDeserialized(ProtocolHeader header)
        {
            ProtocolHeaderDeserialized?.Invoke(header);
        }

        public event Action<ProtocolHeader> ProtocolHeaderDeserialized;
    }
}
