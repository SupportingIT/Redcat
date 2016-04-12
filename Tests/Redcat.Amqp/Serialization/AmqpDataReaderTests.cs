using NUnit.Framework;
using Redcat.Amqp.Serialization;
using Redcat.Core;
using System;
using System.Collections.Generic;

namespace Redcat.Amqp.Tests.Serialization
{
    [TestFixture]
    public class AmqpDataReaderTests
    {
        private ByteBuffer buffer;
        private AmqpDataReader reader;

        [SetUp]
        public void SetUp()
        {
            buffer = new ByteBuffer(100);
            reader = new AmqpDataReader(buffer);
        }

        #region Boolean

        [Test]
        public void IsBoolean_Returns_True_For_Boolean_Codes([ValueSource(nameof(GetBooleanCodes))]byte code)
        {
            buffer.Write(new byte[] { code });

            Assert.That(reader.IsBoolean(), Is.True);
        }

        public IEnumerable<byte> GetBooleanCodes()
        {
            yield return DataTypeCodes.TrueValue;
            yield return DataTypeCodes.FalseValue;
            yield return DataTypeCodes.Boolean;
        }

        [Test]
        public void ReadBoolean_Returns_CorrectValue([ValueSource("GetReadBooleanData")]Tuple<byte[], bool> data)
        {
            byte[] encodedData = data.Item1;
            bool expectedValue = data.Item2;
            buffer.Write(encodedData);

            bool actualValue = reader.ReadBoolean();

            Assert.That(actualValue, Is.EqualTo(expectedValue));
        }

        public IEnumerable<Tuple<byte[], bool>> GetReadBooleanData()
        {
            yield return new Tuple<byte[], bool>(new byte[] { DataTypeCodes.FalseValue }, false);
            yield return new Tuple<byte[], bool>(new byte[] { DataTypeCodes.TrueValue }, true);
            yield return new Tuple<byte[], bool>(new byte[] { DataTypeCodes.Boolean, 0x00 }, false);
            yield return new Tuple<byte[], bool>(new byte[] { DataTypeCodes.Boolean, 0x01 }, true);
        }

        #endregion

        #region Byte types

        [Test]
        public void IsUByte_Returns_True_For_UByte_Code()
        {
            buffer.Write(new byte[] { DataTypeCodes.UByte, 0xfa });

            Assert.That(reader.IsUByte(), Is.True);
        }

        [Test]
        public void ReadUByte_Returns_Correct_Value()
        {
            byte value = 0xfd;
            buffer.Write(new byte[] { DataTypeCodes.UByte, value });

            byte actualValue = reader.ReadUByte();

            Assert.That(actualValue, Is.EqualTo(value));
        }

        [Test]
        public void IsByte_Returns_True_For_Byte_Code()
        {
            buffer.Write(new byte[] { DataTypeCodes.Byte, 0xdc });

            Assert.That(reader.IsByte(), Is.True);
        }

        [Test]
        public void ReadByte_Returs_CorrectValue()
        {
            sbyte value = -2;
            buffer.Write(new byte[] { DataTypeCodes.Byte, (byte)value });

            sbyte actualValue = reader.ReadByte();

            Assert.That(actualValue, Is.EqualTo(value));
        }

        #endregion

        #region Int16 types

        [Test]
        public void IsShort_Returns_True_For_Short_Type_Code()
        {
            buffer.Write(new byte[] { DataTypeCodes.Short, 0xde, 0xaa });

            Assert.That(reader.IsShort(), Is.True);
        }

        [Test]
        public void ReadShort_Returns_Correct_Value()
        {
            buffer.Write(new byte[] { DataTypeCodes.Short, 0xfe, 0x1a });
            unchecked
            {
                short expectedValue = (short)0xfe1a;
                short actualValue = reader.ReadShort();

                Assert.That(actualValue, Is.EqualTo(expectedValue));
            }
        }

        [Test]
        public void IsUShort_Returns_True_For_Unsigned_Short_Type_Codes()
        {
            buffer.Write(new byte[] { DataTypeCodes.UShort, 0x59, 0xff });

            Assert.That(reader.IsUShort(), Is.True);
        }

        [Test]
        public void ReadUShort_Returns_Correct_Value()
        {
            buffer.Write(new byte[] { DataTypeCodes.UShort, 0xad, 0x14 });
            ushort expectedValue = 0xad14;

            ushort actualValue = reader.ReadUShort();

            Assert.That(actualValue, Is.EqualTo(expectedValue));
        }

        #endregion

        #region Int32 types

        public byte[] IntTypeCodes = { DataTypeCodes.Int, DataTypeCodes.SmallInt };

        [Test]
        public void IsInt_Returns_True_For_Int_Data_Type_Codes([ValueSource(nameof(IntTypeCodes))]byte code)
        {
            buffer.Write(new byte[] { code, 0x01, 0x02, 0x03, 0x04 });

            Assert.That(reader.IsInt(), Is.True);
        }

        public IEnumerable<Tuple<byte[], int>> GetReadIntData()
        {
            yield return new Tuple<byte[],int>(new byte[] { DataTypeCodes.Int, 0xff, 0xff, 0xff, 0xff }, -1);
            yield return new Tuple<byte[],int>(new byte[] { DataTypeCodes.SmallInt, 8 }, 8);
        }    

        [Test]
        public void ReadInt_Returns_Correct_Value([ValueSource(nameof(GetReadIntData))]Tuple<byte[], int> data)
        {
            byte[] binaryData = data.Item1;
            int expectedValue = data.Item2;
            buffer.Write(binaryData);

            int actualValue = reader.ReadInt();

            Assert.That(actualValue, Is.EqualTo(expectedValue));
        }

        public byte[] UIntTypeCodes = { DataTypeCodes.UInt0, DataTypeCodes.SmallUInt, DataTypeCodes.UInt };

        [Test]
        public void IsUInt_Returns_True_For_UInt_Type_Codes([ValueSource(nameof(UIntTypeCodes))]byte code)
        {
            buffer.Write(new byte[] { code, 0x01, 0x23, 0x45, 0x67 });

            Assert.That(reader.IsUInt(), Is.True);
        }

        public IEnumerable<Tuple<byte[], uint>> GetReadUIntTestData()
        {
            yield return new Tuple<byte[], uint>(new byte[] { DataTypeCodes.UInt, 0x0a, 0x0b, 0x0c, 0x0d }, 0x0a0b0c0d);
            yield return new Tuple<byte[], uint>(new byte[] { DataTypeCodes.SmallUInt, 0xdf, 0, 0 }, 0xdf);
            yield return new Tuple<byte[], uint>(new byte[] { DataTypeCodes.UInt0 }, 0);
        }

        [Test]
        public void ReadUInt_Returns_Correct_Value([ValueSource(nameof(GetReadUIntTestData))]Tuple<byte[], uint> data)
        {
            byte[] binaryData = data.Item1;
            uint expectedValue = data.Item2;
            buffer.Write(binaryData);

            uint actualValue = reader.ReadUInt();

            Assert.That(actualValue, Is.EqualTo(expectedValue));
        }

        #endregion

        #region Int64 types

        public byte[] LongTypeCodes = { DataTypeCodes.Long, DataTypeCodes.SmallLong };

        [Test]
        public void IsLong_Returns_True_For_Long_Type_Codes([ValueSource(nameof(LongTypeCodes))]byte code)
        {
            buffer.Write(new byte[] { code, 0, 0, 0 });

            Assert.That(reader.IsLong(), Is.True);
        }

        public IEnumerable<Tuple<byte[], long>> GetReadLongTestData()
        {
            yield return new Tuple<byte[], long>(new byte[] { DataTypeCodes.Long, 0x10, 0x20, 0x30, 0x40, 0x50, 0x60, 0x70, 0x80 }, 0x1020304050607080);
            yield return new Tuple<byte[], long>(new byte[] { DataTypeCodes.SmallLong, 0xff }, -1);
        }

        [Test]
        public void ReadLong_Returns_Correct_Value([ValueSource(nameof(GetReadLongTestData))]Tuple<byte[], long> data)
        {            
            long expectedValue = data.Item2;
            buffer.Write(data.Item1);

            long actualValue = reader.ReadLong();

            Assert.That(actualValue, Is.EqualTo(expectedValue));
        }

        public byte[] ULongTypeCodes = { DataTypeCodes.ULong, DataTypeCodes.SmallULong, DataTypeCodes.ULong0 };

        [Test]
        public void IsULong_Returns_True_For_ULong_Type_Codes([ValueSource(nameof(ULongTypeCodes))]byte code)
        {
            buffer.Write(new byte[] { code, 0, 0, 0, 0, 0 });

            Assert.That(reader.IsULong(), Is.True);
        }

        public IEnumerable<Tuple<byte[], ulong>> GetReadULongTestData()
        {
            yield return new Tuple<byte[], ulong>(new byte[] { DataTypeCodes.ULong, 0, 0, 0, 0, 0, 0, 0x01, 0 }, 0x100);
            yield return new Tuple<byte[], ulong>(new byte[] { DataTypeCodes.SmallULong, 10 }, 10);
            yield return new Tuple<byte[], ulong>(new byte[] { DataTypeCodes.ULong0 }, 0);
        }

        [Test]
        public void ReadULong_Returns_Correct_Value([ValueSource(nameof(GetReadULongTestData))]Tuple<byte[], ulong> data)
        {            
            ulong expectedValue = data.Item2;
            buffer.Write(data.Item1);

            ulong actualValue = reader.ReadULong();

            Assert.That(actualValue, Is.EqualTo(expectedValue));
        }

        #endregion
    }
}
