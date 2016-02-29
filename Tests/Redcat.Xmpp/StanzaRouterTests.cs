using FakeItEasy;
using NUnit.Framework;
using Redcat.Xmpp.Xml;
using System;

namespace Redcat.Xmpp.Tests
{
    [TestFixture]
    public class StanzaRouterTests
    {
        [Test]
        public void OnCompleted_Notifies_All_Stanza_Observers()
        {
            IObserver<IqStanza> iqObserver = A.Fake<IObserver<IqStanza>>();
            IObserver<PresenceStanza> presenceObserver = A.Fake<IObserver<PresenceStanza>>();
            IObserver<MessageStanza> messageObserver = A.Fake<IObserver<MessageStanza>>();
            StanzaRouter router = new StanzaRouter();
            router.Subscribe(iqObserver);
            router.Subscribe(presenceObserver);
            router.Subscribe(messageObserver);

            router.OnCompleted();

            A.CallTo(() => iqObserver.OnCompleted()).MustHaveHappened();
            A.CallTo(() => presenceObserver.OnCompleted()).MustHaveHappened();
            A.CallTo(() => messageObserver.OnCompleted()).MustHaveHappened();
        }

        [Test]
        public void OnNext_Notifies_All_Iq_Observers()
        {
            var observers = A.CollectionOfFake<IObserver<IqStanza>>(2);
            StanzaRouter router = new StanzaRouter();
            foreach (var observer in observers) router.Subscribe(observer);
            IqStanza iq = Iq.Get();

            router.OnNext(iq);

            foreach (var observer in observers) A.CallTo(() => observer.OnNext(iq)).MustHaveHappened();
        }

        [Test]
        public void OnNext_Notifies_All_Presence_Observers()
        {
            var observers = A.CollectionOfFake<IObserver<PresenceStanza>>(2);
            StanzaRouter router = new StanzaRouter();
            foreach (var observer in observers) router.Subscribe(observer);
            PresenceStanza presence = Presence.Available();

            router.OnNext(presence);

            foreach (var observer in observers) A.CallTo(() => observer.OnNext(presence)).MustHaveHappened();
        }

        [Test]
        public void OnNext_Notifies_All_Message_Observers()
        {
            var observers = A.CollectionOfFake<IObserver<MessageStanza>>(2);
            StanzaRouter router = new StanzaRouter();
            foreach (var observer in observers) router.Subscribe(observer);
            MessageStanza stanza = new MessageStanza();

            router.OnNext(stanza);

            foreach (var observer in observers) A.CallTo(() => observer.OnNext(stanza)).MustHaveHappened();
        }
    }
}
