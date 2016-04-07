namespace Redcat.Amqp
{
    public static class DataTypeCodes
    {
        public const byte Descriptor = 0x00;

        public const byte Null = 0x40;
        public const byte TrueValue = 0x41;
        public const byte FalseValue = 0x42;
        public const byte UInt0 = 0x43;
        public const byte ULong0 = 0x44;
        public const byte List0 = 0x45;

        public const byte UByte = 0x50;
        public const byte Byte = 0x51;
        public const byte SmallUInt = 0x52;
        public const byte SmallULong = 0x53;
        public const byte SmallInt = 0x54;
        public const byte SmallLong = 0x55;
        public const byte Boolean = 0x56;

        public const byte UShort = 0x60;
        public const byte Short = 0x61;

        public const byte UInt = 0x70;
        public const byte Int = 0x71;
        public const byte Float = 0x72;
        public const byte Char = 0x73;//UTF32 character
        public const byte Decimal32 = 0x74;

        public const byte ULong = 0x80;
        public const byte Long = 0x81;
        public const byte Double = 0x82;
        public const byte Timestamp = 0x83;
        public const byte Decimal64 = 0x84;

        public const byte Decimal128 = 0x94;
        public const byte Uuid = 0x98;

        public const byte VBin8 = 0xA0;
        public const byte Str8 = 0xA1;
        public const byte Sym8 = 0xA3;

        public const byte VBin32 = 0xB0;
        public const byte Str32 = 0xB1;
        public const byte Sym32 = 0xB3;

        public const byte List8 = 0xC0;
        public const byte Map8 = 0xC1;

        public const byte List32 = 0xD0;
        public const byte Map32 = 0xD1;

        public const byte Array8 = 0xE0;

        public const byte Array32 = 0xF0;
    }
}
