using NUnit.Framework;
using Redcat.Xmpp.Parsing;
using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp.Tests.Parsing
{
    [TestFixture]
    public class StanzaBuilderTests
    {
        [Test]
        public void Creates_IqStanza_For_Iq_Elements()
        {
            StanzaBuilder builder = new StanzaBuilder();

            builder.NewElement("iq");

            Assert.That(builder.Element, Is.TypeOf<IqStanza>());
        }

        [Test]
        public void Creates_PresenceStanza_For_Presence_Element()
        {
            StanzaBuilder builder = new StanzaBuilder();

            builder.NewElement("presence");

            Assert.That(builder.Element, Is.TypeOf<PresenceStanza>());
        }

        [Test]
        public void Creates_MessageStanza_For_Message_Element()
        {
            StanzaBuilder builder = new StanzaBuilder();

            builder.NewElement("message");

            Assert.That(builder.Element, Is.TypeOf<MessageStanza>());
        }

        [Test]
        public void Parses_From_Attributes_Into_Jid()
        {
            StanzaBuilder builder = new StanzaBuilder();
            string from = "from@domain.com";

            builder.NewElement("iq");
            builder.AddAttribute("from", from);
            var stanza = (Stanza)builder.Element;

            Assert.That(stanza.From, Is.EqualTo((JID)from));
        }

        [Test]
        public void Parses_To_Attribute_Into_Jid()
        {
            StanzaBuilder builder = new StanzaBuilder();
            string to = "to@domain.com";

            builder.NewElement("iq");
            builder.AddAttribute("to", to);
            var stanza = (Stanza)builder.Element;

            Assert.That(stanza.To, Is.EqualTo((JID)to));
        }
    }
}
