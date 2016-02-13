using FakeItEasy;
using NUnit.Framework;
using Redcat.Core.Channels;
using System;

namespace Redcat.Core.Tests
{
    [TestFixture]
    public class MessageDispatcherTests
    {
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Dispatch_Throws_Exception_If_ChannelProvider_Returns_Null()
        {
            IOutputChannelProvider provider = A.Fake<IOutputChannelProvider>();
            A.CallTo(() => provider.GetChannel(A<string>._)).Returns(null);
            MessageDispatcher dispatcher = new MessageDispatcher(provider);

            dispatcher.Dispatch("Hello world");
        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void Dispatch_Throws_Exceptions_If_Message_Is_Null()
        {
            MessageDispatcher dispatcher = new MessageDispatcher(A.Fake<IOutputChannelProvider>());
            dispatcher.Dispatch<string>(null);
        }

        [Test]
        public void Dispatch_Sends_Message_Via_Channel()
        {
            IOutputChannelProvider provider = A.Fake<IOutputChannelProvider>();
            IOutputChannel<string> channel = A.Fake<IOutputChannel<string>>();
            A.CallTo(() => provider.GetChannel(A<string>._)).Returns(channel);
            MessageDispatcher dispatcher = new MessageDispatcher(provider);

            dispatcher.Dispatch("Hello world");

            A.CallTo(() => channel.Send("Hello world")).MustHaveHappened();
        }
    }
}
