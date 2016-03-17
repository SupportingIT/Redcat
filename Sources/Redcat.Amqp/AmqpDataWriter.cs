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
            throw new NotImplementedException();
        }

        public void Write(short value)
        {
            throw new NotImplementedException();
        }

        public void Write(ushort value)
        {
            stream.WriteByte(DataTypeCodes.UShort);
            stream.WriteByte((byte)((value & 0xff00) >> 8));
            stream.WriteByte((byte)((value & 0x00ff)));
        }

        public void Write(int value)
        {
            throw new NotImplementedException();
        }

        public void Write(uint value)
        {
            throw new NotImplementedException();
        }

        public void Write(long value)
        {
            throw new NotImplementedException();
        }

        public void Write(ulong value)
        {
            throw new NotImplementedException();
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
