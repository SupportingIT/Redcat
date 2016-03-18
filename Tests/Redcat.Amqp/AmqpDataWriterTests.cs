using NUnit.Framework;
using System.IO;
using System.Linq;

namespace Redcat.Amqp.Tests
{
    [TestFixture]
    public class AmqpDataWriterTests
    {
        private MemoryStream stream;
        private AmqpDataWriter writer;

        [SetUp]
        public void SetUp()
        {
            stream = new MemoryStream();
            writer = new AmqpDataWriter(stream);
        }

        [Test]
        public void WriteNull_Test()
        {            
            writer.WriteNull();

            VerifyWrittenByte(DataTypeCodes.Null);
        }

        [Test]
        public void WriteTrue_Test()
        {
            writer.WriteTrue();

            VerifyWrittenByte(DataTypeCodes.TrueValue);
        }

        [Test]
        public void WriteFalse_Test()
        {
            writer.WriteFalse();

            VerifyWrittenByte(DataTypeCodes.FalseValue);
        }

        private void VerifyWrittenByte(byte expectedByte)
        {
            byte actualByte = stream.ToArray().Single();
            Assert.That(actualByte, Is.EqualTo(expectedByte));
        }

        [Test]
        public void Write_BooleanValue_True_Test()
        {
            writer.Write(true);

            VerifyWrittenBytes(DataTypeCodes.Boolean, 0x01);
        }

        [Test]
        public void Write_BooleanValue_False_Test()
        {
            writer.Write(false);

            VerifyWrittenBytes(DataTypeCodes.Boolean, 0x00);
        }

        [Test]
        public void Write_ByteValue_Test()
        {
            byte value = 0xF1;

            writer.Write(value);

            VerifyWrittenBytes(DataTypeCodes.UByte, value);
        }

        [Test]
        public void Write_SByteValue_Test()
        {
            sbyte value = -2;

            writer.Write(value);

            VerifyWrittenBytes(DataTypeCodes.Byte, 0xfe);
        }

        [Test]
        public void Write_UShortValue_Test()
        {
            ushort value = 0x2021;

            writer.Write(value);

            VerifyWrittenBytes(DataTypeCodes.UShort, 0x20, 0x21);
        }

        [Test]
        public void Write_ShortValue_Test()
        {
            short value = 0x1110;

            writer.Write(value);

            VerifyWrittenBytes(DataTypeCodes.Short, 0x11, 0x10);
        }

        [Test]
        public void Write_IntValue_Test()
        {
            unchecked
            {
                int value = (int)0xf0f1f2f3;

                writer.Write(value);

                VerifyWrittenBytes(DataTypeCodes.Int, 0xf0, 0xf1, 0xf2, 0xf3);
            }
        }

        [Test]
        public void Write_UIntValue_Test()
        {
            uint value = 0xe0e1e2e3;

            writer.Write(value);

            VerifyWrittenBytes(DataTypeCodes.UInt, 0xe0, 0xe1, 0xe2, 0xe3);
        }

        [Test]
        public void Write_ULongValue_Test()
        {
            ulong value = 0xa0a1a2a3a4a5a6a7;

            writer.Write(value);

            VerifyWrittenBytes(DataTypeCodes.ULong, 0xa0, 0xa1, 0xa2, 0xa3, 0xa4, 0xa5, 0xa6, 0xa7);
        }

        [Test]
        public void Write_LongValue_Test()
        {
            unchecked
            {
                long value = (long)0xa0a1a2a3a4a5a6a7;

                writer.Write(value);

                VerifyWrittenBytes(DataTypeCodes.Long, 0xa0, 0xa1, 0xa2, 0xa3, 0xa4, 0xa5, 0xa6, 0xa7);
            }
        }

        private void VerifyWrittenBytes(params byte[] expectedBytes)
        {
            byte[] actualBytes = stream.ToArray();
            CollectionAssert.AreEqual(expectedBytes, actualBytes);
        }
    }
}
