using NUnit.Framework;
using Redcat.Test;
using System;
using System.Linq;
using System.Text;

namespace Redcat.Core.Tests
{
    [TestFixture]
    public class ByteBufferTests
    {
        #region Indexer tests

        [Test]
        public void Indexer_Returns_Correct_Elements_After_Write()
        {
            ByteBuffer buffer = new ByteBuffer(3);
            byte[] data = { 6, 5, 4 };
            buffer.Write(data);

            Assert.That(buffer[0], Is.EqualTo(data[0]));
            Assert.That(buffer[1], Is.EqualTo(data[1]));
            Assert.That(buffer[2], Is.EqualTo(data[2]));
        }

        [Test]
        public void Indexer_Returns_Correct_Elements_After_Discard()
        {
            ByteBuffer buffer = new ByteBuffer(5);
            byte[] data = { 6, 7, 8, 9, 1 };
            buffer.Write(data);

            buffer.Discard(2);

            Assert.That(buffer[0], Is.EqualTo(data[2]));
            Assert.That(buffer[1], Is.EqualTo(data[3]));
            Assert.That(buffer[2], Is.EqualTo(data[4]));
        }

        [Test]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void Indexer_Throws_Exception_If_Index_Less_Then_Zero()
        {
            ByteBuffer buffer = new ByteBuffer(5);
            buffer.Write(new byte[] { 4, 3, 2, 1, 0 });
            buffer.Discard(2);

            byte value = buffer[-1];
        }

        [Test]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void Indexer_Throws_Exception_If_Index_Greater_Than_Count()
        {
            ByteBuffer buffer = new ByteBuffer(5);
            buffer.Write(new byte[] { 3, 1, 2 });

            byte value = buffer[4];
        }

        #endregion

        #region Write method tests

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Write_Throws_Exception_If_Source_Array_Greater_Then_Available()
        {
            ByteBuffer buffer = new ByteBuffer(10);
            byte[] data = CreateByteArray(11);

            buffer.Write(data);
        }

        [Test]
        public void Write_Correctly_Sets_Capacity_Available_And_Count()
        {
            ByteBuffer buffer = new ByteBuffer(12);
            byte[] data = CreateByteArray(8);

            buffer.Write(data);

            Assert.That(buffer.Capacity, Is.EqualTo(12));
            Assert.That(buffer.Available, Is.EqualTo(4));
            Assert.That(buffer.Count, Is.EqualTo(8));
        }

        [Test]
        public void Write_Correctly_Writes_Data_After_Discard()
        {
            ByteBuffer buffer = new ByteBuffer(10);
            byte[] data = { 10, 9, 8, 7, 6, 5, 4, 3 };

            buffer.Write(data);
            buffer.Discard(2);
            CollectionAssert.AreEqual(new byte[] { 8, 7, 6, 5, 4, 3 }, buffer);
            buffer.Write(new byte[] { 2, 1, 0, 10 });

            CollectionAssert.AreEqual(new byte[] { 8, 7, 6, 5, 4, 3, 2, 1, 0, 10 }, buffer);
        }

        #endregion

        #region Discard method tests

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Discard_Throws_Exception_If_Count_Greater_Then_Available()
        {
            ByteBuffer buffer = new ByteBuffer(10);
            byte[] data = CreateByteArray(11);

            buffer.Write(data);
        }

        [Test]
        public void Discard_Removes_Elements()
        {
            ByteBuffer buffer = new ByteBuffer(5);
            byte[] data = { 5, 4, 3, 2, 1 };

            buffer.Write(data);
            buffer.Discard(2);

            CollectionAssert.AreEqual(new byte[] { 3, 2, 1 }, buffer);
        }

        [Test]
        public void Discard_Updates_Available_And_Count_Properties()
        {
            ByteBuffer buffer = new ByteBuffer(10);
            byte[] data = CreateByteArray(9);

            buffer.Write(data);
            Assert.That(buffer.Available, Is.EqualTo(1));
            Assert.That(buffer.Count, Is.EqualTo(9));

            buffer.Discard(3);
            Assert.That(buffer.Available, Is.EqualTo(4));
            Assert.That(buffer.Count, Is.EqualTo(6));
        }

        #endregion

        [Test]
        public void ReadByte_Deserializes_Byte_From_Buffer()
        {           
            byte value = 0xad;
            VerifyReadValueMethod(value, value.ToString("x2"), b => b.ReadByte());
        }

        [Test]
        public void ReadSByte_Deserializes_SByte_Value()
        {
            sbyte value = -10;
            VerifyReadValueMethod(value, value.ToString("x2"), b => b.ReadSByte());
        }

        [Test]
        public void ReadInt16_Deserializes_Int16_Value()
        {
            short value = 8765;
            VerifyReadValueMethod(value, value.ToString("x4"), b => b.ReadInt16());
        }

        [Test]
        public void ReadUInt16_Deserializes_UInt16_Value()
        {
            ushort value = 100;
            VerifyReadValueMethod(value, value.ToString("x4"), b => b.ReadUInt16());
        }

        [Test]
        public void ReadInt32_Deserializes_Int32_Value()
        {
            int value = 89078;
            VerifyReadValueMethod(value, value.ToString("x8"), b => b.ReadInt32());
        }

        [Test]
        public void ReadUInt32_Deserializes_UInt32_Value()
        {
            uint value = 2143123;
            VerifyReadValueMethod(value, value.ToString("x8"), b => b.ReadUInt32());
        }

        [Test]
        public void ReadInt64_Deserializes_Int64_Value()
        {
            long value = 5123452353;
            VerifyReadValueMethod(value, value.ToString("x16"), b => b.ReadInt64());
        }

        [Test]
        public void ReadUInt64_Deserializes_UInt64_Value()
        {
            ulong value = 4562542344;
            VerifyReadValueMethod(value, value.ToString("x16"), b => b.ReadUInt64());
        }

        [Test]
        public void ReadString_Deserializes_String_Value()
        {
            string value = "Hello everybody";
            byte[] serialized = Encoding.UTF8.GetBytes(value);
            ByteBuffer buffer = new ByteBuffer(30);
            buffer.Write(serialized);
            Assert.That(buffer.Count, Is.EqualTo(value.Length));

            string actualValue = buffer.ReadString(value.Length);

            Assert.That(actualValue, Is.EqualTo(value));
        }

        private void VerifyReadValueMethod<T>(T value, string hexValue, Func<ByteBuffer, T> readValue)
        {
            ByteBuffer buffer = new ByteBuffer(10);
            byte[] bytes = BinaryUtils.ToByteArray(hexValue);
            buffer.Write(bytes);
            Assert.That(buffer.Count, Is.EqualTo(bytes.Length));

            T actualValue = readValue(buffer);

            Assert.That(actualValue, Is.EqualTo(value));
            Assert.That(buffer.Count, Is.EqualTo(0));
        }

        [Test]
        public void ToString_Returns_String_Constructed_From_Buffer_Bytes()
        {
            ByteBuffer buffer = new ByteBuffer(20);
            byte[] data = Encoding.UTF8.GetBytes("Hello world");
            buffer.Write(data);

            Assert.That(buffer.ToString(), Is.EqualTo("Hello world"));
        }

        [Test]
        public void ToString_Returns_Correct_Substring()
        {
            ByteBuffer buffer = new ByteBuffer(15);
            byte[] data = Encoding.UTF8.GetBytes("Hola world");
            buffer.Write(data);

            Assert.That(buffer.ToString(5, 4), Is.EqualTo("worl"));
        }

        private byte[] CreateByteArray(int count)
        {
            return Enumerable.Range(1, count).Select(i => (byte)i).ToArray();
        }
    }
}
