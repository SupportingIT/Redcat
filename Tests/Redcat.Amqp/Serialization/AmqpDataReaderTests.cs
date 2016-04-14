using NUnit.Framework;
using Redcat.Amqp.Serialization;
using Redcat.Core;
using Redcat.Test;
using System;
using System.Collections.Generic;
using System.Text;

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

        #region Descriptor

        [Test]
        public void IsDescriptor_Returns_True_For_Descriptor_Value_Code()
        {
            buffer.Write(new byte[] { DataTypeCodes.Descriptor, 10, 20, 30 });

            Assert.That(reader.IsDescriptor(), Is.True);
        }

        [Test]
        public void IsULongDescriptor_Returns_True_For_ULong_Descriptors()
        {
            buffer.Write(new byte[] { DataTypeCodes.Descriptor, DataTypeCodes.ULong, 0, 0, 1 });

            Assert.That(reader.IsULongDescriptor(), Is.True);
        }

        public IEnumerable<Tuple<ulong, byte[]>> GetReadULongDescriptorTestData()
        {
            yield return GetTestData(10UL, DataTypeCodes.Descriptor, DataTypeCodes.ULong, 0, 0, 0, 0, 0, 0, 0, 10);
            yield return GetTestData(0x10UL, DataTypeCodes.Descriptor, DataTypeCodes.SmallULong, 0x10);
        }

        [Test]
        public void ReadULongDescriptor_Returns_Correct_Value([ValueSource(nameof(GetReadULongDescriptorTestData))]Tuple<ulong, byte[]> data)
        {
            VerifyReadValueMethod(data.Item1, data.Item2, r => r.ReadULongDescriptor());
        }

        #endregion

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
        public void ReadBoolean_Returns_CorrectValue([ValueSource(nameof(GetReadBooleanData))]Tuple<bool, byte[]> data)
        {
            VerifyReadValueMethod(data.Item1, data.Item2, r => r.ReadBoolean());
        }

        public IEnumerable<Tuple<bool, byte[]>> GetReadBooleanData()
        {
            yield return GetTestData(false, DataTypeCodes.FalseValue);
            yield return GetTestData(true, DataTypeCodes.TrueValue);
            yield return GetTestData(false, DataTypeCodes.Boolean, 0x00);
            yield return GetTestData(true, DataTypeCodes.Boolean, 0x01);
        }

        #endregion

        #region Byte types

        [Test]
        public void IsUByte_Returns_True_For_UByte_Code()
        {
            buffer.Write(new byte[] { DataTypeCodes.UByte, 0xfa });

            Assert.That(reader.IsUByte(), Is.True);
        }

        byte[] ReadUByteTestData = { 0, byte.MaxValue };

        [Test]
        public void ReadUByte_Returns_Correct_Value([ValueSource(nameof(ReadUByteTestData))]byte value)
        {
            VerifyReadValueMethod(value, value.ToString("x2"), DataTypeCodes.UByte, r => r.ReadUByte());
        }

        [Test]
        public void IsByte_Returns_True_For_Byte_Code()
        {
            buffer.Write(new byte[] { DataTypeCodes.Byte, 0xdc });

            Assert.That(reader.IsByte(), Is.True);
        }

        sbyte[] ReadByteTestData = { sbyte.MinValue, 0, sbyte.MaxValue };

        [Test]
        public void ReadByte_Returs_CorrectValue([ValueSource(nameof(ReadByteTestData))]sbyte value)
        {
            VerifyReadValueMethod(value, value.ToString("x2"), DataTypeCodes.Byte, r => r.ReadByte());
        }

        #endregion

        #region Int16 types

        [Test]
        public void IsShort_Returns_True_For_Short_Type_Code()
        {
            buffer.Write(new byte[] { DataTypeCodes.Short, 0xde, 0xaa });

            Assert.That(reader.IsShort(), Is.True);
        }

        short[] ReadShortTestData = { short.MinValue, 0, short.MaxValue };

        [Test]
        public void ReadShort_Returns_Correct_Value([ValueSource(nameof(ReadShortTestData))]short value)
        {            
            VerifyReadValueMethod(value, value.ToString("x4"), DataTypeCodes.Short, r => r.ReadShort());
        }        

        [Test]
        public void IsUShort_Returns_True_For_Unsigned_Short_Type_Codes()
        {
            buffer.Write(new byte[] { DataTypeCodes.UShort, 0x59, 0xff });

            Assert.That(reader.IsUShort(), Is.True);
        }

        ushort[] ReadUShortTestData = { 0, ushort.MaxValue };

        [Test]
        public void ReadUShort_Returns_Correct_Value([ValueSource(nameof(ReadUShortTestData))]ushort value)
        {            
            VerifyReadValueMethod(value, value.ToString("x4"), DataTypeCodes.UShort, r => r.ReadUShort());
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

        public IEnumerable<Tuple<int, byte[]>> GetReadIntData()
        {
            yield return GetTestData(-1, (-1).ToString("x8"), DataTypeCodes.Int);
            yield return GetTestData(8, 8.ToString("x2"), DataTypeCodes.SmallInt);
        }    

        [Test]
        public void ReadInt_Returns_Correct_Value([ValueSource(nameof(GetReadIntData))]Tuple<int, byte[]> data)
        {
            VerifyReadValueMethod(data.Item1, data.Item2, r => r.ReadInt());
        }

        public byte[] UIntTypeCodes = { DataTypeCodes.UInt0, DataTypeCodes.SmallUInt, DataTypeCodes.UInt };

        [Test]
        public void IsUInt_Returns_True_For_UInt_Type_Codes([ValueSource(nameof(UIntTypeCodes))]byte code)
        {
            buffer.Write(new byte[] { code, 0x01, 0x23, 0x45, 0x67 });

            Assert.That(reader.IsUInt(), Is.True);
        }

        public IEnumerable<Tuple<uint, byte[]>> GetReadUIntTestData()
        {
            yield return GetTestData(uint.MaxValue, uint.MaxValue.ToString("x8"), DataTypeCodes.UInt);
            yield return GetTestData((uint)0xfa, ((uint)0xfa).ToString("x2"), DataTypeCodes.SmallUInt);
            yield return GetTestData((uint)0, DataTypeCodes.UInt0);
        }

        [Test]
        public void ReadUInt_Returns_Correct_Value([ValueSource(nameof(GetReadUIntTestData))]Tuple<uint, byte[]> data)
        {
            VerifyReadValueMethod(data.Item1, data.Item2, r => r.ReadUInt());
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

        public IEnumerable<Tuple<long, byte[]>> GetReadLongTestData()
        {
            yield return GetTestData(long.MaxValue, long.MaxValue.ToString("x16"), DataTypeCodes.Long);
            yield return GetTestData(-1L, "ff", DataTypeCodes.SmallLong);            
        }

        [Test]
        public void ReadLong_Returns_Correct_Value([ValueSource(nameof(GetReadLongTestData))]Tuple<long, byte[]> data)
        {
            VerifyReadValueMethod(data.Item1, data.Item2, r => r.ReadLong());
        }

        public byte[] ULongTypeCodes = { DataTypeCodes.ULong, DataTypeCodes.SmallULong, DataTypeCodes.ULong0 };

        [Test]
        public void IsULong_Returns_True_For_ULong_Type_Codes([ValueSource(nameof(ULongTypeCodes))]byte code)
        {
            buffer.Write(new byte[] { code, 0, 0, 0, 0, 0 });

            Assert.That(reader.IsULong(), Is.True);
        }

        public IEnumerable<Tuple<ulong, byte[]>> GetReadULongTestData()
        {
            yield return GetTestData(ulong.MaxValue, ulong.MaxValue.ToString("x16"), DataTypeCodes.ULong);
            yield return GetTestData(0x0fUL, 0x0fUL.ToString("x2"), DataTypeCodes.SmallULong);
            yield return GetTestData(0UL, DataTypeCodes.ULong0);
        }

        [Test]
        public void ReadULong_Returns_Correct_Value([ValueSource(nameof(GetReadULongTestData))]Tuple<ulong, byte[]> data)
        {
            VerifyReadValueMethod(data.Item1, data.Item2, r => r.ReadULong());
        }

        #endregion

        #region String

        byte[] StringTypeCodes = { DataTypeCodes.Str8, DataTypeCodes.Str32, DataTypeCodes.Sym8, DataTypeCodes.Sym32 };

        [Test]
        public void IsString_Returns_True_For_String_Type_Codes([ValueSource(nameof(StringTypeCodes))]byte code)
        {
            buffer.Write(new byte[] { code, 0, 1, 2 });

            Assert.That(reader.IsString(), Is.True);
        }

        IEnumerable<Tuple<string, byte[]>> GetReadStringTestData()
        {
            yield return GetStringTestData("Hello", DataTypeCodes.Str8);
            yield return GetStringTestData("What's up", DataTypeCodes.Sym8);
            yield return GetStringTestData("Another hello", DataTypeCodes.Str32);
            yield return GetStringTestData("Hello Night", DataTypeCodes.Sym32);
        }

        [Test]
        public void ReadString_Returns_Correct_String_Value([ValueSource(nameof(GetReadStringTestData))]Tuple <string, byte[]> data)
        {
            VerifyReadValueMethod(data.Item1, data.Item2, r => r.ReadString());
        }

        #endregion

        private Tuple<string, byte[]> GetStringTestData(string str, byte code)
        {
            List<byte> serialized = new List<byte>();
            serialized.Add(code);

            if (code == DataTypeCodes.Str8 || code == DataTypeCodes.Sym8) serialized.Add((byte)str.Length);
            if (code == DataTypeCodes.Str32 || code == DataTypeCodes.Sym32)
            {
                serialized.AddRange(BinaryUtils.ToByteArray(str.Length.ToString("x8")));
            }

            serialized.AddRange(Encoding.UTF8.GetBytes(str));
            return new Tuple<string, byte[]>(str, serialized.ToArray());
        }

        private Tuple<T, byte[]> GetTestData<T>(T expected, params byte[] serialized)
        {
            return new Tuple<T, byte[]>(expected, serialized);
        }

        private Tuple<T, byte[]> GetTestData<T>(T expected, string hexString, byte code)
        {
            List<byte> serialized = new List<byte>();
            serialized.Add(code);
            serialized.AddRange(BinaryUtils.ToByteArray(hexString));
            return new Tuple<T, byte[]>(expected, serialized.ToArray());
        }

        private void VerifyReadValueMethod<T>(T expectedValue, string hexString, byte code, Func<AmqpDataReader, T> readValue)
        {
            List<byte> bytes = new List<byte>();
            bytes.Add(code);
            bytes.AddRange(BinaryUtils.ToByteArray(hexString));
            VerifyReadValueMethod(expectedValue, bytes.ToArray(), readValue);
        }

        private void VerifyReadValueMethod<T>(T expectedValue, byte[] serializedData, Func<AmqpDataReader, T> readValue)
        {
            buffer.Write(serializedData);

            T actualValue = readValue(reader);

            Assert.That(actualValue, Is.EqualTo(expectedValue));
        }
    }
}
