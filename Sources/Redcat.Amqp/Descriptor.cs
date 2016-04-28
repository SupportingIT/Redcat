using System.Text;

namespace Redcat.Amqp
{
    public class Descriptor
    {
        private ulong? uValue;
        private string sValue;

        public Descriptor(ulong value)
        {
            uValue = value;
        }

        public Descriptor(string value)
        {
            sValue = value;
        }

        public bool IsULong => uValue.HasValue;

        public ulong ULongValue => uValue.Value;

        public bool IsString => sValue != null;

        public string StringValue => sValue;

        public override string ToString()
        {
            if (uValue.HasValue)
            {
                StringBuilder sb = new StringBuilder((uValue.Value >> 32).ToString("x8"));
                sb.Append(':');
                sb.Append((uValue.Value & 0x00000000ffffffff).ToString("x8"));
                return sb.ToString();
            }
            return sValue;
        }

        public static implicit operator Descriptor(string str) => new Descriptor(str);
        public static implicit operator Descriptor(ulong val) => new Descriptor(val);
    }
}
