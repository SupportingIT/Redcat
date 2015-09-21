using System.Xml.Linq;
using NUnit.Framework;
using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp.Tests.Xml
{
    [TestFixture]
    public class DatagramElementTests : DatagramElementTestBase
    {
        #region Equals method test

        [Test]
        public void Equals_FromPropertiesEquals_ReturnsTrue()
        {
            DatagramElement datagram1 = CreateDatagram("from@jid.com", null, null, null);
            DatagramElement datagram2 = CreateDatagram("from@jid.com", null, null, null);

            Assert.That(datagram1.Equals(datagram2), Is.True);
        }

        [Test]
        public void Equals_FromPropertiesNotEquals_ReturnsFalse()
        {
            DatagramElement datagram1 = CreateDatagram("from@jid.com", null, null, null);
            DatagramElement datagram2 = CreateDatagram("from@jid2.com", null, null, null);

            Assert.That(datagram1.Equals(datagram2), Is.False);
        }

        [Test]
        public void Equals_ToPropertiesEquals_ReturnsTrue()
        {
            DatagramElement datagram1 = CreateDatagram(null, "to@jid.com", null, null);
            DatagramElement datagram2 = CreateDatagram(null, "to@jid.com", null, null);

            Assert.That(datagram1.Equals(datagram2), Is.True);
        }

        [Test]
        public void Equals_ToPropertiesNotEquals_ReturnsFalse()
        {
            DatagramElement datagram1 = CreateDatagram(null, "to@jid.com", null, null);
            DatagramElement datagram2 = CreateDatagram(null, "to2@jid.org", null, null);

            Assert.That(datagram1.Equals(datagram2), Is.False);
        }

        [Test]
        public void Equals_IdPropertiesEquals_ReturnsTrue()
        {
            DatagramElement datagram1 = CreateDatagram(null, null, "some-id", null);
            DatagramElement datagram2 = CreateDatagram(null, null, "some-id", null);

            Assert.That(datagram1.Equals(datagram2), Is.True);
        }

        [Test]
        public void Equals_IdPropertiesNotEquals_ReturnsFalse()
        {
            DatagramElement datagram1 = CreateDatagram(null, null, "some-id", null);
            DatagramElement datagram2 = CreateDatagram(null, null, "another-id", null);

            Assert.That(datagram1.Equals(datagram2), Is.False);
        }

        [Test]
        public void Equals_LangPropertiesEquals_ReturnsTrue()
        {
            DatagramElement datagram1 = CreateDatagram(null, null, null, "en");
            DatagramElement datagram2 = CreateDatagram(null, null, null, "en");

            Assert.That(datagram1.Equals(datagram2), Is.True);
        }

        [Test]
        public void Equals_LangPropertiesNotEquals_ReturnsFalse()
        {
            DatagramElement datagram1 = CreateDatagram(null, null, null, "en");
            DatagramElement datagram2 = CreateDatagram(null, null, null, "ru");

            Assert.That(datagram1.Equals(datagram2), Is.False);
        }

        [Test]
        public void Equals_DifferentAttributeSet_ReturnsFalse()
        {
            DatagramElement datagram1 = CreateDatagram("from@jid.com", "to@jid.com", null, "us");
            DatagramElement datagram2 = CreateDatagram("from@jid.com", null, 1, null);

            Assert.That(datagram1.Equals(datagram2), Is.False);
        }

        #endregion

        #region Write method tests
        [Test]
        public void Write_IdNotEmpty_WritesIdAttribute()
        {
            VerifyStanzaXml(null, null, Id, null);
        }

        [Test]
        public void Write_FromNotEmpty_WritesFromAttribute()
        {
            VerifyStanzaXml(FromJid, null, null, null);
        }

        [Test]
        public void Write_ToNotEmpty_WritesToAttribute()
        {
            VerifyStanzaXml(null, ToJid, null, null);
        }

        [Test]
        public void Write_XmlLangNotEmpty_WritesFromAttribute()
        {
            VerifyStanzaXml(null, null, null, "en");
        }

        [Test]
        public void Write_AllPropertiesNotEmpty_WritesAllAttribute()
        {
            VerifyStanzaXml(FromJid, ToJid, Id, "ua");
        }

        private void VerifyStanzaXml(JID from, JID to, object id, string xmlLang)
        {
            DatagramElement stanza = CreateDatagram(from, to, id, xmlLang);
            XElement expectedXml = CreateDatagramXElement(from, to, id, xmlLang);
            VerifyWriteOutput(expectedXml, stanza);
        }

        #endregion

        protected virtual DatagramElement CreateDatagram(JID from, JID to, object id, string xmlLang)
        {
            DatagramElement element = CreateStreamElementMock<DatagramElement>(Name);
            element.Id = id;
            element.From = from;
            element.To = to;
            element.XmlLang = xmlLang;
            return element;
        }
    }
}
