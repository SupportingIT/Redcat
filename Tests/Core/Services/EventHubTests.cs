using NUnit.Framework;
using Redcat.Core.Services;
using System;

namespace Redcat.Core.Tests.Services
{
    [TestFixture]
    public class EventHubTests
    {
        [Test]
        public void RiseEvent_Calls_Added_Event_Handler()
        {
            EventHub hub = new EventHub();
            bool handlerWasCalled = false;
            hub.AddEventHandler<EventArgs>("my-event", args => handlerWasCalled = true);

            hub.RiseEvent("my-event", EventArgs.Empty);

            Assert.That(handlerWasCalled, Is.True);
        }

        [Test]
        public void RiseEvent_Calls_Event_Handler_With_Correct_EventArgs_Type()
        {
            EventHub hub = new EventHub();
            bool handlerWasCalled = false;
            hub.AddEventHandler<EventArgs>("my-event", args => handlerWasCalled = true);
            hub.AddEventHandler<AssemblyLoadEventArgs>("my-event", args => handlerWasCalled = false);

            hub.RiseEvent("my-event", EventArgs.Empty);

            Assert.That(handlerWasCalled, Is.True);
        }

        [Test]
        public void RiseEvent_Dont_Calls_Removed_Event_Handler()
        {
            EventHub hub = new EventHub();
            bool handlerWasCalled = false;
            Action<EventArgs> handler = args => handlerWasCalled = true;

            hub.AddEventHandler("my-event", handler);
            hub.RemoveEventHandler("my-event", handler);
            hub.RiseEvent("my-event", EventArgs.Empty);

            Assert.That(handlerWasCalled, Is.False);
        }
    }
}
