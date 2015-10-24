using NUnit.Framework;
using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp.Tests.Xml
{
    [TestFixture]
    public class StreamFeatureExtensionTests
    {
        [Test]
        public void HasTlsFeature_Returns_False_If_No_Tls_Feature()
        {
            var elements = new[] { new XmlElement("e1"), new XmlElement("e2") };

            bool hasFeature = elements.HasTlsFeature();

            Assert.That(hasFeature, Is.False);
        }

        [Test]
        public void  HasTlsFeature_Returns_True_If_Sequence_Contains_Tls_Feature()
        {
            var elements = new[] { new XmlElement("e34"), Tls.Start, new XmlElement("j43") };

            bool hasFeature = elements.HasTlsFeature();

            Assert.That(hasFeature, Is.True);
        }

        [Test]
        public void TlsFeature_Returns_Null_If_No_Tls_Feature()
        {
            var elements = new[] { new XmlElement("e1"), new XmlElement("e2") };

            var feature = elements.TlsFeature();

            Assert.That(feature, Is.Null);
        }

        [Test]
        public void TlsFeature_Returns_Tls_Feature_If_sequence_Contains_It()
        {
            var elements = new[] { new XmlElement("e34"), Tls.Start, new XmlElement("j43") };

            var feature = elements.TlsFeature();

            Assert.That(feature.Name, Is.EqualTo("starttls"));
            Assert.That(feature.Xmlns, Is.EqualTo(Namespaces.Tls));
        }
    }
}
