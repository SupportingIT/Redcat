using Redcat.Core;
using System;
using System.Collections.Generic;

namespace Redcat.Amqp.Serialization
{
    public class AmqpDataReader2 : IDataReader
    {
        private static Dictionary<byte, Func<ByteBuffer, object>> valueReaders;
        private static Dictionary<byte, object> typedValueReaders;

        static AmqpDataReader2()
        {
            valueReaders = new Dictionary<byte, Func<ByteBuffer, object>>();
            typedValueReaders = new Dictionary<byte, object>();

            AddValueReader(DataTypeCodes.UByte, b => b.ReadByte());
            AddValueReader(DataTypeCodes.Byte, b => b.ReadSByte());
            AddValueReader(DataTypeCodes.UShort, b => b.ReadUInt16());
            AddValueReader(DataTypeCodes.Short, b => b.ReadInt16());
            AddValueReader(DataTypeCodes.UInt, b => b.ReadUInt32());
            AddValueReader(DataTypeCodes.SmallUInt, b => (uint)b.ReadByte());
            AddValueReader(DataTypeCodes.UInt0, b => 0U);
            AddValueReader(DataTypeCodes.Int, b => b.ReadInt32());
            AddValueReader(DataTypeCodes.SmallInt, b => (int)b.ReadSByte());
            AddValueReader(DataTypeCodes.ULong, b => b.ReadUInt64());
            AddValueReader(DataTypeCodes.SmallULong, b => (ulong)b.ReadByte());
            AddValueReader(DataTypeCodes.ULong0, b => 0UL);
            AddValueReader(DataTypeCodes.Long, b => b.ReadInt64());
            AddValueReader(DataTypeCodes.SmallLong, b => (long)b.ReadSByte());
            AddValueReader(DataTypeCodes.Sym8, ReadString8);
            AddValueReader(DataTypeCodes.Str8, ReadString8);
            AddValueReader(DataTypeCodes.Sym32, ReadString32);
            AddValueReader(DataTypeCodes.Str32, ReadString32);
        }

        public virtual bool CanRead()
        {
            throw new NotImplementedException();
        }

        private static string ReadString8(ByteBuffer buffer)
        {
            byte length = buffer.ReadByte();
            return buffer.ReadString(length);
        }

        private static string ReadString32(ByteBuffer buffer)
        {
            int length = (int)buffer.ReadUInt32();
            return buffer.ReadString(length);
        }

        private static void AddValueReader<T>(byte code, Func<ByteBuffer, T> valueReader)
        {
            valueReaders[code] = b => valueReader.Invoke(b);
            typedValueReaders[code] = valueReader;
        }

        private ByteBuffer buffer;

        public AmqpDataReader2(ByteBuffer buffer)
        {
            this.buffer = buffer;
        }

        protected ByteBuffer Buffer => buffer;

        public virtual object Read()
        {
            EnsureTypeCodeSupported();
            byte code = buffer.ReadByte();
            var reader = valueReaders[code];
            return reader.Invoke(buffer);
        }

        public virtual T Read<T>()
        {
            EnsureTypeCodeSupported();            
            var reader = typedValueReaders[buffer[0]] as Func<ByteBuffer, T>;
            if (reader == null) throw new InvalidOperationException($"Can't read type with code 0x{buffer[0].ToString("X2")} as {typeof(T)}");
            byte code = buffer.ReadByte();
            return reader.Invoke(buffer);
        }

        private void EnsureTypeCodeSupported()
        {
            if (!valueReaders.ContainsKey(buffer[0]))
            {
                throw new NotSupportedException($"Unknown type code 0x{buffer[0].ToString("X2")}");
            }
        }
    }
}
