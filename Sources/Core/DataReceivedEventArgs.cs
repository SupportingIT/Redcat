using System;

namespace Redcat.Core
{
    public class DataReceivedEventArgs : EventArgs
    {
        public DataReceivedEventArgs(byte[] buffer) : this(buffer, 0, buffer.Length)
        { }

        public DataReceivedEventArgs(byte[] buffer, int offset, int count)
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
