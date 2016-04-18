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

        public void Write(params byte[] data) => Write(data, 0, data.Length);

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

        public byte ReadByte() => ReadValue((b, i) => b[i], sizeof(byte));

        public sbyte ReadSByte() => ReadValue((b, i) => (sbyte)b[i], sizeof(sbyte));

        public short ReadInt16() => ReadValue((b, i) => b.ReadInt16(i), sizeof(short));

        public ushort ReadUInt16() => ReadValue((b, i) => b.ReadUInt16(i), sizeof(ushort));

        public int ReadInt32() => ReadValue((b, i) => b.ReadInt32(i), sizeof(int));

        public uint ReadUInt32() => ReadValue((b, i) => b.ReadUInt32(i), sizeof(uint));

        public long ReadInt64() => ReadValue((b, i) => b.ReadInt64(i), sizeof(long));

        public ulong ReadUInt64() => ReadValue((b, i) => b.ReadUInt64(i), sizeof(ulong));

        public string ReadString(int length) => ReadValue((b, i) => b.ReadString(length, i), length);

        private T ReadValue<T>(Func<byte[], int, T> valueReader, int valueSize)
        {
            T value = valueReader(buffer, startIndex);
            Discard(valueSize);
            return value;
        }

        public byte PeekByte() => buffer[startIndex];        

        public short PeekInt16() => buffer.ReadInt16();

        public ushort PeekUInt16() => buffer.ReadUInt16();

        public int PeekInt32() => buffer.ReadInt32();

        public uint PeekUInt32() => buffer.ReadUInt32();

        public long PeekInt64() => buffer.ReadInt64();

        public ulong PeekUInt64() => buffer.ReadUInt64();        

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
 

