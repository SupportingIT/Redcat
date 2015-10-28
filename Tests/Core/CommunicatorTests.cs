using FakeItEasy;
using NUnit.Framework;
using Redcat.Core.Communication;
using Redcat.Core.Service;
using System;

namespace Redcat.Core.Tests
{
    [TestFixture]
    public class CommunicatorTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Connect_Throws_Exception_If_Setting_Argument_Is_Null()
        {
            Communicator communicator = new Communicator();
            communicator.Connect(null);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Connect_Throws_Exception_If_Run_Method_Was_Not_Called()
        {
            ConnectionSettings settings = new ConnectionSettings();
            Communicator communicator = new Communicator();
            communicator.Connect(settings);
        }

        [Test]
        public void Connect_Creates_Correct_Type_Of_Channel()
        {
            ConnectionSettings settings = new ConnectionSettings { ChannelType = "test" };
            Communicator communicator = new Communicator();
            IChannelFactory factory = A.Fake<TestChannelFactory>();
            communicator.AddExtension("test", c => 
            {
                c.TryAddSingleton(factory);
            });
            communicator.Run();

            communicator.Connect(settings);

            A.CallTo(() => factory.CreateChannel(settings)).MustHaveHappened();
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Send_Throws_Exception_If_Message_Argument_Is_Null()
        {
            Communicator communicator = new Communicator();
            communicator.Send(null);
        }

        [Test]
        public void Send_Uses_MessageDispatcher_To_Send_Message()
        {
            ConnectionSettings settings = new ConnectionSettings();
            Communicator communicator = new Communicator();
            IMessageDispatcher dispatcher = A.Fake<IMessageDispatcher>();
            communicator.AddExtension("test", c =>
            {
                c.Replace(ServiceDescriptor.Instance(A.Fake<IChannelManager>()));
                c.Replace(ServiceDescriptor.Instance(dispatcher));
            });
            communicator.Run();
            communicator.Connect(settings);
            Message message = new Message();

            communicator.Send(message);

            A.CallTo(() => dispatcher.DispatchOutgoing(message)).MustHaveHappened();
        }
    }

    public class TestChannelFactory : IChannelFactory
    {
        public virtual IMessageChannel CreateChannel(ConnectionSettings settings)
        {
            throw new NotImplementedException();
        }
    }
}
