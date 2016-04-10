using System;
using NUnit.Framework;
using Redcat.Core.Channels;
using FakeItEasy;
using System.IO;
using Redcat.Core.Serializaton;

namespace Redcat.Core.Tests.Chanels
{
    [TestFixture]
    public class ReactiveChannelBaseTests
    {
        private IReactiveStreamChannel streamChannel;
        private TestReactiveChannel channel;
        private Stream stream;

        private FakeDeserializer deserializer;
        private ISerializer<string> serializer;

        [SetUp]
        public void SetUp()
        {
            streamChannel = A.Fake<IReactiveStreamChannel>();
            stream = A.Fake<Stream>();
            A.CallTo(() => streamChannel.GetStream()).Returns(stream);
            deserializer = new FakeDeserializer();
            serializer = A.Fake<ISerializer<string>>();
            channel = new TestReactiveChannel(streamChannel) { DeserializerFake = deserializer, SerializerFake = serializer };
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Send_Throws_Exception_If_Channel_Does_Not_Open()
        {
            channel.Send("Hello guys:)");
        }

        [Test]
        public void Send_Serializes_Message_Into_Stream()
        {            
            string message = "Hello world";
            channel.Open();
            channel.Send(message);

            A.CallTo(() => serializer.Serialize(stream, message)).MustHaveHappened();
        }

        [Test]
        public void Rises_Received_Event_After_Deserialized()
        {            
            string actualMessage = null;

            channel.Open();
            channel.Received += (s, m) => { actualMessage = m; };
            string message = "Hello space";
            deserializer.RiseDeserialized(message);

            Assert.That(actualMessage, Is.EqualTo(message));
        }
    }

    public class TestReactiveChannel : ReactiveChannelBase<string>
    {
        public TestReactiveChannel(IReactiveStreamChannel streamChannel) : base(streamChannel, new ConnectionSettings())
        { }

        public IReactiveDeserializer<string> DeserializerFake { get; set; }

        public ISerializer<string> SerializerFake { get; set; }

        protected override IReactiveDeserializer<string> CreateDeserializer() => DeserializerFake;

        protected override ISerializer<string> CreateSerializer() => SerializerFake;
    }

    public class FakeDeserializer : IReactiveDeserializer<string>
    {
        public event Action<string> Deserialized;

        public void Read(ArraySegment<byte> binaryData)
        {
            throw new NotImplementedException();
        }

        public void RiseDeserialized(string message) => Deserialized?.Invoke(message);
    }
}
