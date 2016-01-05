using System;

namespace Redcat.Core
{
    public class BinaryData
    {
        public BinaryData(byte[] buffer) : this(buffer, 0, buffer.Length)
        { }

        public BinaryData(byte[] buffer, int offset, int count)
        {
            Buffer = buffer;
            Offset = offset;
            Count = count;
        }

        public byte[] Buffer { get; }

        public int Offset { get; }

        public int Count { get; }
    }
}
