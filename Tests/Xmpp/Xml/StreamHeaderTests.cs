using NUnit.Framework;
using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp.Tests.Xml
{
    [TestFixture]
    public class StreamHeaderTests
    {
        [Test]
        public void CreateClient_Returns_Correct_Element()
        {
            var header = StreamHeader.CreateClientHeader("domain.com");
            
            Assert.That(header.Name, Is.EqualTo("stream:stream"));
            Assert.That(header.Xmlns, Is.EqualTo(Namespaces.JabberClient));
            Assert.That(header.To, Is.EqualTo((JID)"domain.com"));
            Assert.That(header.GetAttributeValue<string>("xmlns:stream"), Is.EqualTo(Namespaces.Streams));
            Assert.That(header.GetAttributeValue<string>("version"), Is.EqualTo("1.0"));
        }
    }
}
