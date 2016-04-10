using NUnit.Framework;
using Redcat.Core.Serialization;
using System;

namespace Redcat.Core.Tests.Serialization
{
    [TestFixture]
    public class ByteArrayExtensionTests
    {
        [Test]
        public void ReadInt16_Deserializes_Int16()
        {            
            byte[] bytes = { 0x0f, 0xaa, 0xff, 0xff };
            short expectedValue = -1;

            short actualValue = bytes.ReadInt16(2);

            Assert.That(actualValue, Is.EqualTo(expectedValue));
        }

        [Test]
        public void ReadUInt16_Deserializes_UInt16()
        {
            byte[] bytes = { 0x0a, 0x0b, 0xdd };
            ushort expectedValue = 0x0bdd;

            ushort actualValue = bytes.ReadUInt16(1);

            Assert.That(actualValue, Is.EqualTo(expectedValue));
        }

        [Test]
        public void ReadInt32_Deserializes_Int32()
        {
            byte[] bytes = { 0xff, 0xff, 0x01, 0x02 };
            unchecked
            {
                int expectedValue = (int)0xffff0102;
                int actualValue = bytes.ReadInt32();
                Assert.That(actualValue, Is.EqualTo(expectedValue));
            }
        }

        [Test]
        public void ReadUInt32_Deserializes_UInt32()
        {
            byte[] bytes = { 0x98, 0x76, 0x54, 0x32 };
            uint expectedValue = 0x98765432;

            uint actualValue = bytes.ReadUInt32();

            Assert.That(actualValue, Is.EqualTo(expectedValue));
        }

        [Test]
        public void ReadInt64_Deserializes_Int64()
        {
            byte[] bytes = { 0xfe, 0xee, 0xee, 0xee, 0xee, 0xee, 0xee, 0xee };
            unchecked
            {
                long expectedValue = (long)0xfeeeeeeeeeeeeeee;
                long actualValue = bytes.ReadInt64();

                Assert.That(actualValue, Is.EqualTo(expectedValue));
            }            
        }

        [Test]
        public void ReadUInt64_Deserializes_UInt64()
        {
            Assert.Fail();
        }
    }
}
