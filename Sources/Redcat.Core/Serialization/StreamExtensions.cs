using System.IO;

namespace Redcat.Core.Serialization
{
    /// <summary>
    /// Set of extension methods which serializes base data type into stream.
    /// Unlike BinaryWriter uses big-endian order.
    /// </summary>
    public static class StreamExtensions
    {
        public static void Write(this Stream stream, short value) => Write(stream, (ushort)value);

        public static void Write(this Stream stream, ushort value)
        {
            stream.WriteByte((byte)((value & 0xff00) >> 8));
            stream.WriteByte((byte)((value & 0x00ff)));
        }

        public static void Write(this Stream stream, int value) => Write(stream, (uint)value);

        public static void Write(this Stream stream, uint value)
        {
            stream.WriteByte((byte)((value & 0xff000000) >> 24));
            stream.WriteByte((byte)((value & 0x00ff0000) >> 16));
            stream.WriteByte((byte)((value & 0x0000ff00) >> 8));
            stream.WriteByte((byte)((value & 0x000000ff)));
        }

        public static void Write(this Stream stream, long value) => Write(stream, (ulong)value);

        public static void Write(this Stream stream, ulong value)
        {
            stream.WriteByte((byte)((value & 0xff00000000000000) >> 56));
            stream.WriteByte((byte)((value & 0x00ff000000000000) >> 48));
            stream.WriteByte((byte)((value & 0x0000ff0000000000) >> 40));
            stream.WriteByte((byte)((value & 0x000000ff00000000) >> 32));

            stream.WriteByte((byte)((value & 0x00000000ff000000) >> 24));
            stream.WriteByte((byte)((value & 0x0000000000ff0000) >> 16));
            stream.WriteByte((byte)((value & 0x000000000000ff00) >> 8));
            stream.WriteByte((byte)((value & 0x00000000000000ff)));
        }
    }
}
