using FakeItEasy;
using NUnit.Framework;
using Redcat.Core.Channels;
using System;

namespace Redcat.Core.Tests
{
    [TestFixture]
    public class SingleChannelCommunicatorTests
    {
        private IChannelFactory channelFactory;
        private SingleChannelCommunicator communicator;

        [SetUp]
        public void SetUp()
        {
            channelFactory = A.Fake<IChannelFactory>();
            communicator = new SingleChannelCommunicator(channelFactory);
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
        public void Disconnect_Closes_Created_Channel()
        {
            IChannel channel = A.Fake<IChannel>();
            A.CallTo(() => channelFactory.CreateChannel(A<ConnectionSettings>._)).Returns(channel);

            communicator.Connect(new ConnectionSettings());
            communicator.Disconnect();

            A.CallTo(() => channel.Close()).MustHaveHappened();
        }

        [Test]
        public void Disconnect_Does_Nothing_If_Channel_Is_Null()
        {
            A.CallTo(() => channelFactory.CreateChannel(A<ConnectionSettings>._)).Returns(null);

            communicator.Disconnect();
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Send_Throws_Exception_If_Message_Is_Null()
        {
            communicator.Connect(new ConnectionSettings());
            communicator.Send<string>(null);
        }

        [Test]
        public void Send_Uses_Channel_To_Send_Message()
        {
            IOutputChannel<string> channel = A.Fake<IOutputChannel<string>>();
            A.CallTo(() => channelFactory.CreateChannel(A<ConnectionSettings>._)).Returns(channel);

            communicator.Connect(new ConnectionSettings());
            communicator.Send("Hello world");

            A.CallTo(() => channel.Send("Hello world")).MustHaveHappened();
        }
        
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Send_Throws_Exception_If_Channel_Doesnt_Support_Message_Type()
        {
            var channel = A.Fake<IOutputChannel<string>>();
            A.CallTo(() => channelFactory.CreateChannel(A<ConnectionSettings>._)).Returns(channel);

            communicator.Connect(new ConnectionSettings());

            communicator.Send(new object());
        }        
    }
}
