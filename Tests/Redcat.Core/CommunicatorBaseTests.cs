using FakeItEasy;
using NUnit.Framework;
using Redcat.Core.Channels;
using System;

namespace Redcat.Core.Tests
{
    [TestFixture]
    public class CommunicatorBaseTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Connect_ThrowsException_If_ConnectionSettings_Is_Null()
        {
            CommunicatorBase communicator = CreateCommunicator();

            communicator.Connect(null);
        }

        [Test]
        public void Connect_Creates_Channel_Via_Provided_Factory()
        {
            IChannelFactory factory = A.Fake<IChannelFactory>();
            IChannel channel = A.Fake<IChannel>();
            ConnectionSettings settings = new ConnectionSettings();
            A.CallTo(() => factory.CreateChannel(settings)).Returns(channel);
            var communicator = CreateCommunicator(factory);
            var eventHandler = A.Fake<EventHandler<IChannel>>();
            communicator.ChannelCreated += eventHandler;

            communicator.Connect(settings);

            A.CallTo(() => eventHandler.Invoke(communicator, channel));
        }

        [Test]
        public void Connect_IsConnected_True()
        {
            var communicator = CreateCommunicator();

            Assert.That(communicator.IsConnected, Is.False);
            communicator.Connect(new ConnectionSettings());
            Assert.That(communicator.IsConnected, Is.True);
        }

        private CommunicatorBase CreateCommunicator(IChannelFactory factory = null)
        {
            return A.Fake<CommunicatorBase>(o =>
            {
                o.CallsBaseMethods();
                if (factory != null) o.WithArgumentsForConstructor(new object[] { factory });
            });
        }
    }
}
