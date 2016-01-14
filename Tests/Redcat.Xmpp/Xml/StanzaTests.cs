using NUnit.Framework;
using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp.Tests.Xml
{
    [TestFixture]
    public class StanzaTests
    {
        [Test]
        public void Type_Correctly_Sets_Attribute_Value()
        {
            Stanza stanza = new Stanza("some-name");

            stanza.Type = "some-type";

            Assert.That(stanza.GetAttributeValue<string>("type"), Is.EqualTo("some-type"));
        }
    }
}
