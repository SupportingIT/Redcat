using Redcat.Core;
using System;

namespace Redcat.Amqp.Serialization
{
    public class AmqpListReader : AmqpDataReader
    {
        private uint size;
        private uint count;
        private bool readingList;
        
        public uint ByteSize => size;

        public uint ElementCount => count;

        public bool IsReadingList => readingList;

        public override object Read(ByteBuffer buffer)
        {
            if (IsList(buffer[0]))
            {
                byte code = buffer.ReadByte();
                if (code == DataTypeCodes.List0) return null;                
                InitializeListReading(code, buffer);
            }
            return base.Read(buffer);
        }

        public override T Read<T>(ByteBuffer buffer)
        {
            if (IsList(buffer[0]))
            {
                byte code = buffer.ReadByte();
                if (code == DataTypeCodes.List0) return default(T);
                InitializeListReading(code, buffer);
            }
            return base.Read<T>(buffer);
        }

        private bool IsList(byte code)
        {
            return code == DataTypeCodes.List0 || code == DataTypeCodes.List8 || code == DataTypeCodes.List32;
        }

        private void InitializeListReading(byte code, ByteBuffer buffer)
        {            
            if (code == DataTypeCodes.List8)
            {
                size = buffer.ReadByte();
                count = buffer.ReadByte();
            }
            if (code == DataTypeCodes.List32)
            {
                size = buffer.ReadUInt32();
                count = buffer.ReadUInt32();
            }
            readingList = true;
        }
    }
}
