﻿using Redcat.Core;
using System;

namespace Redcat.Amqp.Serialization
{
    public class AmqpDataReader
    {
        private ByteBuffer buffer;

        public AmqpDataReader(ByteBuffer buffer)
        {
            this.buffer = buffer;
        }

        public string ReadDescriptor()
        {
            throw new NotImplementedException();
        }

        public bool IsBoolean()
        {
            byte code = buffer[0];
            return code == DataTypeCodes.Boolean || code == DataTypeCodes.TrueValue || code == DataTypeCodes.FalseValue;
        }

        public bool ReadBoolean()
        {
            byte code = buffer.ReadByte();
            if (code == DataTypeCodes.FalseValue) return false;
            if (code == DataTypeCodes.TrueValue) return true;
            if (code == DataTypeCodes.Boolean)
            {
                byte value = buffer.ReadByte();
                if (value == 0x00) return false;
                if (value == 0x01) return true;
            }
            return true;
        }

        public bool IsUByte()
        {
            return buffer[0] == DataTypeCodes.UByte;
        }

        public byte ReadUByte()
        {
            byte code = buffer.ReadByte();
            return buffer.ReadByte();
        }

        public bool IsByte()
        {
            return buffer[0] == DataTypeCodes.Byte;
        }

        public sbyte ReadByte()
        {
            byte code = buffer.ReadByte();
            return buffer.ReadSByte();
        }

        public bool IsShort()
        {
            return buffer[0] == DataTypeCodes.Short;
        }

        public short ReadShort()
        {
            byte code = buffer.ReadByte();
            return buffer.ReadInt16();
        }

        public bool IsUShort()
        {
            return buffer[0] == DataTypeCodes.UShort;
        }

        public ushort ReadUShort()
        {
            byte code = buffer.ReadByte();
            return buffer.ReadUInt16();
        }

        public bool IsInt()
        {
            return buffer[0] == DataTypeCodes.Int || buffer[0] == DataTypeCodes.SmallInt;
        }

        public int ReadInt()
        {
            byte code = buffer.ReadByte();
            if (code == DataTypeCodes.Int) return buffer.ReadInt32();
            if (code == DataTypeCodes.SmallInt) return buffer.ReadByte();
            return 0;
        }

        public bool IsUInt()
        {
            return buffer[0] == DataTypeCodes.UInt0 || buffer[0] == DataTypeCodes.SmallUInt || buffer[0] == DataTypeCodes.UInt;
        }

        public uint ReadUInt()
        {
            byte code = buffer.ReadByte();
            if (code == DataTypeCodes.UInt) return buffer.ReadUInt32();
            if (code == DataTypeCodes.SmallUInt) return buffer.ReadByte();
            if (code == DataTypeCodes.UInt0) return 0;
            return 0;
        }

        public bool IsLong()
        {
            return buffer[0] == DataTypeCodes.Long || buffer[0] == DataTypeCodes.SmallLong;
        }

        public long ReadLong()
        {
            byte code = buffer.ReadByte();
            if (code == DataTypeCodes.Long) return buffer.ReadInt64();
            if (code == DataTypeCodes.SmallLong) return buffer.ReadSByte();
            return 0;
        }

        public bool IsULong()
        {
            return buffer[0] == DataTypeCodes.ULong || buffer[0] == DataTypeCodes.SmallULong || buffer[0] == DataTypeCodes.ULong0;
        }

        public ulong ReadULong()
        {
            byte code = buffer.ReadByte();
            if (code == DataTypeCodes.ULong) return buffer.ReadUInt64();
            if (code == DataTypeCodes.SmallULong) return buffer.ReadByte();
            return 0;
        }
    }
}
