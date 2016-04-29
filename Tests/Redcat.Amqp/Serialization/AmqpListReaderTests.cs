using NUnit.Framework;
using Redcat.Amqp.Serialization;
using Redcat.Core;
using System;

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
        public void IsEmptyList_Returns_True_For_List0_Type()
        {
            buffer.Write(DataTypeCodes.List0, 9, 1, 2);

            Assert.That(reader.IsEmptyList(buffer), Is.True);
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

        [Test]
        public void Read_Decreases_ByteSize_And_Element_Count_Properties()
        {
            buffer.Write(DataTypeCodes.List8, 10, 3, DataTypeCodes.UByte, 0xfd, DataTypeCodes.Short, 0x01, 0x21, DataTypeCodes.Int, 0, 0, 0x10, 0x30);
            reader.Read(buffer);
            VerifyReaderState(8, 2, false);
            reader.Read(buffer);
            VerifyReaderState(5, 1, false);
            reader.Read(buffer);
            VerifyReaderState(0, 0, true);
        }

        private void VerifyReaderState(uint size, uint count, bool endOfList)
        {
            Assert.That(reader.ByteSize, Is.EqualTo(size));
            Assert.That(reader.ElementCount, Is.EqualTo(count));
            Assert.That(reader.EndOfList, Is.EqualTo(endOfList));
        }

        private void VerifyReadMethod(object expectedValue, int size, int count, byte[] list)
        {
            buffer.Write(list);

            object value = reader.Read(buffer);

            Assert.That(reader.EndOfList, Is.False);
            Assert.That(reader.ByteSize, Is.EqualTo(size));
            Assert.That(reader.ElementCount, Is.EqualTo(count));
            Assert.That(value, Is.EqualTo(expectedValue));
        }

        [Test]
        public void EndRead_Clears_Buffer_And_Set_EndOfList_False()
        {
            buffer.Write(DataTypeCodes.List8, 11, 3, DataTypeCodes.Short, 0, 0, DataTypeCodes.Short, 0, 0, DataTypeCodes.Int, 0, 0, 0, 0);
            reader.Read(buffer);

            reader.EndRead(buffer);

            VerifyReaderState(0, 0, true);
            Assert.That(buffer.Count, Is.EqualTo(0));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EndRead_Throws_Exception_If_Does_Not_Reading_List()
        {
            buffer.Write(DataTypeCodes.UByte, 12, DataTypeCodes.Short, 0, 0);
            reader.Read(buffer);
            reader.EndRead(buffer);
        }
    }
}
