using Redcat.Core.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Redcat.Core
{
    public class ByteBuffer : IEnumerable<byte>
    {
        private byte[] buffer;
        private int startIndex;
        private int endIndex;

        public ByteBuffer(int bufferSize)
        {
            buffer = new byte[bufferSize];
        }

        public byte this[int index]
        {
            get
            {
                if (index < 0 || index > Count) throw new IndexOutOfRangeException();
                return buffer[startIndex + index];
            }
        }

        public int Available => Capacity - Count;

        public int Capacity => buffer.Length;

        public int Count => endIndex - startIndex;

        public void Discard(int count)
        {
            startIndex += count;
        }

        public void Write(byte[] data) => Write(data, 0, data.Length);

        public void Write(byte[] data, int offset, int count)
        {
            if (Available < count) throw new InvalidOperationException();
            if (Capacity - endIndex < count) DefragBuffer();

            Array.Copy(data, offset, buffer, endIndex, count);
            endIndex += count;
        }

        private void DefragBuffer()
        {
            Array.Copy(buffer, startIndex, buffer, 0, Count);
            endIndex -= startIndex;
            startIndex = 0;
        }

        public byte ReadByte()
        {
            byte value = buffer[startIndex];
            Discard(1);
            return value;
        }

        public short ReadInt16()
        {
            short value = buffer.ReadInt16(startIndex);
            Discard(sizeof(short));
            return value;
        }

        public ushort ReadUInt16()
        {
            ushort value = buffer.ReadUInt16(startIndex);
            Discard(sizeof(ushort));
            return value;
        }

        public int ReadInt32()
        {
            int value = buffer.ReadInt32(startIndex);
            Discard(sizeof(int));
            return value;
        }

        public uint ReadUInt32()
        {
            uint value = buffer.ReadUInt32(startIndex);
            Discard(sizeof(uint));
            return value;
        }

        public IEnumerator<byte> GetEnumerator()
        {
            return buffer.Skip(startIndex).Take(Count).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return Encoding.UTF8.GetString(buffer, startIndex, Count);
        }

        public string ToString(int offset, int count)
        {
            return Encoding.UTF8.GetString(buffer, startIndex + offset, count);
        }
    }
}
 

