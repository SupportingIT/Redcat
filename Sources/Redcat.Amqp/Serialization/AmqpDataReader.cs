using Redcat.Core;
using System;
using System.Collections.Generic;

namespace Redcat.Amqp.Serialization
{
    public class AmqpDataReader : IByteBufferReader
    {
        private static Dictionary<byte, Func<ByteBuffer, object>> valueReaders;
        private static Dictionary<byte, object> typedValueReaders;

        static AmqpDataReader()
        {
            valueReaders = new Dictionary<byte, Func<ByteBuffer, object>>();
            typedValueReaders = new Dictionary<byte, object>();

            AddValueReader(DataTypeCodes.Descriptor, b => ReadDescriptor(b));
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

        private static Descriptor ReadDescriptor(ByteBuffer buffer)
        {
            byte code = buffer.ReadByte();
            if (code == DataTypeCodes.ULong0) return 0;
            if (code == DataTypeCodes.SmallULong) return buffer.ReadByte();
            if (code == DataTypeCodes.ULong) return buffer.ReadUInt64();
            if (code == DataTypeCodes.Sym8 || code == DataTypeCodes.Str8) return ReadString8(buffer);
            if (code == DataTypeCodes.Sym32 || code == DataTypeCodes.Str32) return ReadString32(buffer);
            throw new NotSupportedException("Unknown descriptor type");
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

        public virtual object Read(ByteBuffer buffer)
        {
            EnsureTypeCodeSupported(buffer[0]);
            byte code = buffer.ReadByte();
            var reader = valueReaders[code];
            return reader.Invoke(buffer);
        }

        public virtual T Read<T>(ByteBuffer buffer)
        {
            EnsureTypeCodeSupported(buffer[0]);            
            var reader = typedValueReaders[buffer[0]] as Func<ByteBuffer, T>;
            if (reader == null) throw new InvalidOperationException($"Can't read type with code 0x{buffer[0].ToString("X2")} as {typeof(T)}");
            byte code = buffer.ReadByte();
            return reader.Invoke(buffer);
        }

        private void EnsureTypeCodeSupported(byte code)
        {
            if (!valueReaders.ContainsKey(code))
            {
                throw new NotSupportedException($"Unknown type code 0x{code.ToString("X2")}");
            }
        }
    }
}
