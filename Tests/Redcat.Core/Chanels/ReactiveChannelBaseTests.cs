using System;
using NUnit.Framework;
using Redcat.Core.Channels;
using FakeItEasy;
using System.IO;

namespace Redcat.Core.Tests.Chanels
{
    [TestFixture]
    public class ReactiveChannelBaseTests
    {
        [Test]
        public void Send_Serializes_Message_Into_Stream()
        {
            var deserializer = A.Fake<IReactiveDeserializer<string>>();
            var serializer = A.Fake<ISerializer<string>>();
            var streamChannel = A.Fake<IReactiveStreamChannel>();
            var stream = A.Fake<Stream>();
            A.CallTo(() => streamChannel.GetStream()).Returns(stream);

            var channel = new TestReactiveChannel(streamChannel) { Deserializer = deserializer, Serializer = serializer };
            string message = "Hello world";
            channel.Send(message);

            A.CallTo(() => serializer.Serialize(stream, message)).MustHaveHappened();
        }

        [Test]
        public void Rises_Received_Event_After_Deserialized()
        {
            Assert.Fail();
        }
    }

    public class TestReactiveChannel : ReactiveChannelBase<string>
    {
        public TestReactiveChannel(IReactiveStreamChannel streamChannel) : base(streamChannel, new ConnectionSettings())
        { }

        public IReactiveDeserializer<string> Deserializer { get; set; }

        public ISerializer<string> Serializer { get; set; }

        protected override IReactiveDeserializer<string> CreateDeserializer() => Deserializer;

        protected override ISerializer<string> CreateSerializer() => Serializer;
    }
}
