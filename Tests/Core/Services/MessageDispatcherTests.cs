using FakeItEasy;
using NUnit.Framework;
using Redcat.Core.Services;
using System;

namespace Redcat.Core.Tests.Services
{
    [TestFixture]
    public class MessageDispatcherTests
    {
        [Test]
        public void DispatchIncoming_Calls_All_Handlers()
        {
            VerifyDispatcher(true);
        }

        [Test]
        public void DispatchOutgoing_Calls_All_Handlers()
        {
            VerifyDispatcher(false);
        }

        private void VerifyDispatcher(bool isIncoming)
        {
            var handlers = A.CollectionOfFake<Action<Message>>(2);
            MessageDispatcher dispatcher = new MessageDispatcher();
            foreach(var handler in handlers)
            {
                if (isIncoming) dispatcher.IncomingMessageHandlers.Add(handler);
                else dispatcher.OutgoingMessageHandlers.Add(handler);
            }
            Message message = new Message();

            if (isIncoming) dispatcher.DispatchIncoming(message);
            else dispatcher.DispatchOutgoing(message);

            foreach (var handler in handlers) A.CallTo(() => handler.Invoke(message)).MustHaveHappened();
        }
    }
}
