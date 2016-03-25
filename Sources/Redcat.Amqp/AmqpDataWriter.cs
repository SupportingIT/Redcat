using System;
using System.IO;
using System.Text;
using Redcat.Amqp.Serializers;

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
            stream.Write(value);
        }

        public void Write(ushort value)
        {
            stream.WriteByte(DataTypeCodes.UShort);
            stream.Write(value);
        }

        public void Write(int value)
        {
            stream.WriteByte(DataTypeCodes.Int);
            stream.Write(value);
        }

        public void Write(uint value)
        {
            stream.WriteByte(DataTypeCodes.UInt);
            stream.Write(value);
        }

        public void Write(long value)
        {
            stream.WriteByte(DataTypeCodes.Long);
            stream.Write(value);
        }

        public void Write(ulong value)
        {
            stream.WriteByte(DataTypeCodes.ULong);
            stream.Write(value);
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

        public void Write(string value)
        {
            WriteString(value, DataTypeCodes.Str8, DataTypeCodes.Str32, Encoding.UTF8);
        }

        public void WriteSymbolic(string value)
        {
            WriteString(value, DataTypeCodes.Sym8, DataTypeCodes.Sym32, Encoding.UTF8);
        }

        public void Write(DateTime value)
        {
            throw new NotImplementedException();
        }

        public void Write(Guid value)
        {
            throw new NotImplementedException();
        }

        private void WriteString(string value, byte code8, byte code32, Encoding encoding)
        {
            int length = value.Length;

            if (length <= byte.MaxValue)
            {
                stream.WriteByte(code8);
                stream.WriteByte((byte)value.Length);
            }
            else
            {
                stream.WriteByte(code32);
                stream.Write(length);
            }

            var strBytes = encoding.GetBytes(value);
            stream.Write(strBytes, 0, strBytes.Length);
        }
    }
}
