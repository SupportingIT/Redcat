using NUnit.Framework;
using Redcat.Amqp.Performatives;
using Redcat.Amqp.Serializers;
using System.Collections.Generic;

namespace Redcat.Amqp.Tests.Serializers
{
    [TestFixture]
    public class PayloadSerializerTests
    {
        [Test]
        public void Serialize_CorrectlySerializes_Open_Performative()
        {
            PayloadSerializer serializer = new PayloadSerializer();
            Open performative = new Open { ContainerId = "Container123" };
            //List<byte> expectedBytes = new List<byte> { 0x00, 0xA3, (byte)Descriptors.Open.Length };
            Assert.Fail();
        }

        [Test]
        public void Serialize_Correctly_Serializes_Close_Performative()
        {
            Assert.Fail();
        }
    }
}
