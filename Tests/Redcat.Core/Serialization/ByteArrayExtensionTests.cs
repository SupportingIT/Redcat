using NUnit.Framework;
using Redcat.Core.Serialization;
using Redcat.Test;
using System;
using System.Text;

namespace Redcat.Core.Tests.Serialization
{
    [TestFixture]
    public class ByteArrayExtensionTests
    {
        short[] ReadInt16TestData = { short.MinValue, 0, short.MaxValue };

        [Test]
        public void ReadInt16_Deserializes_Int16([ValueSource(nameof(ReadInt16TestData))]short expectedValue)
        {           
            ValidateReadValueMethod(expectedValue, expectedValue.ToString("x4"), b => b.ReadInt16());
        }

        ushort[] ReadUInt16TestData = { 0, ushort.MaxValue / 2, ushort.MaxValue };

        [Test]
        public void ReadUInt16_Deserializes_UInt16([ValueSource(nameof(ReadUInt16TestData))]ushort expectedValue)
        {
            ValidateReadValueMethod(expectedValue, expectedValue.ToString("x4"), b => b.ReadUInt16());
        }

        int[] ReadInt32TestData = { int.MinValue, 0, int.MaxValue };

        [Test]
        public void ReadInt32_Deserializes_Int32([ValueSource(nameof(ReadInt32TestData))]int expectedValue)
        {
            ValidateReadValueMethod(expectedValue, expectedValue.ToString("x8"), b => b.ReadInt32());
        }

        uint[] ReadUInt32TestData = { 0, uint.MaxValue / 2, uint.MaxValue };

        [Test]
        public void ReadUInt32_Deserializes_UInt32([ValueSource(nameof(ReadUInt32TestData))]uint expectedValue)
        {
            ValidateReadValueMethod(expectedValue, expectedValue.ToString("x8"), b => b.ReadUInt32());
        }

        long[] ReadInt64TestData = { long.MinValue, 0, long.MaxValue };

        [Test]
        public void ReadInt64_Deserializes_Int64([ValueSource(nameof(ReadInt64TestData))]long expectedValue)
        {
            ValidateReadValueMethod(expectedValue, expectedValue.ToString("x16"), b => b.ReadInt64());
        }

        ulong[] ReadUInt64TestData = { 0, ulong.MaxValue / 2, ulong.MaxValue };

        [Test]
        public void ReadUInt64_Deserializes_UInt64([ValueSource(nameof(ReadUInt64TestData))]ulong expectedValue)
        {
            ValidateReadValueMethod(expectedValue, expectedValue.ToString("x16"), b => b.ReadUInt64());
        }

        [Test]
        public void ReadString_Deserializes_String()
        {
            byte[] serialized = Encoding.UTF8.GetBytes("Hello world");

            string actualString = serialized.ReadString(5);

            Assert.That(actualString, Is.EqualTo("Hello"));
        }

        private void ValidateReadValueMethod<T>(T expectedValue, string hexString, Func<byte[], T> readValue)
        {
            byte[] bytes = BinaryUtils.ToByteArray(hexString);

            T actualValue = readValue(bytes);

            Assert.That(actualValue, Is.EqualTo(expectedValue));
        }        
    }
}
