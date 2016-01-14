using FakeItEasy;
using NUnit.Framework;
using System;
using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp.Tests.Parsing
{
    [TestFixture]
    public class StanzaParserFactoryTests
    {
        [Test]
        public void CreateDatagram_Cretes_Element_With_Correct_Id_To_And_From_Attributes()
        {
            var parser = StanzaParserFactory.CreateDatagramParser(s => A.Fake<DatagramElement>());
            string to = "to@jid.com";
            string from = "from@jid.com";
            string id = Guid.NewGuid().ToString();

            parser.NewElement("datagram");
            parser.AddAttribute("to", to);
            parser.AddAttribute("from", from);
            parser.AddAttribute("id", id);
            var datagram = (DatagramElement)parser.ParsedElement;

            Assert.That(datagram.Id, Is.EqualTo(id));
            Assert.That(datagram.From, Is.EqualTo((JID)from));
            Assert.That(datagram.To, Is.EqualTo((JID)to));
        }

        [Test]
        public void CreateStanza_Creates_Element_With_Correct_Type_Attributes()
        {
            var parser = StanzaParserFactory.CreateStanzaParser(s => A.Fake<Stanza>());
            string type = "some-type";

            parser.NewElement("stanza");
            parser.AddAttribute("type", type);
            var stanza = (Stanza)parser.ParsedElement;

            Assert.That(stanza.Type, Is.EqualTo(type));
        }

        [Test]
        public void CreateMessageParser_Correctly_Handles_Basic_Nodes()
        {
            var parser = StanzaParserFactory.CreateMessageParser();
            string subject = "Hello Subj";
            string body = "body text";

            parser.NewElement("message");
            ParseNodeAndValue(parser, "subject", subject);
            ParseNodeAndValue(parser, "body", body);

            MessageStanza message = (MessageStanza)parser.ParsedElement;

            Assert.That(message.Subject, Is.EqualTo(subject));
            Assert.That(message.Body, Is.EqualTo(body));
        }

        [Test]
        public void CreateMessageParser_CanParse_Returns_True_For_Messages()
        {
            var parser = StanzaParserFactory.CreateMessageParser();

            Assert.That(parser.CanParse("message"), Is.True);
        }

        [Test]
        public void CreatePresenceParser_Correctly_Handles_Basic_Nodes()
        {
            var parser = StanzaParserFactory.CreatePresenceParser();
            string showValue = "dnd";
            string statusValue = "I'm buzzy";

            parser.NewElement("presence");
            ParseNodeAndValue(parser, "show", showValue);
            ParseNodeAndValue(parser, "status", statusValue);

            var presence = (PresenceStanza)parser.ParsedElement;

            Assert.That(presence.Show, Is.EqualTo(showValue));
            Assert.That(presence.Status, Is.EqualTo(statusValue));
        }

        [Test]
        public void CreatePresenceParser_CanParse_Returns_True_For_Presence()
        {
            var parser = StanzaParserFactory.CreatePresenceParser();

            Assert.That(parser.CanParse("presence"), Is.True);
            Assert.That(parser.CanParse("another-stanza"), Is.False);
        }

        private void ParseNodeAndValue(StreamElementParser parser, string nodeName, string nodeValue)
        {
            parser.StartNode(nodeName);
            parser.SetNodeValue(nodeValue);
            parser.EndNode();
        }
    }
}
