using FakeItEasy;
using NUnit.Framework;
using Redcat.Core.Channels;
using System;

namespace Redcat.Core.Tests
{
    [TestFixture]
    public class SingleChannelCommunicatorTests
    {
        private IChannelFactory<IChannel> channelFactory;
        private SingleChannelCommunicator<IChannel> communicator;

        [SetUp]
        public void SetUp()
        {
            channelFactory = A.Fake<IChannelFactory<IChannel>>();
            communicator = new SingleChannelCommunicator<IChannel>(channelFactory);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Connect_Throws_Exception_If_Settings_Is_Null()
        {
            communicator.Connect(null);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Connect_Throws_Exception_If_CreateChannel_Returns_Null()
        {
            A.CallTo(() => channelFactory.CreateChannel(A<ConnectionSettings>._)).Returns(null);

            communicator.Connect(new ConnectionSettings());
        }

        [Test]
        public void Connect_Create_And_Open_Channel()
        {            
            IChannel channel = A.Fake<IChannel>();
            A.CallTo(() => channelFactory.CreateChannel(A<ConnectionSettings>._)).Returns(channel);            
            
            communicator.Connect(new ConnectionSettings());

            A.CallTo(() => channel.Open()).MustHaveHappened();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Connect_Throws_Exception_If_Connected()
        {
            communicator.Connect(new ConnectionSettings());
            communicator.Connect(new ConnectionSettings());
        }

        [Test]
        public void Disconnect_Closes_Created_Channel()
        {
            IChannel channel = A.Fake<IChannel>();
            A.CallTo(() => channelFactory.CreateChannel(A<ConnectionSettings>._)).Returns(channel);

            communicator.Connect(new ConnectionSettings());
            communicator.Disconnect();

            A.CallTo(() => channel.Close()).MustHaveHappened();
            Assert.That(communicator.IsConnected, Is.False);
        }
    }
}
