using NUnit.Framework;
using System;
using System.Linq;

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

        private byte[] CreateByteArray(int count)
        {
            return Enumerable.Range(1, count).Select(i => (byte)i).ToArray();
        }
    }
}
