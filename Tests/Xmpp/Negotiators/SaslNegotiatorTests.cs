using NUnit.Framework;
using Redcat.Xmpp.Negotiators;
using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp.Tests.Negotiators
{
    [TestFixture]
    public class SaslNegotiatorTests
    {
        [Test]
        public void CanNegoatiate_Returns_True_For_Sasl_Feature()
        {
            SaslNegotiator negoatiator = new SaslNegotiator();
            XmlElement sasl = new XmlElement("mechanisms", Namespaces.Sasl);

            Assert.That(negoatiator.CanNeogatiate(sasl), Is.True);
        }
    }
}
