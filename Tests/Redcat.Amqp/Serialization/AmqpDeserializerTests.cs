using NUnit.Framework;
using Redcat.Amqp.Serialization;
using System;

namespace Redcat.Amqp.Tests.Serialization
{
    [TestFixture]
    public class AmqpDeserializerTests
    {
        [Test]
        public void Read_Deserializes_ProtocolHeader()
        {
            ProtocolHeader expectedHeader = new ProtocolHeader(ProtocolType.Sasl, new Version(4, 5, 6));
            ProtocolHeader actualHeader = null;
            AmqpDeserializer deserializer = new AmqpDeserializer();
            deserializer.ProtocolHeaderDeserialized += h => actualHeader = h;

            deserializer.Read(new ArraySegment<byte>(new byte[] { (byte)'A', (byte)'M', (byte)'Q', (byte)'P',
                                                                  (byte)expectedHeader.ProtocolType, 4, 5, 6 }));

            Assert.That(actualHeader, Is.EqualTo(expectedHeader));
        }
    }
}
