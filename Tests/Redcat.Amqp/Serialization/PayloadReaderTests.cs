using FakeItEasy;
using NUnit.Framework;
using Redcat.Amqp.Serialization;
using Redcat.Core;
using Redcat.Test;
using System;
using System.Collections.Generic;

namespace Redcat.Amqp.Tests.Serialization
{
    [TestFixture]
    public class PayloadReaderTests
    {
        private ByteBuffer buffer;
        private AmqpDataReader dataReader;

        private PayloadReader payloadReader;
        private IPayloadReader childReader;

        [SetUp]
        public void SetUp()
        {
            buffer = new ByteBuffer(100);
            dataReader = new AmqpDataReader(buffer);

            payloadReader = new PayloadReader();
            childReader = A.Fake<IPayloadReader>();
        }

        [Test]
        public void Read_Uses_Child_Reader_For_Specied_ULong_Descriptor()
        {
            ulong descriptor = 100;
            payloadReader.AddChildReader(descriptor, null, childReader);
            
            List<byte> bytes = new List<byte>();
            bytes.Add(DataTypeCodes.Descriptor);
            bytes.Add(DataTypeCodes.ULong);
            bytes.Add(descriptor);
            buffer.Write(bytes.ToArray());

            payloadReader.Read(dataReader);

            A.CallTo(() => childReader.Read(dataReader)).MustHaveHappened();
        }

        [Test]
        public void Read_Uses_Child_Reader_For_Specified_String_Descriptor()
        {
            string descriptor = "some:kinda:descriptor";
            payloadReader.AddChildReader(1, descriptor, childReader);

            List<byte> bytes = new List<byte>();
            bytes.Add(DataTypeCodes.Descriptor);
            bytes.Add(DataTypeCodes.Str8);
            bytes.Add((byte)descriptor.Length);
            bytes.Add(descriptor);
            buffer.Write(bytes.ToArray());

            payloadReader.Read(dataReader);

            A.CallTo(() => childReader.Read(dataReader)).MustHaveHappened();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Read_Throws_Exception_If_No_Child_Readers_For_Descriptor()
        {
            payloadReader.AddChildReader(0, null, childReader);

            List<byte> bytes = new List<byte>();
            bytes.Add(DataTypeCodes.Descriptor);
            bytes.Add(DataTypeCodes.ULong);
            bytes.Add(1UL);
            buffer.Write(bytes.ToArray());

            payloadReader.Read(dataReader);
        }
    }
}
