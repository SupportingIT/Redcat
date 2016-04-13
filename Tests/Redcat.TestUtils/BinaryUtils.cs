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
    }
}
