using FakeItEasy;
using NUnit.Framework;
using Redcat.Amqp.Serialization;
using Redcat.Test;
using System;
using System.Collections.Generic;

namespace Redcat.Amqp.Tests.Serialization
{
    [TestFixture]
    public class AmqpDeserializerTests
    {
        private IPayloadReader payloadReader;
        private AmqpDeserializer deserializer;

        [SetUp]
        public void SetUp()
        {
            payloadReader = A.Fake<IPayloadReader>();
            deserializer = new AmqpDeserializer(payloadReader);
        }

        [Test]
        public void Read_Deserializes_ProtocolHeader()
        {
            ProtocolHeader expectedHeader = new ProtocolHeader(ProtocolType.Sasl, new Version(4, 5, 6));
            ProtocolHeader actualHeader = null;            
            deserializer.ProtocolHeaderDeserialized += h => actualHeader = h;

            deserializer.Read(new ArraySegment<byte>(new byte[] { (byte)'A', (byte)'M', (byte)'Q', (byte)'P',
                                                                  (byte)expectedHeader.ProtocolType, 4, 5, 6 }));

            Assert.That(actualHeader, Is.EqualTo(expectedHeader));
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void Read_Throws_Exception_If_Size_Field_Lesser_Then_8()
        {
            byte[] frame = GetSerializedFrame(7, 0);
            
            deserializer.Read(frame);
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void Read_Throws_Exception_If_Doff_Lesser_Then_2()
        {
            byte[] frame = GetSerializedFrame(8, 0, 1);
            
            deserializer.Read(frame);
        }

        [Test]
        public void Read_Correctly_Deserializes_Channel()
        {
            short channel = 4;
            AmqpFrame frame = null;            
            byte[] serializedFrame = GetSerializedFrame(8, 0, 2, channel);
            deserializer.Deserialized += f => frame = f;
            deserializer.Read(serializedFrame);

            Assert.That(frame.Channel, Is.EqualTo(channel));
        }

        [Test]
        public void Read_Does_Not_Deserializes_Frame_If_Frame_Size_Greater_Then_Byte_Count_In_Buffer()
        {
            AmqpFrame frame = null;
            byte[] serializedFrame = GetSerializedFrame(12, 0, 2, 0, new byte[] { 0, 1 });
            deserializer.Deserialized += f => frame = f;
            deserializer.Read(serializedFrame);

            Assert.That(frame, Is.Null);
        }

        private byte[] GetSerializedFrame(int size = 8, byte type = 0, byte doff = 2, short channel = 0, byte[] payload = null)
        {
            List<byte> serialized = new List<byte>();
            serialized.Add(size);
            serialized.Add(doff);
            serialized.Add(type);            
            serialized.Add(channel);
            if (payload != null) serialized.AddRange(payload);

            return serialized.ToArray();
        }
    }
}
