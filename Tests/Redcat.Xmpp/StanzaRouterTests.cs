using FakeItEasy;
using NUnit.Framework;
using Redcat.Core.Channels;
using Redcat.Xmpp.Xml;
using System;

namespace Redcat.Xmpp.Tests
{
    [TestFixture]
    public class StanzaRouterTests
    {
        [Test]
        public void OnXmlElementReceived_Notifies_Iq_Handler()
        {
            IqStanzaHandler handler = A.Fake<IqStanzaHandler>();
            StanzaRouter router = new StanzaRouter(handler, null, null);
            IqStanza iq = Iq.Get();

            router.OnXmlElementReceived(router, new ChannelMessageEventArgs<XmlElement>(iq));

            A.CallTo(() => handler.Invoke(iq)).MustHaveHappened();
        }

        [Test]
        public void OnXmlElementReceived_Notifies_Presence_Handler()
        {
            PresenceStanzaHandler handler = A.Fake<PresenceStanzaHandler>();
            StanzaRouter router = new StanzaRouter(null, handler, null);            
            PresenceStanza presence = Presence.Available();

            router.OnXmlElementReceived(router, new ChannelMessageEventArgs<XmlElement>(presence));

            A.CallTo(() => handler.Invoke(presence)).MustHaveHappened();
        }

        [Test]
        public void OnXmlElementReceived_Notifies_Message_Handler()
        {
            MessageStanzaHandler handler = A.Fake<MessageStanzaHandler>();
            StanzaRouter router = new StanzaRouter(null, null, handler);            
            MessageStanza stanza = new MessageStanza();

            router.OnXmlElementReceived(router, new ChannelMessageEventArgs<XmlElement>(stanza));

            A.CallTo(() => handler.Invoke(stanza)).MustHaveHappened();
        }
    }
}
