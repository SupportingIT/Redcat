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

        public bool EndOfList => !readingList;

        public bool IsEmptyList(ByteBuffer buffer) => buffer[0] == DataTypeCodes.List0;

        public override object Read(ByteBuffer buffer)
        {
            if (IsList(buffer[0]) && !readingList)
            {
                byte code = buffer.ReadByte();
                if (code == DataTypeCodes.List0) return null;
                InitializeListReading(code, buffer);
            }
            int size = buffer.Count;
            object value = base.Read(buffer);
            UpdateReadingStatus(buffer, size);
            return value;
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

        private void UpdateReadingStatus(ByteBuffer buffer, int previousSize)
        {
            previousSize -= buffer.Count;
            size -= (uint)previousSize;
            count--;
            if (count == 0) readingList = false;
        }

        public void EndRead(ByteBuffer buffer)
        {
            if (!readingList) throw new InvalidOperationException();
            buffer.Discard((int)size);
            size = count = 0;
            readingList = false;
        }
    }
}
