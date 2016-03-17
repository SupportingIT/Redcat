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
        public void Write_UShortValue_Test()
        {
            ushort value = 0x2021;

            writer.Write(value);

            VerifyWrittenBytes(DataTypeCodes.UShort, 0x20, 0x21);
        }

        private void VerifyWrittenBytes(params byte[] expectedBytes)
        {
            byte[] actualBytes = stream.ToArray();
            CollectionAssert.AreEqual(expectedBytes, actualBytes);
        }
    }
}
