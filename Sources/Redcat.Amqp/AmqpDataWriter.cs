using System;
using System.IO;

namespace Redcat.Amqp
{
    public class AmqpDataWriter
    {        
        private Stream stream;

        public AmqpDataWriter(Stream stream)
        {
            this.stream = stream;
        }

        public void WriteFalse()
        {
            stream.WriteByte(DataTypeCodes.FalseValue);
        }

        public void WriteTrue()
        {
            stream.WriteByte(DataTypeCodes.TrueValue);
        }

        public void WriteNull()
        {
            stream.WriteByte(DataTypeCodes.Null);
        }

        public void Write(bool value)
        {
            stream.WriteByte(DataTypeCodes.Boolean);
            stream.WriteByte(Convert.ToByte(value));
        }

        public void Write(byte value)
        {
            stream.WriteByte(DataTypeCodes.UByte);
            stream.WriteByte(value);
        }

        public void Write(sbyte value)
        {            
            stream.WriteByte(DataTypeCodes.Byte);
            stream.WriteByte((byte)value);
        }

        public void Write(short value)
        {
            stream.WriteByte(DataTypeCodes.Short);
            stream.WriteByte((byte)((value & 0xff00) >> 8));
            stream.WriteByte((byte)((value & 0x00ff)));
        }

        public void Write(ushort value)
        {
            stream.WriteByte(DataTypeCodes.UShort);
            stream.WriteByte((byte)((value & 0xff00) >> 8));
            stream.WriteByte((byte)((value & 0x00ff)));
        }

        public void Write(int value)
        {
            stream.WriteByte(DataTypeCodes.Int);
            stream.WriteByte((byte)((value & 0xff000000) >> 24));
            stream.WriteByte((byte)((value & 0x00ff0000) >> 16));
            stream.WriteByte((byte)((value & 0x0000ff00) >> 8));
            stream.WriteByte((byte)((value & 0x000000ff)));
        }

        public void Write(uint value)
        {
            stream.WriteByte(DataTypeCodes.UInt);
            stream.WriteByte((byte)((value & 0xff000000) >> 24));
            stream.WriteByte((byte)((value & 0x00ff0000) >> 16));
            stream.WriteByte((byte)((value & 0x0000ff00) >> 8));
            stream.WriteByte((byte)((value & 0x000000ff)));
        }

        public void Write(long value)
        {
            stream.WriteByte(DataTypeCodes.Long);

            unchecked
            {
                stream.WriteByte((byte)((value & (long)0xff00000000000000) >> 56));
            }
            stream.WriteByte((byte)((value & 0x00ff000000000000) >> 48));
            stream.WriteByte((byte)((value & 0x0000ff0000000000) >> 40));
            stream.WriteByte((byte)((value & 0x000000ff00000000) >> 32));

            stream.WriteByte((byte)((value & 0x00000000ff000000) >> 24));
            stream.WriteByte((byte)((value & 0x0000000000ff0000) >> 16));
            stream.WriteByte((byte)((value & 0x000000000000ff00) >> 8));
            stream.WriteByte((byte)((value & 0x00000000000000ff)));
        }

        public void Write(ulong value)
        {
            stream.WriteByte(DataTypeCodes.ULong);

            stream.WriteByte((byte)((value & 0xff00000000000000) >> 56));
            stream.WriteByte((byte)((value & 0x00ff000000000000) >> 48));
            stream.WriteByte((byte)((value & 0x0000ff0000000000) >> 40));
            stream.WriteByte((byte)((value & 0x000000ff00000000) >> 32));

            stream.WriteByte((byte)((value & 0x00000000ff000000) >> 24));
            stream.WriteByte((byte)((value & 0x0000000000ff0000) >> 16));
            stream.WriteByte((byte)((value & 0x000000000000ff00) >> 8));
            stream.WriteByte((byte)((value & 0x00000000000000ff)));
        }

        public void Write(float value)
        {
            throw new NotImplementedException();
        }

        public void Write(double value)
        {
            throw new NotImplementedException();
        }

        public void Write(decimal value)
        {
            throw new NotImplementedException();
        }

        public void Write(char ch)
        {
            throw new NotImplementedException();
        }

        public void Write(DateTime value)
        {
            throw new NotImplementedException();
        }

        public void Write(Guid value)
        {
            throw new NotImplementedException();
        }
    }
}
