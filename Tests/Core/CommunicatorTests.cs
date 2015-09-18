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
            IKernelExtension extension = CreateExtension(manager);
            Communicator communicator = new Communicator();
            communicator.AddExtension(extension);
            communicator.Run();
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
