using NUnit.Framework;
using Redcat.Xmpp.Xml;
using System;

namespace Redcat.Xmpp.Tests.Xml
{
    [TestFixture]
    public class DatagramElementTests
    {
        [Test]
        public void Correctly_Sets_To_And_From_Attributes()
        {            
            JID to = "to@jid.com";
            JID from = "from@jid.com";
            DatagramElement element = new DatagramElement("element") { From = from, To = to };

            Assert.That(element.GetAttributeValue<JID>("to"), Is.EqualTo(to));
            Assert.That(element.GetAttributeValue<JID>("from"), Is.EqualTo(from));
        }

        [Test]
        public void Correctly_Sets_Id_Attribute()
        {
            object id = Guid.NewGuid();
            DatagramElement element = new DatagramElement("element") { Id = id };

            Assert.That(element.GetAttributeValue<object>("id"), Is.EqualTo(id));
        }

        [Test]
        public void Correctly_Sets_XmlLang_Attribute()
        {
            string xmlLang = "testLang";
            DatagramElement element = new DatagramElement("name") { XmlLang = xmlLang };

            Assert.That(element.GetAttributeValue<string>("xml:lang"), Is.EqualTo(xmlLang));
        }

        [Test]
        public void NewId_Generates_New_Value_For_Id_Attribute()
        {
            var element = new DatagramElement("you").NewId();

            Assert.That(element.Id, Is.Not.Null);
        }
    }
}
