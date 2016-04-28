using NUnit.Framework;
using Redcat.Amqp.Serialization;
using Redcat.Core;

namespace Redcat.Amqp.Tests.Serialization
{
    [TestFixture]
    public class AmqpListReaderTests
    {
        private ByteBuffer buffer;
        private AmqpListReader reader;

        [SetUp]
        public void SetUp()
        {
            buffer = new ByteBuffer(100);
            reader = new AmqpListReader();
        }

        [Test]
        public void Read_Returns_Null_For_List0()
        {
            byte[] list = { DataTypeCodes.List0, 0, 0 };
            buffer.Write(list);

            object value = reader.Read(buffer);

            Assert.That(value, Is.Null);
        }

        [Test]
        public void Read_Initializes_List8_Reading_And_Returns_First_Element()
        {
            byte size = 3;
            byte count = 2;
            byte[] list = { DataTypeCodes.List8, size, count, DataTypeCodes.Short, 0x00, 0x12 };
                        
            VerifyReadMethod((short)0x0012, size, count, list);
        }

        [Test]
        public void Read_Initializes_List32_Reading_And_Returns_First_Element()
        {
            byte size = 6;
            byte count = 3;
            byte[] list = { DataTypeCodes.List32, 0, 0, 0, size, 0, 0, 0, count, DataTypeCodes.UByte, 0x90 };

            VerifyReadMethod((byte)0x90, size, count, list);
        }

        private void VerifyReadMethod(object expectedValue, int size, int count, byte[] list)
        {
            buffer.Write(list);

            object value = reader.Read(buffer);

            Assert.That(reader.IsReadingList, Is.True);
            Assert.That(reader.ByteSize, Is.EqualTo(size));
            Assert.That(reader.ElementCount, Is.EqualTo(count));
            Assert.That(value, Is.EqualTo(expectedValue));
        }
    }
}
