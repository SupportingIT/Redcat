using System;
using System.Text;

namespace Redcat.Core.Serialization
{
    /// <summary>
    /// Set of extensions methods which converts binary data to base data types.
    /// Unlike BitConverter those methods read data in big-endian order
    /// </summary>
    public static class ByteArrayExtensions
    {
        public static short ReadInt16(this byte[] array, int offset = 0)
        {                  
            return (short)(array[offset] << 8 | array[offset + 1]);            
        }

        public static ushort ReadUInt16(this byte[] array, int offset = 0)
        {
            return (ushort)(array[offset] << 8 | array[offset + 1]);
        }

        public static int ReadInt32(this byte[] array, int offset = 0)
        {
            int value = array[offset] << 8 | array[offset + 1];
            value = value << 8 | array[offset + 2];
            return value << 8 | array[offset + 3];
        }

        public static uint ReadUInt32(this byte[] array, int offset = 0)
        {
            uint value = (uint)(array[offset] << 8 | array[offset + 1]);
            value = value << 8 | array[offset + 2];
            return value << 8 | array[offset + 3];
        }

        public static long ReadInt64(this byte[] array, int offset = 0)
        {
            long value = array[offset] << 8 | array[offset + 1];
            value = value << 8 | array[offset + 2];
            value = value << 8 | array[offset + 3];
            value = value << 8 | array[offset + 4];
            value = value << 8 | array[offset + 5];
            value = value << 8 | array[offset + 6];
            return value << 8 | array[offset + 7];            
        }

        public static ulong ReadUInt64(this byte[] array, int offset = 0)
        {
            ulong value = (ulong)(array[offset] << 8 | array[offset + 1]);
            value = value << 8 | array[offset + 2];
            value = value << 8 | array[offset + 3];
            value = value << 8 | array[offset + 4];
            value = value << 8 | array[offset + 5];
            value = value << 8 | array[offset + 6];
            return value << 8 | array[offset + 7];
        }

        public static string ReadString(this byte[] array, int length, int offset = 0)
        {
            var encoding = Encoding.UTF8;
            return encoding.GetString(array, offset, length);
        }
    }
}
