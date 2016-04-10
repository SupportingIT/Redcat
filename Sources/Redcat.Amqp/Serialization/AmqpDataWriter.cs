using Redcat.Core.Serialization;
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace Redcat.Amqp.Serialization
{
    public class AmqpDataWriter
    {
        private Dictionary<Type, Action<object>> writerMap;
        private MemoryStream buffer;        
        private Stream stream;

        public AmqpDataWriter(Stream stream)
        {
            buffer = new MemoryStream();
            this.stream = stream;
            Stream = this.stream;
            writerMap = CreateWriterMap();
        }

        private Dictionary<Type, Action<object>> CreateWriterMap()
        {
            return new Dictionary<Type, Action<object>>
            {
                [typeof(byte)] = v => Write((byte)v),
                [typeof(int)] = v => Write((int)v),
                [typeof(long)] = v => Write((long)v),
                [typeof(string)] = v => Write((string)v)
            };
        }

        private Stream Stream { get; set; }

        public void WriteFalse()
        {
            Stream.WriteByte(DataTypeCodes.FalseValue);
        }

        public void WriteTrue()
        {
            Stream.WriteByte(DataTypeCodes.TrueValue);
        }

        public void WriteNull()
        {
            Stream.WriteByte(DataTypeCodes.Null);
        }

        public void Write(bool value)
        {
            Stream.WriteByte(DataTypeCodes.Boolean);
            Stream.WriteByte(Convert.ToByte(value));
        }

        public void Write(byte value)
        {
            Stream.WriteByte(DataTypeCodes.UByte);
            Stream.WriteByte(value);
        }

        public void Write(sbyte value)
        {            
            Stream.WriteByte(DataTypeCodes.Byte);
            Stream.WriteByte((byte)value);
        }

        public void Write(short value)
        {
            Stream.WriteByte(DataTypeCodes.Short);
            Stream.Write(value);
        }

        public void Write(ushort value)
        {
            Stream.WriteByte(DataTypeCodes.UShort);
            Stream.Write(value);
        }

        public void Write(int value)
        {
            Stream.WriteByte(DataTypeCodes.Int);
            Stream.Write(value);
        }

        public void Write(uint value)
        {
            Stream.WriteByte(DataTypeCodes.UInt);
            Stream.Write(value);
        }

        public void Write(long value)
        {
            Stream.WriteByte(DataTypeCodes.Long);
            Stream.Write(value);
        }

        public void Write(ulong value)
        {
            Stream.WriteByte(DataTypeCodes.ULong);
            Stream.Write(value);
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
                Stream.WriteByte(code8);
                Stream.WriteByte((byte)value.Length);
            }
            else
            {
                Stream.WriteByte(code32);
                Stream.Write(length);
            }

            var strBytes = encoding.GetBytes(value);
            Stream.Write(strBytes, 0, strBytes.Length);
        }

        public void WriteDescriptor(string descriptor)
        {
            Stream.WriteByte(DataTypeCodes.Descriptor);
            Write(descriptor);
        }

        public void WriteArray<T>(IEnumerable<T> array)
        {
            throw new NotImplementedException();
        }

        public void WriteList(IEnumerable<object> list)
        {
            int count = list.Count();
            Stream = buffer;
            foreach(object item in list)
            {
                if (item == null) WriteNull();
                if (!writerMap.ContainsKey(item.GetType())) throw new InvalidOperationException();
                var writer = writerMap[item.GetType()];
                writer.Invoke(item);
            }
            Stream = stream;
            stream.WriteByte(DataTypeCodes.List32);
            stream.Write((int)buffer.Length);
            buffer.WriteTo(stream);
        }

        public void WriteRaw(byte[] rawData)
        {
            stream.Write(rawData, 0, rawData.Length);
        }
    }
}
