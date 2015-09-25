using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using NUnit.Framework;
using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp.Tests
{
    [TestFixture]
    public class XmppStreamWriterTests
    {
        private StringBuilder stringBuilder;

        [SetUp]
        public void Setup()
        {
            stringBuilder = new StringBuilder();
        }

        [Test]
        public void Correctly_Writes_Element_With_Only_Name()
        {
            XmppStreamWriter xmppWriter = CreateStreamWriter();
            XmlElement element = new XmlElement("some-name");

            xmppWriter.Write(element);

            XElement actualElement = XElement.Parse(stringBuilder.ToString());
            Assert.That(actualElement.Name.LocalName, Is.EqualTo("some-name"));
            Assert.That(actualElement.Attributes().Count(), Is.EqualTo(0));
            Assert.That(actualElement.Elements().Count(), Is.EqualTo(0));
        }

        [Test]
        public void Correctly_Writes_Element_Name_And_Xmlns()
        {
            XmppStreamWriter xmppWriter = CreateStreamWriter();
            XmlElement element = new XmlElement("some-name", "name-space");

            xmppWriter.Write(element);

            XElement actualElement = XElement.Parse(stringBuilder.ToString());
            Assert.That(actualElement.Name.LocalName, Is.EqualTo("some-name"));
            Assert.That(actualElement.Attribute("xmlns").Value, Is.EqualTo("name-space"));
        }

        [Test]
        public void Correctly_Writes_Attributes()
        {
            XmppStreamWriter xmppWriter = CreateStreamWriter();
            XmlElement element = new XmlElement("element");
            element.SetAttributeValue("attribute1", "value1");
            element.SetAttributeValue("attribute2", 2);

            xmppWriter.Write(element);

            XElement actualElement = XElement.Parse(stringBuilder.ToString());
            Assert.That(actualElement.Attributes().Count(), Is.EqualTo(2));
            Assert.That(actualElement.Attribute("attribute1").Value, Is.EqualTo("value1"));
            Assert.That(actualElement.Attribute("attribute2").Value, Is.EqualTo("2"));
        }

        [Test]
        public void Correctly_Writes_Xmlns_And_Elements_Value()
        {
            XmppStreamWriter xmppWriter = CreateStreamWriter();
            XmlElement element = new XmlElement("name", "namespace") {Value = "Some-Value"};

            xmppWriter.Write(element);

            XElement actualElement = XElement.Parse(stringBuilder.ToString());
            Assert.That(actualElement.Value, Is.EqualTo("Some-Value"));
        }

        private XmppStreamWriter CreateStreamWriter()
        {
            TextWriter writer = new StringWriter(stringBuilder);
            return new XmppStreamWriter(writer);
        }
    }
}
