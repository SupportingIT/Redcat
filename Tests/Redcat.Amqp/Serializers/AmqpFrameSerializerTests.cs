using FakeItEasy;
using NUnit.Framework;
using Redcat.Amqp.Serializers;
using System.Collections.Generic;
using System.IO;

namespace Redcat.Amqp.Tests.Serializers
{
    [TestFixture]
    public class AmqpFrameSerializerTests
    {
        private AmqpFrameSerializer serializer;
        private MemoryStream stream;
        private IPayloadSerializer payloadSerializer;

        [SetUp]
        public void SetUp()
        {
            stream = new MemoryStream();
            payloadSerializer = A.Fake<IPayloadSerializer>();
            serializer = new AmqpFrameSerializer(stream, payloadSerializer);
        }

        [Test]
        public void Serialize_Serializes_Frame_Without_Payload()
        {
            AmqpFrame frame = new AmqpFrame(null);
            byte[] expectedBytes = { 0x00, 0x00, 0x00, 0x08, 0x2, 0x00, 0x00, 0x00 };

            serializer.Serialize(frame);

            var actualBytes = stream.ToArray();
            CollectionAssert.AreEqual(expectedBytes, actualBytes);
        }

        [Test]
        public void Serialize_Uses_Payload_Serializer()
        {
            string payload = "payload";
            AmqpFrame frame = new AmqpFrame(payload);
            serializer.Serialize(frame);

            A.CallTo(() => payloadSerializer.Serialize(A<AmqpDataWriter>._, payload)).MustHaveHappened();
        }

        [Test]
        public void Serialize_Serializes_Payload()
        {
            byte[] payload = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 };            
            ConfigurePayloadSerializer(payload);
            List<byte> expectedBytes = new List<byte> { 0x00, 0x00, 0x00, 0x0e, 0x2, 0x00, 0x00, 0x00 };
            expectedBytes.AddRange(payload);

            serializer.Serialize(new AmqpFrame(payload));
                        
            CollectionAssert.AreEqual(expectedBytes, stream.ToArray());
        }

        [Test]
        public void Serialize_Can_Serialize_More_Then_One_Frame()
        {
            byte[] payload = { 0xb1, 0xb2, 0xb3, 0xb4, 0xb5, 0xb6 };
            ConfigurePayloadSerializer(payload);
            List<byte> expectedBytes = new List<byte> { 0x00, 0x00, 0x00, 0x0e, 0x2, 0x00, 0x00, 0x00 };
            expectedBytes.AddRange(payload);
            expectedBytes.AddRange(expectedBytes);

            serializer.Serialize(new AmqpFrame(payload));
            serializer.Serialize(new AmqpFrame(payload));

            CollectionAssert.AreEqual(expectedBytes, stream.ToArray());
        }

        private void ConfigurePayloadSerializer(byte[] payload)
        {
            A.CallTo(() => payloadSerializer.Serialize(A<AmqpDataWriter>._, payload)).Invokes(c =>
            {
                var stream = c.GetArgument<Stream>(0);
                stream.Write(payload, 0, payload.Length);
            });
        }
    }
}
