﻿using FakeItEasy;
using NUnit.Framework;
using Redcat.Core.Channels;
using System.Linq;

namespace Redcat.Core.Tests
{
    [TestFixture]
    public class CommunicatorTests
    {
        private IChannelFactory channelFactory;
        private IMessageDispatcher dispatcher;
        private Communicator communicator;

        [SetUp]
        public void SetUp()
        {
            channelFactory = A.Fake<IChannelFactory>();
            dispatcher = A.Fake<IMessageDispatcher>();
            communicator = new Communicator(channelFactory, dispatcher);
        }

        [Test]
        public void Connect_Crates_Connection_With_Given_Name()
        {
            string connectionName = "ConnName";
            ConnectionSettings settings = new ConnectionSettings { ConnectionName = connectionName };
            communicator.Connect(settings);

            Connection connection = communicator.ActiveConnections.Single();

            Assert.That(connection.Name, Is.EqualTo(connectionName));
        }

        [Test]
        public void Connect_Opens_Created_Channel()
        {
            IChannel channel = A.Fake<IChannel>();
            A.CallTo(() => channelFactory.CreateChannel(A<ConnectionSettings>._)).Returns(channel);
            
            communicator.Connect(new ConnectionSettings());

            A.CallTo(() => channel.Open()).MustHaveHappened();
        }

        [Test]
        public void Closing_Connection_Closes_Channels_And_Removes_It_From_ActiveConnections()
        {
            IChannel channel = A.Fake<IChannel>();
            A.CallTo(() => channelFactory.CreateChannel(A<ConnectionSettings>._)).Returns(channel);            
            communicator.Connect(new ConnectionSettings());

            Connection conn = communicator.ActiveConnections.Single();
            conn.Close();

            A.CallTo(() => channel.Close()).MustHaveHappened();
            CollectionAssert.IsEmpty(communicator.ActiveConnections);
        }

        [Test]
        public void OnNext_Open_ConnectionCommand_Creates_New_Connection()
        {
            string connName = "some-name";
            ConnectionSettings settings = new ConnectionSettings { ConnectionName = connName };
            var command = ConnectionCommand.Open(settings);

            communicator.OnNext(command);

            A.CallTo(() => channelFactory.CreateChannel(settings)).MustHaveHappened();
            Assert.That(communicator.ActiveConnections.Single().Name, Is.EqualTo(connName));
        }

        [Test]
        public void OnNext_Close_ConnectionCommand_Closes_Existed_Active_Connection()
        {
            string connName = "conn-name";
            ConnectionSettings settings = new ConnectionSettings { ConnectionName = connName };
            communicator.Connect(settings);
            var command = ConnectionCommand.Close(connName);

            communicator.OnNext(command);

            CollectionAssert.IsEmpty(communicator.ActiveConnections);
        }

        [Test]
        public void Send_Uses_Dispatcher_To_Send_Messages()
        {
            communicator.Send("Hello world");

            A.CallTo(() => dispatcher.Dispatch("Hello world")).MustHaveHappened();
        }
    }
}
