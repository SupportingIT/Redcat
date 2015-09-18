using FakeItEasy;
using NUnit.Framework;
using Redcat.Core.Services;
using System;

namespace Redcat.Core.Tests
{
    [TestFixture]
    public class CommunicatorTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Connect_Throws_Exception_If_ConnectionSetings_Is_Null()
        {
            Communicator communicator = new Communicator();
            communicator.Connect(null);
        }

        [Test]
        public void Connect_Creates_Channel_Using_ChanelManager()
        {
            IChannelManager manager = A.Fake<IChannelManager>();
            Communicator communicator = CreateAndRunCommunicator(manager);
            ConnectionSettings settings = new ConnectionSettings();

            communicator.Connect(settings);

            A.CallTo(() => manager.OpenChannel(A<ConnectionSettings>._)).MustHaveHappened();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Connect_Throws_Exception_If_Run_Was_Not_Called_Before()
        {
            Communicator communicator = new Communicator();
            ConnectionSettings settings = new ConnectionSettings();
            communicator.Connect(settings);
        }

        [Test]
        public void Run_Installs_ChannelManager()
        {
            IKernelExtension extension = A.Fake<IKernelExtension>();
            IChannelManager manager = null;
            A.CallTo(() => extension.Attach(A<IKernel>._)).Invokes(c => {
                IKernel kernel = c.GetArgument<IKernel>(0);
                manager = kernel.GetService<IChannelManager>();
            });
            Communicator communicator = new Communicator();
            communicator.AddExtension(extension);

            communicator.Run();

            Assert.That(manager, Is.Not.Null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Send_Throws_Exception_If_Message_Is_Null()
        {
            Communicator communicator = new Communicator();
            communicator.Send(null);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Send_Throws_Exception_If_Default_Channel_Is_Null()
        {
            IChannelManager manager = A.Fake<IChannelManager>();
            A.CallTo(() => manager.DefaultChannel).Returns(null);
            Communicator communicator = CreateAndRunCommunicator(manager);

            communicator.Send(new Message());
        }

        [Test]
        public void Send_Sends_Message_Throught_Default_Channel()
        {
            IMessageChannel channel = A.Fake<IMessageChannel>();
            IChannelManager manager = A.Fake<IChannelManager>();
            A.CallTo(() => manager.DefaultChannel).Returns(channel);
            Communicator communicator = CreateAndRunCommunicator(manager);
            Message message = new Message();

            communicator.Send(message);

            A.CallTo(() => channel.Send(message)).MustHaveHappened();
        }

        private Communicator CreateAndRunCommunicator(IChannelManager manager)
        {
            Communicator communicator = new Communicator();            
            var extension = CreateExtension(manager);
            communicator.AddExtension(extension);
            communicator.Run();
            return communicator;
        }

        private IKernelExtension CreateExtension(IChannelManager manager)
        {
            IServiceProvider provider = A.Fake<IServiceProvider>();
            A.CallTo(() => provider.GetService(typeof(IChannelManager))).Returns(manager);
            IKernelExtension extension = A.Fake<IKernelExtension>();
            A.CallTo(() => extension.Attach(A<IKernel>._)).Invokes(c =>
            {
                IKernel kernel = c.GetArgument<IKernel>(0);
                kernel.Providers.Clear();
                kernel.Providers.Add(provider);
            });
            return extension;
        }
    }
}
