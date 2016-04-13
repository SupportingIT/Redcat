using NUnit.Framework;
using Redcat.Core.Serialization;
using Redcat.Test;
using System;
using System.IO;

namespace Redcat.Core.Tests.Serialization
{
    [TestFixture]
    public class StreamExtensionTests
    {
        private MemoryStream stream;

        [SetUp]
        public void SetUp()
        {
            stream = new MemoryStream();
        }

        short[] WriteInt16TestData = { short.MinValue, 0, short.MaxValue };

        [Test]
        public void Write_Correctly_Serializes_Int16_Value([ValueSource(nameof(WriteInt16TestData))]short value)
        {
            VerifyWriteMethod(value.ToString("x4"), s => s.Write(value));
        }

        ushort[] WriteUInt16TestData = { 0, ushort.MaxValue / 2, ushort.MaxValue };

        [Test]
        public void Write_Correctly_Serializes_UInt16([ValueSource(nameof(WriteUInt16TestData))]ushort value)
        {
            VerifyWriteMethod(value.ToString("x4"), s => s.Write(value));
        }

        int[] WriteInt32TestData = { int.MinValue, 0, int.MaxValue };

        [Test]
        public void Write_Correctly_Serializes_Int32([ValueSource(nameof(WriteInt32TestData))]int value)
        {
            VerifyWriteMethod(value.ToString("x8"), s => s.Write(value));
        }

        uint[] WriteUInt32TestData = { 0, uint.MaxValue / 2, uint.MaxValue };

        [Test]
        public void Write_Correctly_Serializes_UInt32_Value([ValueSource(nameof(WriteUInt32TestData))]uint value)
        {
            VerifyWriteMethod(value.ToString("x8"), s => s.Write(value));
        }

        long[] WriteInt64TestData = { long.MinValue, 0, long.MaxValue };

        [Test]
        public void Write_Correctly_Serializes_Int64_Value([ValueSource(nameof(WriteInt64TestData))]long value)
        {
            VerifyWriteMethod(value.ToString("x16"), s => s.Write(value));
        }

        ulong[] WriteUInt64TestData = { 0, ulong.MaxValue / 2, ulong.MaxValue };

        [Test]
        public void Write_Correctly_Serializes_UInt64([ValueSource(nameof(WriteUInt64TestData))]ulong value)
        {
            VerifyWriteMethod(value.ToString("x16"), s => s.Write(value));
        }

        private void VerifyWriteMethod(string hexString, Action<Stream> write)
        {
            byte[] expected = BinaryUtils.ToByteArray(hexString);

            write(stream);

            byte[] actual = stream.ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
