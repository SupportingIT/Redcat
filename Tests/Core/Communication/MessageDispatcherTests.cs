using FakeItEasy;
using NUnit.Framework;
using Redcat.Core.Communication;
using System;
using System.Linq;

namespace Redcat.Core.Tests.Communication
{
    [TestFixture]
    public class MessageDispatcherTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_Throws_Exception_If_Argument_Is_Null()
        {
            MessageDispatcher dispatcher = new MessageDispatcher(null);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Dispatch_Throws_Exception_If_No_ActiveChannels()
        {
            IChannelManager manager = A.Fake<IChannelManager>();            
            MessageDispatcher dispatcher = new MessageDispatcher(manager);

            dispatcher.Dispatch("some-string");
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Dispatch_Throws_Exception_If_No_Output_Channels_For_Aproriate_Massage()
        {            
            IOutputChannel<int> channel = A.Fake<IOutputChannel<int>>();
            IChannelManager manager = CreateChannelManager(channel);
            MessageDispatcher dispatcher = new MessageDispatcher(manager);

            dispatcher.Dispatch("some-string");
        }

        [Test]
        public void Dispatch_Sends_Message_Via_Approriate_Output_Channel()
        {            
            IOutputChannel<Guid> guidChannel = A.Fake<IOutputChannel<Guid>>();
            IOutputChannel<string> stringChannel = A.Fake<IOutputChannel<string>>();
            IChannelManager manager = CreateChannelManager(guidChannel, stringChannel);
            MessageDispatcher dispatcher = new MessageDispatcher(manager);
            Guid message = Guid.NewGuid();

            dispatcher.Dispatch(message);

            A.CallTo(() => guidChannel.Send(message)).MustHaveHappened();
        }

        [Test]
        public void Dispatch_Sends_Message_Via_Default_Channel()
        {
            IOutputChannel<int> defaultChannel = A.Fake<IOutputChannel<int>>();
            IOutputChannel<int> activeChannel = A.Fake<IOutputChannel<int>>();
            IChannelManager manager = CreateChannelManager(activeChannel);
            A.CallTo(() => manager.DefaultChannel).Returns(defaultChannel);
            MessageDispatcher dispatcher = new MessageDispatcher(manager);
            int message = 8;

            dispatcher.Dispatch(message);

            A.CallTo(() => defaultChannel.Send(message)).MustHaveHappened();
            A.CallTo(() => activeChannel.Send(message)).MustNotHaveHappened();
        }

        private IChannelManager CreateChannelManager(params IChannel[] activeChannels)
        {
            IChannelManager manager = A.Fake<IChannelManager>();
            if (activeChannels.Length > 0) A.CallTo(() => manager.ActiveChannels).Returns(activeChannels);
            return manager;
        }
    }
}
