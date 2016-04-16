using System.Collections.Generic;
using System.Globalization;

namespace Redcat.Test
{
    public static class BinaryUtils
    {
        public static byte[] ToByteArray(string hexString)
        {
            List<byte> bytes = new List<byte>();

            for (int i = 0; i < hexString.Length; i += 2)
            {
                byte b = byte.Parse(hexString.Substring(i, 2), NumberStyles.HexNumber);
                bytes.Add(b);
            }

            return bytes.ToArray();
        }

        public static void Add(this ICollection<byte> bytes, short value)
        {
            bytes.Add(value.ToString("x4"));
        }

        public static void Add(this ICollection<byte> bytes, int value)
        {
            bytes.Add(value.ToString("x8"));
        }

        public static void Add(this ICollection<byte> bytes, string hexString)
        {
            bytes.AddRange(ToByteArray(hexString));
        }

        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> values)
        {
            foreach (T value in values) collection.Add(value);
        }

        public static void AddRange<T>(this ICollection<T> collection, params T[] values)
        {
            collection.AddRange((IEnumerable<T>)values);
        }
    }
}
