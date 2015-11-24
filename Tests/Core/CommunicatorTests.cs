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
        public void Start_Adds_Extensions()
        {
            Communicator communicator = new Communicator();
            Action<IServiceCollection> extension = A.Fake<Action<IServiceCollection>>();
            communicator.AddExtension("test", extension);

            A.CallTo(() => extension.Invoke(A<IServiceCollection>._)).MustNotHaveHappened();
            communicator.Start();

            A.CallTo(() => extension.Invoke(A<IServiceCollection>._)).MustHaveHappened();
        }

        [Test]
        public void Start_Initializes_Extensions_Only_Once()
        {
            Communicator communicator = new Communicator();
            Action<IServiceCollection> extension = A.Fake<Action<IServiceCollection>>();
            communicator.AddExtension("test", extension);

            communicator.Start();
            communicator.Start();

            A.CallTo(() => extension.Invoke(A<IServiceCollection>._)).MustHaveHappened(Repeated.Exactly.Once);
        }

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
            communicator.Start();

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
        [ExpectedException(typeof(InvalidOperationException))]
        public void Send_Throws_Exception_If_Run_Method_Was_Not_Called()
        {
            Communicator communicator = new Communicator();
            communicator.Send(new Message());
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
            communicator.Start();
            communicator.Connect(settings);
            Message message = new Message();

            communicator.Send(message);

            A.CallTo(() => dispatcher.Dispatch(message)).MustHaveHappened();
        }
    }

    public class TestChannelFactory : IChannelFactory
    {
        public virtual IChannel CreateChannel(ConnectionSettings settings)
        {
            throw new NotImplementedException();
        }
    }
}
