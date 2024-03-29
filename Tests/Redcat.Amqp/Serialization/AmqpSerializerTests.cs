﻿using FakeItEasy;
using NUnit.Framework;
using Redcat.Amqp.Serialization;
using System;
using System.Collections.Generic;
using System.IO;

namespace Redcat.Amqp.Tests.Serialization
{
    [TestFixture]
    public class AmqpSerializerTests
    {
        private AmqpSerializer serializer;
        private MemoryStream stream;
        private IPayloadSerializer payloadSerializer;

        [SetUp]
        public void SetUp()
        {
            stream = new MemoryStream();
            payloadSerializer = A.Fake<IPayloadSerializer>();
            serializer = new AmqpSerializer(payloadSerializer);
        }

        [Test]
        public void Serialize_Serializes_Frame_Without_Payload()
        {
            AmqpFrame frame = new AmqpFrame(null);
            byte[] expectedBytes = { 0x00, 0x00, 0x00, 0x08, 0x2, 0x00, 0x00, 0x00 };

            serializer.Serialize(stream, frame);

            var actualBytes = stream.ToArray();
            CollectionAssert.AreEqual(expectedBytes, actualBytes);
        }

        [Test]
        public void Serialize_Uses_Payload_Serializer()
        {
            string payload = "payload";
            AmqpFrame frame = new AmqpFrame(payload);
            serializer.Serialize(stream, frame);

            A.CallTo(() => payloadSerializer.Serialize(A<AmqpDataWriter>._, payload)).MustHaveHappened();
        }

        [Test]
        public void Serialize_Serializes_Payload()
        {
            byte[] payload = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 };            
            ConfigurePayloadSerializer(payload);
            List<byte> expectedBytes = new List<byte> { 0x00, 0x00, 0x00, 0x0e, 0x2, 0x00, 0x00, 0x00 };
            expectedBytes.AddRange(payload);

            serializer.Serialize(stream, new AmqpFrame(payload));
                        
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

            serializer.Serialize(stream, new AmqpFrame(payload));
            serializer.Serialize(stream, new AmqpFrame(payload));

            CollectionAssert.AreEqual(expectedBytes, stream.ToArray());
        }

        private void ConfigurePayloadSerializer(byte[] payload)
        {
            A.CallTo(() => payloadSerializer.Serialize(A<AmqpDataWriter>._, payload)).Invokes(c =>
            {
                var writer = c.GetArgument<AmqpDataWriter>(0);
                writer.WriteRaw(payload);
            });
        }

        [Test]
        public void Serialize_Serializes_ProtocolHeader()
        {
            ProtocolHeader header = new ProtocolHeader(ProtocolType.Sasl, new Version(1, 2, 3, 4));
            byte[] expectedBytes = { (byte)'A', (byte)'M', (byte)'Q', (byte)'P', (byte)header.ProtocolType, 1, 2, 3 };

            serializer.Serialize(stream, header);

            CollectionAssert.AreEqual(expectedBytes, stream.ToArray());
        }
    }
}
