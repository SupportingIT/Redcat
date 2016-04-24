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
    public class AmqpDataReader2Tests
    {
        private ByteBuffer buffer;
        private AmqpDataReader2 reader;

        [SetUp]
        public void SetUp()
        {
            buffer = new ByteBuffer(100);
            reader = new AmqpDataReader2(buffer);
        }

        byte[] ReadUByteTestData = { 0, byte.MaxValue };
        sbyte[] ReadByteTestData = { sbyte.MinValue, 0, sbyte.MaxValue };
        short[] ReadShortTestData = { short.MinValue, 0, short.MaxValue };
        ushort[] ReadUShortTestData = { 0, ushort.MaxValue };

        IEnumerable<Tuple<int, byte[]>> GetReadIntData()
        {
            yield return GetTestData(-1, (-1).ToString("x8"), DataTypeCodes.Int);
            yield return GetTestData(8, 8.ToString("x2"), DataTypeCodes.SmallInt);
        }

        IEnumerable<Tuple<uint, byte[]>> GetReadUIntTestData()
        {
            yield return GetTestData(uint.MaxValue, uint.MaxValue.ToString("x8"), DataTypeCodes.UInt);
            yield return GetTestData(0xfaU, 0xfaU.ToString("x2"), DataTypeCodes.SmallUInt);
            yield return GetTestData(0U, DataTypeCodes.UInt0);
        }

        IEnumerable<Tuple<long, byte[]>> GetReadLongTestData()
        {
            yield return GetTestData(long.MaxValue, long.MaxValue.ToString("x16"), DataTypeCodes.Long);
            yield return GetTestData(-1L, "ff", DataTypeCodes.SmallLong);
        }

        IEnumerable<Tuple<ulong, byte[]>> GetReadULongTestData()
        {
            yield return GetTestData(ulong.MaxValue, ulong.MaxValue.ToString("x16"), DataTypeCodes.ULong);
            yield return GetTestData(0x0fUL, 0x0fUL.ToString("x2"), DataTypeCodes.SmallULong);
            yield return GetTestData(0UL, DataTypeCodes.ULong0);
        }

        IEnumerable<Tuple<string, byte[]>> GetReadStringTestData()
        {
            yield return GetStringTestData("Hello", DataTypeCodes.Str8);
            yield return GetStringTestData("What's up", DataTypeCodes.Sym8);
            yield return GetStringTestData("Another hello", DataTypeCodes.Str32);
            yield return GetStringTestData("Hello Night", DataTypeCodes.Sym32);
        }

        private Tuple<T, byte[]> GetTestData<T>(T expected, string hexString, byte code)
        {
            List<byte> serialized = new List<byte>();
            serialized.Add(code);
            serialized.AddRange(BinaryUtils.ToByteArray(hexString));
            return new Tuple<T, byte[]>(expected, serialized.ToArray());
        }

        private Tuple<T, byte[]> GetTestData<T>(T expected, params byte[] serialized)
        {
            return new Tuple<T, byte[]>(expected, serialized);
        }

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

        #region Read method tests

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void Read_Throws_Exception_For_Unknown_Type_Code()
        {
            buffer.Write(0xff, 1, 2, 3, 4);
            reader.Read();
        }

        [Test]
        public void Read_Returns_Correct_UByte_Value([ValueSource(nameof(ReadUByteTestData))]byte value)
        {
            VerifyReadMethod(value, value.ToString("x2"), DataTypeCodes.UByte);
        }

        [Test]
        public void Read_Returs_Correct_Byte_Value([ValueSource(nameof(ReadByteTestData))]sbyte value)
        {
            VerifyReadMethod(value, value.ToString("x2"), DataTypeCodes.Byte);
        }

        [Test]
        public void Read_Returns_Correct_Int16_Value([ValueSource(nameof(ReadShortTestData))]short value)
        {
            VerifyReadMethod(value, value.ToString("x4"), DataTypeCodes.Short);
        }

        [Test]
        public void Read_Returns_Correct_UInt16_Value([ValueSource(nameof(ReadUShortTestData))]ushort value)
        {
            VerifyReadMethod(value, value.ToString("x4"), DataTypeCodes.UShort);
        }

        [Test]
        public void Read_Returns_Correct_Int32_Value([ValueSource(nameof(GetReadIntData))]Tuple<int, byte[]> data)
        {
            VerifyReadMethod(data.Item1, data.Item2);
        }

        [Test]
        public void Read_Returns_Correct_Uint32_Value([ValueSource(nameof(GetReadUIntTestData))]Tuple<uint, byte[]> data)
        {
            VerifyReadMethod(data.Item1, data.Item2);
        }        

        [Test]
        public void Read_Returns_Correct_Int64_Value([ValueSource(nameof(GetReadLongTestData))]Tuple<long, byte[]> data)
        {
            VerifyReadMethod(data.Item1, data.Item2);
        }        

        [Test]
        public void Read_Returns_Correct_UInt64_Value([ValueSource(nameof(GetReadULongTestData))]Tuple<ulong, byte[]> data)
        {
            VerifyReadMethod(data.Item1, data.Item2);
        }

        [Test]
        public void Read_Returns_Correct_String_Value([ValueSource(nameof(GetReadStringTestData))]Tuple<string, byte[]> data)
        {
            VerifyReadMethod(data.Item1, data.Item2);
        }

        private void VerifyReadMethod(object expectedValue, string hexString, byte code)
        {
            List<byte> bytes = new List<byte>();
            bytes.Add(code);
            bytes.AddRange(BinaryUtils.ToByteArray(hexString));
            
            VerifyReadMethod(expectedValue, bytes.ToArray());
        }

        private void VerifyReadMethod(object expectedValue, byte[] bytes)
        {
            buffer.Write(bytes);

            object actualValue = reader.Read();

            Assert.That(actualValue, Is.EqualTo(expectedValue));
        }

        #endregion

        #region Generic Read tests

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void GenericRead_Throws_Exception_For_Unknown_Type_Code()
        {
            buffer.Write(0xFE, 9, 8, 7, 6, 5);
            reader.Read<int>();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GenericRead_Throws_Exception_For_Invalid_Types()
        {
            buffer.Write(DataTypeCodes.Byte, 6, 0, 1);
            reader.Read<short>();
        }

        [Test]
        public void GenericRead_Returns_Correct_Byte_Value([ValueSource(nameof(ReadUByteTestData))]byte value)
        {
            VerifyGenericReadMethod(value, value.ToString("x2"), DataTypeCodes.UByte);
        }

        [Test]
        public void GenericRead_Returs_Correct_Byte_Value([ValueSource(nameof(ReadByteTestData))]sbyte value)
        {
            VerifyGenericReadMethod(value, value.ToString("x2"), DataTypeCodes.Byte);
        }

        [Test]
        public void GenericRead_Returns_Correct_Int16_Value([ValueSource(nameof(ReadShortTestData))]short value)
        {
            VerifyGenericReadMethod(value, value.ToString("x4"), DataTypeCodes.Short);
        }

        [Test]
        public void GenericRead_Returns_Correct_UInt16_Value([ValueSource(nameof(ReadUShortTestData))]ushort value)
        {
            VerifyGenericReadMethod(value, value.ToString("x4"), DataTypeCodes.UShort);
        }

        [Test]
        public void GenericRead_Returns_Correct_Int32_Value([ValueSource(nameof(GetReadIntData))]Tuple<int, byte[]> data)
        {
            VerifyGenericReadMethod(data.Item1, data.Item2);
        }

        [Test]
        public void GenericRead_Returns_Correct_UInt32_Value([ValueSource(nameof(GetReadUIntTestData))]Tuple<uint, byte[]> data)
        {
            VerifyGenericReadMethod(data.Item1, data.Item2);
        }        

        [Test]
        public void GenericRead_Returns_Correct_Int64_Value([ValueSource(nameof(GetReadLongTestData))]Tuple<long, byte[]> data)
        {
            VerifyGenericReadMethod(data.Item1, data.Item2);
        }

        [Test]
        public void GenericRead_Returns_Correct_UInt64_Value([ValueSource(nameof(GetReadULongTestData))]Tuple<ulong, byte[]> data)
        {
            VerifyGenericReadMethod(data.Item1, data.Item2);
        }

        [Test]
        public void GenericRead_Returns_Correct_String_Value([ValueSource(nameof(GetReadStringTestData))]Tuple<string, byte[]> data)
        {
            VerifyGenericReadMethod(data.Item1, data.Item2);
        }

        private void VerifyGenericReadMethod<T>(T expectedValue, string hexString, byte code)
        {
            List<byte> bytes = new List<byte>();
            bytes.Add(code);
            bytes.AddRange(BinaryUtils.ToByteArray(hexString));
            VerifyGenericReadMethod(expectedValue, bytes.ToArray());
        }

        private void VerifyGenericReadMethod<T>(T expectedValue, byte[] serializedData)
        {
            buffer.Write(serializedData);

            T actualValue = reader.Read<T>();

            Assert.That(actualValue, Is.EqualTo(expectedValue));
        }

        #endregion
    }
}
